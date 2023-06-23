using Azure.Core;
using BL.DTO;
using BL.Mapping;
using BL.Models;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public UserDto Create(UserRegisterRequest user)
        {
            var existingUser = _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Username == user.UserName);

            if (existingUser != null)
                throw new InvalidOperationException("User already exists");

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);
            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: user.Password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            byte[] securityToken = RandomNumberGenerator.GetBytes(256 / 8);
            string b64SecToken = Convert.ToBase64String(securityToken);

            var newUser = new User
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PwdHash = b64Hash,
                PwdSalt = b64Salt,
                IsConfirmed = false,
                SecurityToken = b64SecToken,
                Phone = user.Phone,
                Role = user.Role ?? "User",
                CountryId = user.CountryId
            };

            var encodedEmail = HttpUtility.UrlEncode(newUser.Email);
            var encodedSecurityToken = HttpUtility.UrlEncode(newUser.SecurityToken);

            var notificationForValidateEmail = new Notification
            {
                ReceiverEmail = newUser.Email,
                Subject = "Email verification",
                Body = $"To verify your e-mail address please click on link: https://localhost:44318/api/Users/ValidateEmail?Email={encodedEmail}&B64SecToken={encodedSecurityToken}"
            };

            _unitOfWork.UserRepo.Add(newUser);
            _unitOfWork.Notification.Add(notificationForValidateEmail);
            _unitOfWork.Save();

            return UserMapping.MapToDto(newUser);
        }

        public void ValidateEmail(ValidateEmilRequest request)
        {
            var decodedEmail = HttpUtility.UrlDecode(request.Email);
            var decodedSecurityToken = HttpUtility.UrlDecode(request.B64SecToken);

            var foundUser = _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Email == request.Email && u.SecurityToken == request.B64SecToken);

            if (foundUser == null)
                throw new Exception("Authentication failed");

            foundUser.IsConfirmed = true;
            _unitOfWork.Save();
        }

        private User? Authenticate(string email, string password)
        {
            var user =_unitOfWork.UserRepo.GetFirstOrDefault(u => u.Email == email && u.IsConfirmed);

            if (user == null) return null;

            byte[] salt = Convert.FromBase64String(user.PwdSalt);
            byte[] hash = Convert.FromBase64String(user.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash) ? user : null;
        }

        public string GetToken(LoginRequest request)
        {
            var user = Authenticate(request.Email, request.Password);

            if (user == null)
                throw new Exception("Authentication failed");

            var jwtKey = _configuration["Jwt:Key"];
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);
            var role = user.Role;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(JwtRegisteredClaimNames.Sub, request.Email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(jwtKeyBytes),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public UserDto? GetConfirmedUser(LoginRequest request)
        {
            var confirmedUser = Authenticate(request.Email, request.Password);

            return confirmedUser != null ? UserMapping.MapToDto(confirmedUser) : null;
        }

        public void ChangePass(ChangePasswordRequest request)
        {
            var user = Authenticate(request.Email, request.OldPassword);

            if (user == null)
                throw new InvalidOperationException("Authentication failed");

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string b64Salt = Convert.ToBase64String(salt);

            byte[] hash =
                KeyDerivation.Pbkdf2(
                    password: request.NewPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);
            string b64Hash = Convert.ToBase64String(hash);

            user.PwdSalt = b64Salt;
            user.PwdHash = b64Hash;

            _unitOfWork.Save();
        }

        public UserDto? Delete(int id)
        {
            var userForDelete = _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false);

            if(userForDelete == null) return null;

            userForDelete.IsDeleted = true;
            userForDelete.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Save();

            return UserMapping.MapToDto(userForDelete);
        }

        public UserDto? Get(int id)
        {
            var user = _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false, includeProperties: "Country");

            if(user == null) return null;

            return UserMapping.MapToDto(user);
        }

        public ICollection<UserDto> GetAll()
        {
            var allUsers = _unitOfWork.UserRepo.GetAll(u => u.IsDeleted == false, includeProperties: "Country");
            return UserMapping.MapToDto(allUsers).ToList();
        }

        public UserDto? Update(int id, UserDto user)
        {
            var userForUpdate = _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false, includeProperties: "Country");

            if(userForUpdate == null) return null;

            userForUpdate.Username = user.UserName;
            userForUpdate.FirstName = user.FirstName;
            userForUpdate.LastName = user.LastName;
            userForUpdate.Email = user.Email;
            userForUpdate.Phone = user.Phone;
            userForUpdate.CountryId = user.CountryId;

            _unitOfWork.Save();

            return UserMapping.MapToDto(userForUpdate);
        }

        public IEnumerable<UserDto> GetPagedUsers(int page, int size)
        {
            var allUsers = _unitOfWork.UserRepo.GetAll(u => !u.IsDeleted, includeProperties: "Country");

            var pagedUsers = allUsers.Skip(page * size).Take(size);

            return UserMapping.MapToDto(pagedUsers);
        }

        public IEnumerable<UserDto> GetFilteredUsers(IEnumerable<UserDto> users, string? filterBy, string? filter)
        {
            filter = filter?.ToLower();

            if(filter != null)
            {
                if (String.Compare(filterBy, "firstname", true) == 0)
                {
                    users = users.Where(u => u.FirstName.ToLower().Contains(filter));
                }
                else if (String.Compare(filterBy, "lastname", true) == 0)
                {
                    users = users.Where(u => u.LastName.ToLower().Contains(filter));
                }
                else if (String.Compare(filterBy, "username", true) == 0)
                {
                    users = users.Where(u => u.UserName.ToLower().Contains(filter));
                }
                else if (String.Compare(filterBy, "country", true) == 0)
                {
                    users = users.Where(u => u.Country.ToLower().Contains(filter));
                }
            }
            
            return users;
        }

        public int GetNumberOfUsers() => _unitOfWork.UserRepo.GetAll(u => u.IsConfirmed && !u.IsDeleted).Count();
    }
}
