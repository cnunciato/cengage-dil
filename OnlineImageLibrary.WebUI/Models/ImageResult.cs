using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace OnlineImageLibrary.WebUI.Models
{
    public class ImageResult : FileStreamResult
    {
        public ImageResult(Image input) : this(input, input.Width) { }

        public ImageResult(Image input, int width) : base(GetMemoryStream(input, width), "image/png")
        { 
            
        }

        static MemoryStream GetMemoryStream(Image input, int width)
        {
            var height = input.Height * width / input.Width;
            var bmp = new Bitmap(input, width, height);
            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }
    } 

}
