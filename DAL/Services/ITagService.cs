using DAL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public interface ITagService
    {
        ICollection<TagDto> GetAll();
        TagDto? Get(int id);
        TagDto Create(TagDto tag);
        TagDto? Update(int id, TagDto tag);
        TagDto? Delete(int id);
    }
}
