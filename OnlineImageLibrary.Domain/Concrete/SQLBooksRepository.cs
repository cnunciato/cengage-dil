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
    public class SQLBooksRepository : IBooksRepository
    {
        private Table<Book> booksTable;

        public SQLBooksRepository(string connectionString)
        {
            booksTable = (new DataContext(connectionString).GetTable<Book>());
        }

        public IQueryable<Book> Books
        {
            get { return booksTable; }
        }
    }
}
