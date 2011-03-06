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
    public class SQLImagesRepository : IImagesRepository
    {
        private Table<Image> imagesTable;

        public SQLImagesRepository(string connectionString)
        {
            imagesTable = (new DataContext(connectionString).GetTable<Image>());
        }

        public IQueryable<Image> Images
        {
            get { return imagesTable; }
        }

        public bool SaveImage(Image image)
        {
            try
            {
                if (image.ImageID == 0)
                {
                    imagesTable.InsertOnSubmit(image);
                }
                else if (imagesTable.GetOriginalEntityState(image) == null)
                {
                    imagesTable.Attach(image);
                    imagesTable.Context.Refresh(RefreshMode.KeepCurrentValues, image);
                }

                imagesTable.Context.SubmitChanges();

                return true;
            }
            catch (Exception e)
            {

            }

            return false;
        }

        public bool DeleteImage(Image image)
        {
            try
            {
                if (image.ImageID == 0)
                {
                    return false;
                }
                else if (imagesTable.GetOriginalEntityState(image) == null)
                {
                    imagesTable.Attach(image);
                    imagesTable.DeleteOnSubmit(image);
                    imagesTable.Context.Refresh(RefreshMode.KeepCurrentValues, image);
                }

                imagesTable.Context.SubmitChanges();

                

                return true;
            }
            catch (Exception e)
            {

            }

            return false;
        }
    }
}
