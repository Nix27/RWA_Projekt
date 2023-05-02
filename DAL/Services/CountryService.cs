using DAL.DTO;
using DAL.IRepositories;
using DAL.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
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
