using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineImageLibrary.Domain.Entities;

namespace OnlineImageLibrary.Domain.Abstract
{
    public interface IBooksRepository
    {
        IQueryable<Book> Books { get; }
    }
}
