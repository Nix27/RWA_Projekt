using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private byte[]? GetFileAsMemoryStream(IFormFile file)
        {
            if(file != null)
            {
                if(file.Length > 0)
                {
                    using(var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);

                        if(memoryStream.Length < 50 * 1024 * 1024)
                        {
                            return memoryStream.ToArray();
                        }
                    }
                }
            }

            return null;
        }

        public ImageDto Create(IFormFile image)
        {
            var imageAsStream = GetFileAsMemoryStream(image);

            Image newImage = new Image();

            if(imageAsStream != null)
            {
                newImage.Content = Convert.ToBase64String(imageAsStream);
            }

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

        public ImageDto? Update(int id, IFormFile image)
        {
            var imageForUpdate = _unitOfWork.Image.GetFirstOrDefault(i => i.Id == id);

            if(imageForUpdate == null) return null;

            var imageAsStream = GetFileAsMemoryStream(image);

            if (imageAsStream != null)
            {
                imageForUpdate.Content = Convert.ToBase64String(imageAsStream);
            }

            _unitOfWork.Save();

            return ImageMapping.MapToDto(imageForUpdate);
        }
    }
}
