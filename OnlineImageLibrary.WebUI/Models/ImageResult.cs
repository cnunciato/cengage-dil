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
        public ImageResult(Image input, int width) : base(GetMemoryStream(input, width), "image/png") { }
        public ImageResult(Image input, int x, int y, int width, int height) : base(GetMemoryStream(input, x, y, width, height), "image/png") { }

        static MemoryStream GetMemoryStream(Image input, int width)
        {
            var height = input.Height * width / input.Width;
            var bmp = new Bitmap(input, width, height);
            var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            ms.Position = 0;
            return ms;
        }

        static MemoryStream GetMemoryStream(Image input, int x, int y, int width, int height)
        {
            var bmp = new Bitmap(width, height);
            bmp.SetResolution(input.HorizontalResolution, input.VerticalResolution);

            var ms = new MemoryStream();
            var graphic = Graphics.FromImage(bmp);
            graphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            graphic.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            graphic.DrawImage(input, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            bmp.Save(ms, input.RawFormat);
            ms.Position = 0;
            return ms;
        }
    } 

}
