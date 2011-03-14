using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineImageLibrary.Domain.Abstract;
using OnlineImageLibrary.Domain.Concrete;
using OnlineImageLibrary.Domain.Entities;
using System.Configuration;
using System.ComponentModel;

namespace OnlineImageLibrary.WebUI.Controllers
{
    public class DataController : Controller
    {
        private IImagesRepository imagesRepository;

        public DataController() 
        {
            imagesRepository = new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString);
        }

        public JsonResult Books()
        {
            return Json(new SQLBooksRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Books.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Chapters()
        {
            return Json(new SQLChaptersRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Chapters.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Images()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Artists()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.Select(a => a.Artist).Distinct().ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Media()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.Select(a => a.Medium).Distinct().ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Themes()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.Select(a => a.Theme).Distinct().ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Periods()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.Select(a => a.Period).Distinct().ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Origins()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.Select(a => a.Origin).Distinct().ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Collections()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.Select(a => a.Collection).Distinct().ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult New()
        {
            return Json(new SQLImagesRepository(ConfigurationManager.ConnectionStrings["appDB"].ConnectionString).Images.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
