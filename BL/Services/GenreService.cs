using BL.DTO;
using BL.Mapping;
using DAL.IRepositories;
using DAL.Repositories;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GenreDto Create(GenreDto genre)
        {
            var newGenre = GenreMapping.FromDto(genre);

            _unitOfWork.Genre.Add(newGenre);
            _unitOfWork.Save();

            return GenreMapping.MapToDto(newGenre);
        }

        public GenreDto? Delete(int id)
        {
            var genreForDelete = _unitOfWork.Genre.GetFirstOrDefault(g => g.Id == id);

            if (genreForDelete == null) return null;

            _unitOfWork.Genre.Delete(genreForDelete);
            _unitOfWork.Save();

            return GenreMapping.MapToDto(genreForDelete);
        }

        public GenreDto? Get(int id)
        {
            var requestedGenre = _unitOfWork.Genre.GetFirstOrDefault(g => g.Id == id);

            if (requestedGenre == null) return null;

            return GenreMapping.MapToDto(requestedGenre);
        }

        public ICollection<GenreDto> GetAll()
        {
            var allGenres = _unitOfWork.Genre.GetAll();

            return GenreMapping.MapToDto(allGenres).ToList();
        }

        public GenreDto? Update(int id, GenreDto genre)
        {
            var genreForUpdate = _unitOfWork.Genre.GetFirstOrDefault(g => g.Id == id);

            if (genreForUpdate == null) return null;

            genreForUpdate.Name = genre.Name;
            genreForUpdate.Description = genre.Description;

            _unitOfWork.Save();

            return GenreMapping.MapToDto(genreForUpdate);
        }
    }
}
