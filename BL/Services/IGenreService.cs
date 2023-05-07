using BL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IGenreService
    {
        ICollection<GenreDto> GetAll();
        GenreDto? Get(int id);
        GenreDto Create(GenreDto genre);
        GenreDto? Update(int id, GenreDto genre);
        GenreDto? Delete(int id);
    }
}
