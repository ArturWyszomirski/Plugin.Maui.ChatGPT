namespace Plugin.Maui.ChatGPT.Sample.ViewModels;

public class AudioChatGptViewModel
{
    public string OpenAiApiKey { get; private set; } = Secrets.GetOpenAiApiKey();
}
