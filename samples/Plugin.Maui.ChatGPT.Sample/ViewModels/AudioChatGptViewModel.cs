namespace Plugin.Maui.ChatGPT.Sample.ViewModels;

public class AudioChatGptViewModel(IKeys keys)
{
    public string OpenAiApiKey { get; private set; } = keys.GetOpenAiApiKey();
}
