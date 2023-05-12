﻿using BL.DTO;
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
    }
}