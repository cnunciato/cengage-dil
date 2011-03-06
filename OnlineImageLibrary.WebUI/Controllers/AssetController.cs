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
//using DocumentFormat.OpenXml;
//using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Validation;
//using DocumentFormat.OpenXml.Presentation;
//using System.Drawing.Imaging;
//using DocumentFormat.OpenXml;
//using a = DocumentFormat.OpenXml.Drawing;

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

        //public void Pptx(List<string> ids)
        //{
        //    Response.ContentType = "application/vnd.ms-powerpoint";
        //    Response.AddHeader("Content-Disposition", "attachment; filename=\"Presentation.pptx\"");

        //    string newPresentation = System.Guid.NewGuid().ToString() + ".pptx";
        //    string presentationTemplate = "PresentationTemplate.pptx";
        //    string presentationFolder = Server.MapPath("/") + @"\Media\PowerPoint\";
        //    string imageFolder = Server.MapPath("/") + @"\Media\Images\";
        //    string[] imageFileExtensions = new[] { "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.png", "*.tif" };

        //    System.IO.File.Copy(presentationFolder + presentationTemplate, presentationFolder + newPresentation, true);

        //    List<string> imageFileNames = GetImageFileNames(imageFolder, imageFileExtensions);

        //    if (imageFileNames.Count() > 0)
        //        CreateSlides(imageFileNames, presentationFolder + newPresentation);

        //    //OpenXmlValidator validator = new OpenXmlValidator();
        //    //var errors = validator.Validate(PresentationDocument.Open(presentationFolder + newPresentation, true));

        //    /*
        //    if (errors.Count() > 0)
        //    {
        //        Console.WriteLine("The deck creation process completed but " +
        //          "the created presentation failed to validate.");
        //        Console.WriteLine("There are " + errors.Count() +
        //          " errors:\r\n");

        //        DisplayValidationErrors(errors);
        //    }
        //    else
        //        Console.WriteLine("The deck creation process completed and " +
        //          "the created presentation validated with 0 errors.");
        //    */

            
        //    //Response.WriteFile(presentationFolder + newPresentation);
        //}

        //private void CreateSlides(List<string> imageFileNames, string newPresentation)
        //{
        //    string relId;
        //    SlideId slideId;

        //    // Slide identifiers have a minimum value of greater than or
        //    // equal to 256 and a maximum value of less than 2147483648.
        //    // Assume that the template presentation being used has no slides.
        //    uint currentSlideId = 256;

        //    string imageFileNameNoPath;

        //    long imageWidthEMU = 0;
        //    long imageHeightEMU = 0;

            
        //    PresentationDocument newDeck = PresentationDocument.Open(newPresentation, true);

        //        PresentationPart presentationPart = newDeck.PresentationPart;

        //        // Reuse the slide master part. This code assumes that the
        //        // template presentation being used has at least one
        //        // master slide.
        //        var slideMasterPart = presentationPart.SlideMasterParts.First();

        //        // Reuse the slide layout part. This code assumes that the
        //        // template presentation being used has at least one
        //        // slide layout.
        //        var slideLayoutPart = slideMasterPart.SlideLayoutParts.First();

        //        // If the new presentation doesn't have a SlideIdList element
        //        // yet then add it.
        //        if (presentationPart.Presentation.SlideIdList == null)
        //            presentationPart.Presentation.SlideIdList = new SlideIdList();

        //        // Loop through each image file creating slides
        //        // in the new presentation.
        //        foreach (string imageFileNameWithPath in imageFileNames)
        //        {
        //            imageFileNameNoPath = Path.GetFileNameWithoutExtension(imageFileNameWithPath);

        //            // Create a unique relationship id based on the current
        //            // slide id.
        //            relId = "rel" + currentSlideId;

        //            // Get the bytes, type and size of the image.
        //            ImagePartType imagePartType = ImagePartType.Png;
        //            byte[] imageBytes = GetImageData(imageFileNameWithPath,
        //              ref imagePartType, ref imageWidthEMU, ref imageHeightEMU);

        //            // Create a slide part for the new slide.
        //            var slidePart = presentationPart.AddNewPart<SlidePart>(relId);
        //            GenerateSlidePart(imageFileNameNoPath, imageFileNameNoPath,
        //              imageWidthEMU, imageHeightEMU).Save(slidePart);

        //            // Add the relationship between the slide and the
        //            // slide layout.
        //            slidePart.AddPart<SlideLayoutPart>(slideLayoutPart);

        //            // Create an image part for the image used by the new slide.
        //            // A hardcoded relationship id is used for the image part since
        //            // there is only one image per slide. If more than one image
        //            // was being added to the slide an approach similar to that
        //            // used above for the slide part relationship id could be
        //            // followed, where the image part relationship id could be
        //            // incremented for each image part.
        //            var imagePart = slidePart.AddImagePart(ImagePartType.Jpeg, "relId1");
        //            GenerateImagePart(imagePart, imageBytes);

        //            // Add the new slide to the slide list.
        //            slideId = new SlideId();
        //            slideId.RelationshipId = relId;
        //            slideId.Id = currentSlideId;
        //            presentationPart.Presentation.SlideIdList.Append(slideId);

        //            // Increment the slide id;
        //            currentSlideId++;
        //        }

        //        // Save the changes to the slide master part.
        //        slideMasterPart.SlideMaster.Save();

        //        // Save the changes to the new deck.
        //        presentationPart.Presentation.Save(Response.OutputStream);
             
        //}

        //private List<string> GetImageFileNames(string imageFolder, string[] imageFileExtensions)
        //{
        //    List<string> fileNames = new List<string>();

        //    foreach (string extension in imageFileExtensions)
        //    {
        //        fileNames.AddRange(Directory.GetFiles(imageFolder, extension, SearchOption.TopDirectoryOnly));
        //    }

        //    return fileNames;
        //}

        //private byte[] GetImageData(string imageFilePath, ref ImagePartType imagePartType, ref long imageWidthEMU, ref long imageHeightEMU)
        //{
        //    byte[] imageFileBytes;
        //    // Bitmap imageFile;

        //    // Open a stream on the image file and read it's contents. The
        //    // following code will generate an exception if an invalid file
        //    // name is passed.
        //    using (FileStream fsImageFile = System.IO.File.OpenRead(imageFilePath))
        //    {
        //        imageFileBytes = new byte[fsImageFile.Length];
        //        fsImageFile.Read(imageFileBytes, 0, imageFileBytes.Length);

        //        using (Bitmap imageFile = new Bitmap(fsImageFile))
        //        {
        //            // Determine the format of the image file. This sample code
        //            // supports working with the following types of image files:
        //            //
        //            // Bitmap (BMP)
        //            // Graphics Interchange Format (GIF)
        //            // Joint Photographic Experts Group (JPG, JPEG)
        //            // Portable Network Graphics (PNG)
        //            // Tagged Image File Format (TIFF)

        //            if (imageFile.RawFormat.Guid == ImageFormat.Bmp.Guid)
        //                imagePartType = ImagePartType.Bmp;
        //            else if (imageFile.RawFormat.Guid == ImageFormat.Gif.Guid)
        //                imagePartType = ImagePartType.Gif;
        //            else if (imageFile.RawFormat.Guid == ImageFormat.Jpeg.Guid)
        //                imagePartType = ImagePartType.Jpeg;
        //            else if (imageFile.RawFormat.Guid == ImageFormat.Png.Guid)
        //                imagePartType = ImagePartType.Png;
        //            else if (imageFile.RawFormat.Guid == ImageFormat.Tiff.Guid)
        //                imagePartType = ImagePartType.Tiff;
        //            else
        //            {
        //                throw new ArgumentException(
        //                  "Unsupported image file format: " + imageFilePath);
        //            }

        //            // Get the dimensions of the image in English Metric Units
        //            // (EMU) for use when adding the markup for the image to the
        //            // slide.
        //            imageWidthEMU =
        //            (long)
        //            ((imageFile.Width / imageFile.HorizontalResolution) * 914400L);

        //            imageHeightEMU =
        //            (long)
        //            ((imageFile.Height / imageFile.VerticalResolution) * 914400L);
        //        }
        //    }

        //    return imageFileBytes;
        //}

        //private static Slide GenerateSlidePart(string imageName,
        //  string imageDescription, long imageWidthEMU, long imageHeightEMU)
        //{
        //    var element =
        //      new Slide(
        //        new CommonSlideData(
        //          new ShapeTree(
        //            new NonVisualGroupShapeProperties(
        //              new NonVisualDrawingProperties() { Id = (UInt32Value)1U, Name = "" },
        //              new NonVisualGroupShapeDrawingProperties(),
        //              new ApplicationNonVisualDrawingProperties()),
        //            new GroupShapeProperties(
        //              new a.TransformGroup(
        //                new a.Offset() { X = 0L, Y = 0L },
        //                new a.Extents() { Cx = 0L, Cy = 0L },
        //                new a.ChildOffset() { X = 0L, Y = 0L },
        //                new a.ChildExtents() { Cx = 0L, Cy = 0L })),
        //            new Picture(
        //              new NonVisualPictureProperties(
        //                new NonVisualDrawingProperties()
        //                {
        //                    Id = (UInt32Value)4U,
        //                    Name = imageName,
        //                    Description = imageDescription
        //                },
        //                new NonVisualPictureDrawingProperties(
        //                  new a.PictureLocks() { NoChangeAspect = true }),
        //                new ApplicationNonVisualDrawingProperties()),
        //                new BlipFill(
        //                  new a.Blip() { Embed = "relId1" },
        //                  new a.Stretch(
        //                    new a.FillRectangle())),
        //                new ShapeProperties(
        //                  new a.Transform2D(
        //                    new a.Offset() { X = 0L, Y = 0L },
        //                    new a.Extents()
        //                    {
        //                        Cx = imageWidthEMU,
        //                        Cy = imageHeightEMU
        //                    }),
        //                  new a.PresetGeometry(
        //                    new a.AdjustValueList()) { Preset = a.ShapeTypeValues.Rectangle }
        //                )))),
        //        new ColorMapOverride(
        //          new a.MasterColorMapping()));

        //    return element;
        //}

        //private void GenerateImagePart(OpenXmlPart part, byte[] imageFileBytes)
        //{
        //    // Write the contents of the image to the ImagePart.
        //    using (BinaryWriter writer = new BinaryWriter(part.GetStream()))
        //    {
        //        writer.Write(imageFileBytes);
        //        writer.Flush();
        //    }
        //}

        //private void DisplayValidationErrors(IEnumerable<ValidationErrorInfo> errors)
        //{
        //    int errorIndex = 1;

        //    foreach (ValidationErrorInfo errorInfo in errors)
        //    {
        //        Console.WriteLine(errorInfo.Description);
        //        Console.WriteLine(errorInfo.Path.XPath);

        //        if (++errorIndex <= errors.Count())
        //            Console.WriteLine("================");
        //    }
        //}

    }
}
