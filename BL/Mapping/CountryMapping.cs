using BL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class CountryMapping
    {
        public static IEnumerable<CountryDto> MapToDto(IEnumerable<Country> countries) => countries.Select(c => MapToDto(c));

        public static CountryDto MapToDto(Country country)
        {
            return new CountryDto
            {
                Id = country.Id,
                Name = country.Name,
                Code = country.Code
            };
        }

        public static IEnumerable<Country> FromDto(IEnumerable<CountryDto> countryDtos) => countryDtos.Select(c => FromDto(c));

        public static Country FromDto(CountryDto countryDto)
        {
            return new Country
            {
                Id = countryDto.Id ?? 0,
                Name = countryDto.Name,
                Code = countryDto.Code
            };
        }
    }
}
