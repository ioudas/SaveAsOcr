using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using OcrEngine;

namespace SaveAsOcr
{
    class MainController : IMainController
    {
        private IPdfConverter pdfConverter;
        private IOcrReader ocrReader;

        public MainController(IPdfConverter pdfConverter, IOcrReader ocrReader)
        {
            this.pdfConverter = pdfConverter;
            this.ocrReader = ocrReader;
        }

        public void OnStartClicked(string inputDir, string outputDir, string matchRegex, string replaceRegex)
        {

            Regex matchRegexCompiled = null;
            Regex replaceRegexCompiled = null;

//            if (!Directory.Exists(inputDir))
//            {
//                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Input path '{0}' doesn't exist.", inputDir));
//            }
//            if (!Directory.Exists(inputDir))
//            {
//                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Output path '{0}' doesn't exist.", outputDir));
//            }
//
//            try
//            {
//                matchRegexCompiled = new Regex(matchRegex, RegexOptions.Compiled);
//            }
//            catch
//            {
//                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Match regex '{0}' is invalid.", matchRegex));
//            }
//            try
//            {
//                replaceRegexCompiled = new Regex(replaceRegex, RegexOptions.Compiled);
//            }
//            catch
//            {
//                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Replace regex '{0}' is invalid.", replaceRegex));
//            }

            foreach (string path in Directory.EnumerateFiles(inputDir))
            {
                SaveAsUsingRegex(path, outputDir, matchRegexCompiled, replaceRegexCompiled);
            }

        }

        private SaveResult SaveAsUsingRegex(string path, string outputDir, Regex matchRegex, Regex replaceRegex)
        {
            string fileContents = GetFileContents(path);
            string targetFileName = GetTargetFileName(fileContents, matchRegex, replaceRegex);
            SaveFileToTargetDir(path, outputDir, targetFileName);

            return null;
        }

        private string GetFileContents(string path)
        {
            if (Path.GetExtension(path).Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
            {
                IEnumerable<Stream> rasterizedPdfs = pdfConverter.RasterizePdf(path);
                var combinedStream = new CombinedStream(rasterizedPdfs);
                return ocrReader.GetTextFromImage(combinedStream);
            }

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                return ocrReader.GetTextFromImage(stream);
            }
        }

        private string GetTargetFileName(string fileContents, Regex matchRegex, Regex replaceRegex)
        {
            throw new NotImplementedException();
        }

        private void SaveFileToTargetDir(string path, string outputDir, string targetFileName)
        {
            throw new NotImplementedException();
        }
    }
}
