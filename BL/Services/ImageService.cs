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
    public class ImageService : IImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ImageDto Create(ImageDto image)
        {
            var newImage = ImageMapping.FromDto(image);

            _unitOfWork.Image.Add(newImage);
            _unitOfWork.Save();

            return ImageMapping.MapToDto(newImage);
        }

        public ImageDto? Delete(int id)
        {
            var imageForDelete = _unitOfWork.Image.GetFirstOrDefault(i => i.Id == id);

            if(imageForDelete == null) return null;

            _unitOfWork.Image.Delete(imageForDelete);
            _unitOfWork.Save();

            return ImageMapping.MapToDto(imageForDelete);
        }

        public ImageDto? Get(int id)
        {
            var requestedImage = _unitOfWork.Image.GetFirstOrDefault(i => i.Id == id);

            if(requestedImage == null) return null;

            return ImageMapping.MapToDto(requestedImage);
        }

        public ICollection<ImageDto> GetAll()
        {
            var allImages = _unitOfWork.Image.GetAll();

            return ImageMapping.MapToDto(allImages).ToList();
        }
    }
}
