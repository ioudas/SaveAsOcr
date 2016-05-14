namespace SaveAsOcr
{
    public interface IMainController
    {
        SaveResult OnStartClicked(string inputDir, string outputDir, string matchRegex, string replaceRegex);
    }
}