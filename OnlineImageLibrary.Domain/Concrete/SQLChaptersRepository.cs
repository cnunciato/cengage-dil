using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineImageLibrary.Domain;
using OnlineImageLibrary.Domain.Abstract;
using OnlineImageLibrary.Domain.Entities;
using System.Data.Linq;

namespace OnlineImageLibrary.Domain.Concrete
{
    public class SQLChaptersRepository : IChaptersRepository
    {
        private Table<Chapter> chaptersTable;

        public SQLChaptersRepository(string connectionString)
        {
            chaptersTable = (new DataContext(connectionString).GetTable<Chapter>());
        }

        public IQueryable<Chapter> Chapters
        {
            get { return chaptersTable; }
        }
    }
}
