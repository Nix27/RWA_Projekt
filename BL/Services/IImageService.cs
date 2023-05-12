using BL.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IImageService
    {
        ImageDto? Get(int id);
        ImageDto Create(IFormFile image);

        ImageDto? Update(int id, IFormFile image);
    }
}
