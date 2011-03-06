using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineImageLibrary.Domain.Entities;

namespace OnlineImageLibrary.Domain.Abstract
{
    public interface IImagesRepository
    {
        IQueryable<Image> Images { get; }
        bool SaveImage(Image image);
        bool DeleteImage(Image image);
    }
}
