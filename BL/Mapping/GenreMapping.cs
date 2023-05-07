using BL.DTO;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class GenreMapping
    {
        public static IEnumerable<GenreDto> MapToDto(IEnumerable<Genre> genres) => genres.Select(g => MapToDto(g));

        public static GenreDto MapToDto(Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            };
        }

        public static IEnumerable<Genre> FromDto(IEnumerable<GenreDto> genreDtos) => genreDtos.Select(g => FromDto(g));

        public static Genre FromDto(GenreDto genreDto)
        {
            return new Genre
            {
                Id = genreDto.Id ?? 0,
                Name = genreDto.Name,
                Description = genreDto.Description
            };
        }
    }
}
