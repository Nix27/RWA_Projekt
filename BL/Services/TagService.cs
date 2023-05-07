using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TagDto Create(TagDto tag)
        {
            var newTag = TagMapping.FromDto(tag);

            _unitOfWork.Tag.Add(newTag);
            _unitOfWork.Save();

            return TagMapping.MapToDto(newTag);
        }

        public TagDto? Delete(int id)
        {
            var tagForDelete = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == id);

            if (tagForDelete == null) return null;

            _unitOfWork.Tag.Delete(tagForDelete);
            _unitOfWork.Save();

            return TagMapping.MapToDto(tagForDelete);
        }

        public TagDto? Get(int id)
        {
            var requestedTag = _unitOfWork.Tag.GetFirstOrDefault(t =>t.Id == id);

            if (requestedTag == null) return null;

            return TagMapping.MapToDto(requestedTag);
        }

        public ICollection<TagDto> GetAll()
        {
            var allTags = _unitOfWork.Tag.GetAll();

            return TagMapping.MapToDto(allTags).ToList();
        }

        public TagDto? Update(int id, TagDto tag)
        {
            var tagForUpdate = _unitOfWork.Tag.GetFirstOrDefault(t => t.Id == id);

            if (tagForUpdate == null) return null;

            tagForUpdate.Name = tag.Name;

            _unitOfWork.Save();

            return TagMapping.MapToDto(tagForUpdate);
        }
    }
}
