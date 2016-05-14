using System.Drawing;
using System.IO;
using Tesseract;

namespace OcrEngine
{
    public class OcrReader : IOcrReader
    {
        public string GetTextFromImage(Stream inputStream)
        {
            using (var engine = new TesseractEngine("tessdata", "eng", EngineMode.Default))
            {
                // have to load Pix via a bitmap since Pix doesn't support loading a stream.
                using (var image = new Bitmap(inputStream))
                {
                    using (var pix = PixConverter.ToPix(image))
                    {
                        using (var page = engine.Process(pix))
                        {
                            //var meanConfidence = string.Format("{0:P}", page.GetMeanConfidence());
                            return page.GetText();
                        }
                    }
                }
            }
        }
    }
}