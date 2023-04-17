using DAL.ApplicationDbContext;
using DAL.IRepositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public void Update(Genre genre)
        {
            _dbContext.Genres.Update(genre);
        }
    }
}
