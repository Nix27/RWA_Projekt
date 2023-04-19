﻿using DAL.ApplicationDbContext;
using DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Video = new VideoRepository(_db);
            Genre= new GenreRepository(_db);
            Tag = new TagRepository(_db);
            VideoTag = new VideoTagsRepository(_db);
        }

        public IVideoRepository Video { get; private set; }

        public IGenreRepository Genre { get; private set; }

        public ITagRepository Tag { get; private set; }

        public IVideoTagsRepository VideoTag { get; private set; }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
