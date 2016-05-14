using System.IO;

namespace OcrEngine
{
    public interface IOcrReader
    {
        string GetTextFromImage(Stream inputStream);
    }
}