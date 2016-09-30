using System;
using System.IO;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Expressions;
using NReco.VideoConverter;
using OVC.Web.Infrastructure.Extensions;
using OVC.Web.Models.Convert;

namespace OVC.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Convert()
        {
            var availableFormats = (typeof(Format)).GetConstantsValues<string>();
            var model = new ConvertViewModel
            {
                AvailableFormatTypes = new SelectList(availableFormats)
            };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Convert(HttpPostedFileBase inputFile, string outputFileType)
        {
            if (inputFile.IsNull())
            {
                return this.View();
            }

            // Build folder path
            var basePath = Server.MapPath(@"~\App_Data");
            var currentDate = DateTime.Now;
            var currentDateFolder = $"{currentDate.Year}-{currentDate.ToString("MMMM")}-{currentDate.Day}";
            var directoryPath = $@"{basePath}\{currentDateFolder}";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Save the input file to the file system
            var inputFileName = inputFile.FileName;
            var inputFilePath = $@"{directoryPath}\{inputFileName}";
            inputFile.SaveAs(inputFilePath);

            // Convert the input file to the selected output file type
            var outputFileName = $"{Path.GetFileNameWithoutExtension(inputFileName)}.{outputFileType}";
            var outputFilePath = $@"{directoryPath}\{outputFileName}";
            var ffMpeg = new FFMpegConverter();
            ffMpeg.ConvertMedia(inputFilePath, outputFilePath, outputFileType);

            // Download file
            var contentType = MediaTypeNames.Application.Octet;
            return this.File(outputFilePath, contentType, outputFileName);
        }

        public ActionResult Contact()
        {
            return this.View();
        }
    }
}