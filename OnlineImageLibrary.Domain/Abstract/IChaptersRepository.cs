using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineImageLibrary.Domain.Entities;

namespace OnlineImageLibrary.Domain.Abstract
{
    public interface IChaptersRepository
    {
        IQueryable<Chapter> Chapters { get; }
    }
}
