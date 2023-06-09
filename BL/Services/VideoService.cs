using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class VideoService : IVideoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VideoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public VideoDto Create(VideoDto video)
        {
            var newVideo = VideoMapping.FromDto(video);
            var tags = _unitOfWork.Tag.GetAll(t => video.Tags.Contains(t.Name));
            newVideo.VideoTags = tags.Select(t => new VideoTag { Tag = t }).ToList();

            _unitOfWork.Video.Add(newVideo);
            _unitOfWork.Save();

            return VideoMapping.MapToDto(newVideo);
        }

        public VideoDto? Delete(int id)
        {
            var videoForDelete = _unitOfWork.Video.GetFirstOrDefault(v => v.Id == id, includeProperties: "Image,VideoTags.Tag");

            if (videoForDelete == null) return null;

            _unitOfWork.Video.Delete(videoForDelete);
            _unitOfWork.Save();

            return VideoMapping.MapToDto(videoForDelete);
        }

        public VideoDto? Get(int id)
        {
            var requestedVideo = _unitOfWork.Video.GetFirstOrDefault(v => v.Id == id, includeProperties: "Image,VideoTags.Tag");

            if (requestedVideo == null) return null;

            return VideoMapping.MapToDto(requestedVideo);
        }

        public ICollection<VideoDto> GetAll()
        {
            var allVideos = _unitOfWork.Video.GetAll(includeProperties: "Image,VideoTags.Tag");

            return VideoMapping.MapToDto(allVideos).ToList();
        }

        public VideoDto? Update(int id, VideoDto video)
        {
            var videoForUpdate = _unitOfWork.Video.GetFirstOrDefault(v => v.Id == id, includeProperties: "Genre,Image,VideoTags.Tag");

            videoForUpdate.Name = video.Name;
            videoForUpdate.Description = video.Description;
            videoForUpdate.TotalSeconds = video.TotalSeconds;
            videoForUpdate.StreamingURL = video.StreamingURL;
            videoForUpdate.GenreId = video.GenreId;
            videoForUpdate.ImageId = video.ImageId;

            var videoTagsForRemove = videoForUpdate.VideoTags.Where(vt => !video.Tags.Contains(vt.Tag.Name));
            foreach (var videoTag in videoTagsForRemove)
            {
                _unitOfWork.VideoTag.Delete(videoTag);
            }

            var existingTagNames = videoForUpdate.VideoTags.Select(vt => vt.Tag.Name);
            var newTagNames = video.Tags.Except(existingTagNames);
            foreach (var newTagName in newTagNames)
            {
                var tagFromDb = _unitOfWork.Tag.GetFirstOrDefault(t => newTagName == t.Name);
                if (tagFromDb == null) continue;

                videoForUpdate.VideoTags.Add(new VideoTag
                {
                    Video = videoForUpdate,
                    Tag = tagFromDb
                });
            }

            _unitOfWork.Save();

            return VideoMapping.MapToDto(videoForUpdate);
        }

        public ICollection<VideoDto> Search(int size, int page, string? filterNames, string? orderBy, string? direction)
        {
            var videos = _unitOfWork.Video.GetAll(includeProperties: "Image,VideoTags.Tag");

            //filtering
            if(filterNames != null)
                videos = videos.Where(v => v.Name.Contains(filterNames));

            //ordering
            if (String.Compare(orderBy, "name", true) == 0)
                videos = videos.OrderBy(v => v.Name);
            else if (String.Compare(orderBy, "totalSeconds", true) == 0)
                videos = videos.OrderBy(v => v.TotalSeconds);
            else
                videos = videos.OrderBy(v => v.Id);

            //direction
            if (String.Compare(direction, "desc", true) == 0)
                videos = videos.Reverse();

            //paging
            videos = videos.Skip(size * page).Take(size);

            return VideoMapping.MapToDto(videos).ToList();
        }

        public ICollection<Video> GetAllForView()
        {
            return _unitOfWork.Video.GetAll(includeProperties: "Genre,Image,VideoTags.Tag").ToList();
        }

        public IEnumerable<VideoDto> GetPagedVideos(int page, int size)
        {
            var allVideos = _unitOfWork.Video.GetAll(includeProperties: "Genre,Image,VideoTags.Tag");

            var pagedVideos = allVideos.Skip(page * size).Take(size);

            return VideoMapping.MapToDto(pagedVideos);
        }

        public IEnumerable<VideoDto> GetFilteredVideos(IEnumerable<VideoDto> videos, string? filterBy, string? filter)
        {
            filter = filter?.ToLower();

            if(filter != null)
            {
                if (String.Compare(filterBy, "name", true) == 0)
                {
                    videos = videos.Where(v => v.Name.ToLower().Contains(filter));
                }
                else if (String.Compare(filterBy, "genre", true) == 0)
                {
                    videos = videos.Where(v => v.Genre.ToLower().Contains(filter));
                }
            }
            
            return videos;
        }

        public int GetNumberOfVideos() => _unitOfWork.Video.GetAll().Count();
    }
}
