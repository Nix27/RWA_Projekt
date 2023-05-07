using BL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class UserMapping
    {
        public static IEnumerable<UserDto> MapToDto(IEnumerable<User> users) => users.Select(u => MapToDto(u));

        public static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                CountryId = user.CountryId,
                Country = user.Country?.Name
            };
        }

        public static IEnumerable<User> FromDto(IEnumerable<UserDto> userDtos) => userDtos.Select(u => FromDto(u));

        public static User FromDto(UserDto userDto)
        {
            return new User
            {
                Id = userDto.Id ?? 0,
                Username = userDto.UserName,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Phone = userDto.Phone,
                CountryId = userDto.CountryId
            };
        }
    }
}
