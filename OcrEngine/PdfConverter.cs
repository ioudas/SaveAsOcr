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
            var xDpi = 96;
            var yDpi = 96;

            log.Info("Raterizing PDF '{0}'", pathToPdf);
            using (var rasterizer = new GhostscriptRasterizer())
            {
                var buffer = File.ReadAllBytes(pathToPdf);
                var ms = new MemoryStream(buffer);

                rasterizer.Open(ms);

                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    log.Info("Raterizing PDF page number: {0}", pageNumber);

                    yield return rasterizer.GetPage(xDpi, yDpi, pageNumber).ToStream(ImageFormat.Bmp);
                }
            }
        }
    }
}