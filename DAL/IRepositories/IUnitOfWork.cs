using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.IRepositories
{
    public interface IUnitOfWork
    {
        IVideoRepository Video { get; }
        IGenreRepository Genre { get; }
        ITagRepository Tag { get; }
        IVideoTagsRepository VideoTag { get; }
        IUserRepository UserRepo { get; }
        INotificationRepository Notification { get; }

        void Save();
    }
}
