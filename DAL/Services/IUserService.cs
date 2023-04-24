using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IUserService
    {
        ICollection<UserDto> GetAll();
        UserDto? Get(int id);
        UserDto Create(UserRegisterRequest user);
        UserDto? Update(int id, UserDto user);
        UserDto? Delete(int id);
        void ValidateEmail(ValidateEmilRequest request);
        string GetToken(LoginRequest request);
        void ChangePass(ChangePasswordRequest request);
    }
}
