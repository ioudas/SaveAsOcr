using System.Drawing;
using System.IO;
using NLog;
using Tesseract;

namespace OcrEngine
{
    public class OcrReader : IOcrReader
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public string GetTextFromImage(Stream inputStream)
        {
            log.Info("Initializing Tesseract engine.");
            using (var engine = new TesseractEngine("tessdata", "eng", EngineMode.Default))
            {
                log.Info("Processing image stream.");
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