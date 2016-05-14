namespace SaveAsOcr
{
    public interface IMainController
    {
        void OnStartClicked(string inputDir, string outputDir, string matchRegex, string replaceRegex);
    }
}