using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NLog;
using OcrEngine;
using SaveAsOcr.Exceptions;

namespace SaveAsOcr
{
    class MainController : IMainController
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private IPdfConverter pdfConverter;
        private IOcrReader ocrReader;

        public MainController(IPdfConverter pdfConverter, IOcrReader ocrReader)
        {
            this.pdfConverter = pdfConverter;
            this.ocrReader = ocrReader;
        }

        public SaveResult OnStartClicked(string inputDir, string outputDir, string matchRegex, string replaceRegex)
        {

            Regex matchRegexCompiled = null;

            if (!Directory.Exists(inputDir))
            {
                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Input path '{0}' doesn't exist.", inputDir));
            }
            if (!Directory.Exists(inputDir))
            {
                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Output path '{0}' doesn't exist.", outputDir));
            }

            try
            {
                matchRegexCompiled = new Regex(matchRegex, RegexOptions.Compiled);
            }
            catch
            {
                return new SaveResult(SaveResultStatus.FAILURE, String.Format("Match regex '{0}' is invalid.", matchRegex));
            }

            int count = 0;
            foreach (string path in Directory.EnumerateFiles(inputDir))
            {
                try
                {
                    SaveAsUsingRegex(path, outputDir, matchRegexCompiled, replaceRegex);
                    count++;
                }
                catch (Exception e)
                {
                    log.Error(String.Format("Error while processing file '{0}'", path), e);
                    return new SaveResult(SaveResultStatus.FAILURE, String.Format("Failed to read file '{0}'. Error: '{1}'", path, e.Message));
                }
            }

            return new SaveResult(SaveResultStatus.SUCCESS, String.Format("Saved '{0}' files.", count));
        }

        private void SaveAsUsingRegex(string path, string outputDir, Regex matchRegex, string replaceRegex)
        {
            string fileContents = GetFileContents(path);
            string targetFileName = GetTargetFileName(fileContents, matchRegex, replaceRegex);
            SaveFileToTargetDir(path, outputDir, targetFileName);
        }

        private string GetFileContents(string path)
        {
            Stream bitmapStream;
            if (Path.GetExtension(path).Equals(".pdf", StringComparison.InvariantCultureIgnoreCase))
            {
                bitmapStream = ConvertFromPdf(path);
            }
            else
            {
                bitmapStream = new FileStream(path, FileMode.Open);
            }

            return ocrReader.GetTextFromImage(bitmapStream);
        }

        private CombinedStream ConvertFromPdf(string path)
        {
            log.Info("File '{0}' is PDF, converting to bitmap", path);
            IEnumerable<Stream> rasterizedPdfs = pdfConverter.RasterizePdf(path);
            var combinedStream = new CombinedStream(rasterizedPdfs);

            return combinedStream;
        }

        private string GetTargetFileName(string fileContents, Regex matchRegex, string replaceRegex)
        {
            log.Info("File contents: \n {0} \n End of file contents", fileContents);
            Match match = matchRegex.Match(fileContents);
            if (match.Success)
            {
                string result = replaceRegex;
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    log.Info("Matched regex group: '{0}'", match.Groups[i].Value);
                    result = result.Replace(String.Format("${0}", i), match.Groups[i].Value.Trim());
                }
                log.Info("Result of regex replace: '{0}'", result);

                return result;
            }

            throw new RegexException(String.Format("Failed to match regex '{0}'", matchRegex));
        }

        private void SaveFileToTargetDir(string path, string outputDir, string targetFileName)
        {
            string targetPath = Path.Combine(outputDir, targetFileName + Path.GetExtension(path));
            log.Info("Saving new file. Original path: '{0}'. New path: '{1}'", path, targetPath);
            File.Copy(path, targetPath);
        }
    }
}
