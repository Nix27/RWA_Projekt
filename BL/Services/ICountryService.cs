using BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICountryService
    {
        ICollection<CountryDto> GetAll();
        CountryDto? Get(int id);
        IEnumerable<CountryDto> GetPagedCountries(int page, int size);
        int GetNumberOfCountries();
        CountryDto Create(CountryDto country);
        CountryDto? Update(int id, CountryDto country);
        CountryDto? Delete(int id);
    }
}
