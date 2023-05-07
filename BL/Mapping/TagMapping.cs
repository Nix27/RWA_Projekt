using BL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class TagMapping
    {
        public static IEnumerable<TagDto> MapToDto(IEnumerable<Tag> tags) => tags.Select(t => MapToDto(t));

        public static TagDto MapToDto(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public static IEnumerable<Tag> FromDto(IEnumerable<TagDto> tagDtos) => tagDtos.Select(t => FromDto(t));

        public static Tag FromDto(TagDto tagDto)
        {
            return new Tag
            {
                Id = tagDto.Id ?? 0,
                Name = tagDto.Name
            };
        }
    }
}
