using BL.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IImageService
    {
        ICollection<ImageDto> GetAll();
        ImageDto? Get(int id);
        ImageDto Create(IFormFile image);
        ImageDto? Delete(int id);

        ImageDto? Update(int id, IFormFile image);
    }
}
