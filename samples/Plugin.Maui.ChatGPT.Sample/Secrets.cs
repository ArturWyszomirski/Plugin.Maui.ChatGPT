namespace Plugin.Maui.ChatGPT.Sample;

internal static class Secrets
{
    internal static async Task<string> GetOpenAiApiKeyAsync()
    {
        string fileName = "OpenAiApiKey.txt";

        try
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);
            return reader.ReadToEndAsync().Result;
        }
        catch (FileNotFoundException ex)
        {
            throw new FileNotFoundException($"File containing OpenAI API key not found. " +
                                            $"In Resources/Raw directory create a file named {fileName} containing valid key and set the Build Action as MauiAsset in file properties.",
                                            nameof(fileName),
                                            ex);
        }
    }
}
