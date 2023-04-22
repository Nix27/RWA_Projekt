using Azure.Core;
using DAL.DTO;
using DAL.IRepositories;
using DAL.Mapping;
using DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

namespace DAL.Services
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
                throw new Exception("User already exists");

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
                IsConfirmed = true,
                SecurityToken = b64SecToken,
                Phone = user.Phone,
                Role = user.Role ?? "User",
                CountryId = user.CountryId
            };

            _unitOfWork.UserRepo.Add(newUser);
            _unitOfWork.Save();

            return UserMapping.MapToDto(newUser);
        }

        private bool Authenticate(string email, string password)
        {
            var user =_unitOfWork.UserRepo.GetFirstOrDefault(u => u.Email == email);

            if (user == null) return false;
            if(!user.IsConfirmed) return false;

            byte[] salt = Convert.FromBase64String(user.PwdSalt);
            byte[] hash = Convert.FromBase64String(user.PwdHash);

            byte[] calcHash =
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8);

            return hash.SequenceEqual(calcHash);
        }

        private string GetRole(string email)
        {
            return _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Email == email).Role;
        }

        public string GetToken(string email, string password)
        {
            var isAuthenticated = Authenticate(email, password);

            if (!isAuthenticated)
                throw new Exception("Authentication failed");

            var jwtKey = _configuration["Jwt:Key"];
            var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKey);
            var role = GetRole(email);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new System.Security.Claims.Claim[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.Email, email),
                    new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, email),
                    new System.Security.Claims.Claim(ClaimTypes.Role, role)
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

            return tokenHandler.WriteToken(token);
        }

        public void ChangePassword(ChangePasswordRequest request)
        {
            var isAuthenticated = Authenticate(request.Email, request.OldPassword);

            if (!isAuthenticated)
                throw new Exception("Authentication failed");

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

            var user = _unitOfWork.UserRepo.GetFirstOrDefault(u => u.Email == request.Email);
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
    }
}
