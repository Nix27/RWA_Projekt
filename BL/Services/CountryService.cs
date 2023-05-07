using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
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
    }
}
