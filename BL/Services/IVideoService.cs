using BL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IVideoService
    {
        ICollection<VideoDto> GetAll();
        ICollection<Video> GetAllForView();
        VideoDto? Get(int id);
        VideoDto Create(VideoDto video);
        VideoDto? Update(int id, VideoDto video);
        VideoDto? Delete(int id);
        ICollection<VideoDto> Search(int size, int page, string? filterNames, string? orderBy, string? direction);
    }
}
