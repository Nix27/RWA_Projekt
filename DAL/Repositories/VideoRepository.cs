using DAL.ApplicationDbContext;
using DAL.DTO;
using DAL.IRepositories;
using DAL.Mapping;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly AppDbContext _dbContext;

        public VideoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public VideoDto Add(VideoDto video)
        {
            var dbVideo = VideoMapping.FromDto(video);
            var dbTags = _dbContext.Tags.Where(t => video.Tags.Contains(t.Name));
            dbVideo.VideoTags = dbTags.Select(t => new VideoTag { Tag = t }).ToList();

            _dbContext.Videos.Add(dbVideo);

            return VideoMapping.MapToDto(dbVideo);
        }

        public void Delete(Video video)
        {
            _dbContext.Remove(video);
        }

        public void DeleteRange(IEnumerable<Video> videos)
        {
            _dbContext.RemoveRange(videos);
        }

        public IEnumerable<VideoDto> GetAll(Expression<Func<Video, bool>>? filter = null, string? includeProperties = null)
        {
            var dbVideos = _dbContext.Videos;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbVideos.Include(includeProperty);
                } 
            }

            var videos = VideoMapping.MapToDto(dbVideos);

            return videos;
        }

        public VideoDto GetFirstOrDefault(Expression<Func<Video, bool>> filter, string? includeProperties = null)
        {
            var dbVideos = _dbContext.Videos;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    dbVideos.Include(includeProperty);
                }
            }

            var video = dbVideos.Where(filter).FirstOrDefault();

            return VideoMapping.MapToDto(video);
        }

        public void Update(int id, VideoDto video)
        {
            var dbVideo = _dbContext.Videos.FirstOrDefault(v => v.Id == id);

            dbVideo.Name = video.Name;
            dbVideo.Description = video.Description;
            dbVideo.TotalSeconds = video.TotalSeconds;
            dbVideo.StreamingURL = video.StreamingURL;
            dbVideo.GenreId = video.GenreId;
            dbVideo.ImageId = video.ImageId;
            
            var toRemove = dbVideo.VideoTags.Where(vt => !video.Tags.Contains(vt.Tag.Name));
            foreach (var videoTag in toRemove)
            {
                _dbContext.VideoTags.Remove(videoTag);
            }

            var existingDbTagNames = dbVideo.VideoTags.Select(vt => vt.Tag.Name);
            var newTagNames = video.Tags.Except(existingDbTagNames);
            foreach (var newTagName in newTagNames)
            {
                var dbTag = _dbContext.Tags.FirstOrDefault(t => newTagName == t.Name);
                if (dbTag == null)
                    continue;

                dbVideo.VideoTags.Add(new VideoTag
                {
                    Video = dbVideo,
                    Tag = dbTag
                });
            }
        }
    }
}
