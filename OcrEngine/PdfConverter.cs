using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ghostscript.NET.Rasterizer;
using NLog;
using SaveAsOcr;

namespace OcrEngine
{
    public class PdfConverter : IPdfConverter
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public IEnumerable<Stream> RasterizePdf(string pathToPdf)
        {
            var xDpi = 256;
            var yDpi = 256;

            log.Info("Raterizing PDF '{0}'", pathToPdf);
            using (var rasterizer = new GhostscriptRasterizer())
            {
                var buffer = File.ReadAllBytes(pathToPdf);
                var ms = new MemoryStream(buffer);

                rasterizer.Open(ms);

                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    log.Info("Raterizing PDF page number: {0}", pageNumber);
                    Image bitmap = rasterizer.GetPage(xDpi, yDpi, pageNumber);
//                    bitmap.Save(@"C:\git\testdata\out\bitmap.bmp", ImageFormat.Bmp);

                    yield return bitmap.ToStream(ImageFormat.Bmp);
                }
            }
        }
    }
}