using DAL.ApplicationDbContext;
using DAL.IRepositories;
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
    public class VideoRepository : Repository<Video>, IVideoRepository
    {
        public VideoRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
