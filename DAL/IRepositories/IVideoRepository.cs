using DAL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IVideoRepository
    {
        IEnumerable<VideoDto> GetAll(Expression<Func<Video, bool>>? filter = null, string? includeProperties = null);
        VideoDto GetFirstOrDefault(Expression<Func<Video, bool>> filter, string? includeProperties = null);
        VideoDto Add(VideoDto video);
        VideoDto Update(int id, VideoDto video);
        VideoDto Delete(Video video);
        IEnumerable<Video> DeleteRange(IEnumerable<Video> videos);
    }
}
