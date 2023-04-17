using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Mapping
{
    public class VideoMapping
    {
        public static IEnumerable<VideoDto> MapToDto(IEnumerable<Video> videos) => videos.Select(v => MapToDto(v));

        public static VideoDto MapToDto(Video video)
        {
            return new VideoDto
            {
                Id = video.Id,
                Name = video.Name,
                Description = video.Description,
                TotalSeconds = video.TotalSeconds,
                StreamingURL = video.StreamingURL,
                GenreId = video.GenreId,
                ImageId = video.ImageId,
                Tags = video.VideoTags.Select(vt => vt.Tag.Name).ToList()
            };
        }

        public static IEnumerable<Video> FromDto(IEnumerable<VideoDto> videoDtos) => videoDtos.Select(v => FromDto(v));

        public static Video FromDto(VideoDto videoDto)
        {
            return new Video
            {
                Id = videoDto.Id ?? 0,
                Name = videoDto.Name,
                Description = videoDto.Description,
                TotalSeconds = videoDto.TotalSeconds,
                StreamingURL = videoDto.StreamingURL,
                GenreId = videoDto.GenreId,
                ImageId = videoDto.ImageId
            };
        }
    }
}
