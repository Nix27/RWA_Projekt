using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface IVideoService
    {
        ICollection<VideoDto> GetAll();
        VideoDto? Get(int id);
        VideoDto Create(VideoDto video);
        VideoDto? Update(int id, VideoDto video);
        VideoDto? Delete(int id);
    }
}
