using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Ghostscript.NET.Rasterizer;
using SaveAsOcr;

namespace OcrEngine
{
    public class PdfConverter : IPdfConverter
    {
        public IEnumerable<Stream> RasterizePdf(string pathToPdf)
        {
            var xDpi = 96;
            var yDpi = 96;

            using (var rasterizer = new GhostscriptRasterizer())
            {
                var buffer = File.ReadAllBytes(pathToPdf);
                var ms = new MemoryStream(buffer);

                rasterizer.Open(ms);

                for (var pageNumber = 1; pageNumber <= rasterizer.PageCount; pageNumber++)
                {
                    yield return rasterizer.GetPage(xDpi, yDpi, pageNumber).ToStream(ImageFormat.Bmp);
                }
            }
        }
    }
}