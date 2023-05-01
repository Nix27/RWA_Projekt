using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public class ImageMapping
    {
        public static IEnumerable<ImageDto> MapToDto(IEnumerable<Image> images) => images.Select(i => MapToDto(i));

        public static ImageDto MapToDto(Image image)
        {
            return new ImageDto
            {
                Id = image.Id,
                Content = image.Content
            };
        }

        public static IEnumerable<Image> FromDto(IEnumerable<ImageDto> imageDtos) => imageDtos.Select(i => FromDto(i));

        public static Image FromDto(ImageDto imageDto)
        {
            return new Image
            {
                Id = imageDto.Id ?? 0,
                Content = imageDto.Content
            };
        }
    }
}
