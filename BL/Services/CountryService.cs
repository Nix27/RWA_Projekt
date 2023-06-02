using Azure;
using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ICollection<CountryDto> GetAll()
        {
            var allCountries = _unitOfWork.Country.GetAll();

            return CountryMapping.MapToDto(allCountries).ToList();
        }

        public IEnumerable<CountryDto> GetPagedCountries(int page, int size)
        {
            var allCountries = _unitOfWork.Country.GetAll();

            var pagedCountries = allCountries.Skip(page * size).Take(size);

            return CountryMapping.MapToDto(pagedCountries);
        }

        public int GetNumberOfCountries() => _unitOfWork.Country.GetAll().Count();

        public CountryDto Create(CountryDto country)
        {
            var newCountry = CountryMapping.FromDto(country);

            _unitOfWork.Country.Add(newCountry);
            _unitOfWork.Save();

            return CountryMapping.MapToDto(newCountry);
        }

        public CountryDto? Update(int id, CountryDto country)
        {
            var countryForUpdate = _unitOfWork.Country.GetFirstOrDefault(c => c.Id == id);

            if (countryForUpdate == null) return null;

            countryForUpdate.Name = country.Name;
            countryForUpdate.Code = country.Code;

            _unitOfWork.Save();

            return CountryMapping.MapToDto(countryForUpdate);
        }

        public CountryDto? Get(int id)
        {
            var requestedCountry = _unitOfWork.Country.GetFirstOrDefault(c => c.Id == id);

            if (requestedCountry == null) return null;

            return CountryMapping.MapToDto(requestedCountry);
        }

        public CountryDto? Delete(int id)
        {
            var countryForDelete = _unitOfWork.Country.GetFirstOrDefault(c => c.Id == id);

            if (countryForDelete == null) return null;

            _unitOfWork.Country.Delete(countryForDelete);
            _unitOfWork.Save();

            return CountryMapping.MapToDto(countryForDelete);
        }
    }
}
