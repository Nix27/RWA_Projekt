using BL.DTO;
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
        ImageDto Create(ImageDto image);
        ImageDto? Delete(int id);
    }
}
