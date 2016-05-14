using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace OcrEngine
{
    public interface IPdfConverter
    {
        IEnumerable<Stream> RasterizePdf(string pathToPdf);
    }
}