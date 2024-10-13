namespace Plugin.Maui.ChatGPT.Sample.ViewModels;

public class TextChatGptViewModel
{
    public string OpenAiApiKey { get; private set; } = Secrets.GetOpenAiApiKey();
}
