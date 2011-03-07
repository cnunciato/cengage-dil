using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineImageLibrary.Domain.Concrete;
using System.Configuration;
using OnlineImageLibrary.Domain.Abstract;
using System.Web.UI.WebControls;
using System.Drawing;
using Ionic.Zip;
using System.IO;

namespace OnlineImageLibrary.WebUI.Controllers
{
    public class AssetController : Controller
    {
        private IImagesRepository imagesRepository;

        public AssetController()
        {
            imagesRepository = new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString);
        }

        [HttpPost]
        public JsonResult Upload(OnlineImageLibrary.Domain.Entities.Image image)
        {
            object o = SaveImage(image);
            Request.Files[0].SaveAs(Server.MapPath("/") + ConfigurationManager.AppSettings["uploadPath"].ToString() + "/" + image.ImageID.ToString() + ".jpg");
            return Json(o);
        }

        [HttpPost]
        public JsonResult Save(OnlineImageLibrary.Domain.Entities.Image image)
        {
            return Json(SaveImage(image));
        }

        [HttpPost]
        public JsonResult Delete(OnlineImageLibrary.Domain.Entities.Image image)
        {
            bool result = imagesRepository.DeleteImage(image);

            if (result)
            {
                System.IO.File.Delete(Server.MapPath("/") + ConfigurationManager.AppSettings["uploadPath"].ToString() + "/" + image.ImageID.ToString() + ".jpg");
            }

            return Json(result);
        }

        private object SaveImage(OnlineImageLibrary.Domain.Entities.Image image)
        {
            return new { success = imagesRepository.SaveImage(image), assetID = image.ImageID };
        }

        public ActionResult Thumb(int id, int width)
        {
            System.Drawing.Image i = null;

            try
            {
                i = System.Drawing.Image.FromFile(Server.MapPath("/") + ConfigurationManager.AppSettings["uploadPath"].ToString() + "/" + id.ToString() + ".jpg");
                return new OnlineImageLibrary.WebUI.Models.ImageResult(i, width);
            }
            catch (Exception ex)
            {
                i = new Bitmap(width, width);
                return new OnlineImageLibrary.WebUI.Models.ImageResult(i, width);
            }
            finally
            {
                if (i != null) i.Dispose();
            }   
        }

        public ActionResult Crop(int id, int x, int y, int width, int height)
        {
            System.Drawing.Image i = null;

            try
            {
                i = System.Drawing.Image.FromFile(Server.MapPath("/") + ConfigurationManager.AppSettings["uploadPath"].ToString() + "/" + id.ToString() + ".jpg");
                return new OnlineImageLibrary.WebUI.Models.ImageResult(i, x, y, width, height);
            }
            catch (Exception ex)
            {
                i = new Bitmap(width, height);
                return new OnlineImageLibrary.WebUI.Models.ImageResult(i, width);
            }
            finally
            {
                if (i != null) i.Dispose();
            }
        }

        public void Zip(List<string> ids)
        {
            ZipFile zip = new ZipFile();
            
            foreach (string id in ids)
            {
                zip.AddFile(Server.MapPath("/") + ConfigurationManager.AppSettings["uploadPath"].ToString() + "/" + id.ToString() + ".jpg", "/");
            }

            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment; filename=\"Export.zip\"");
            zip.Save(Response.OutputStream);
        }
    }
}
