namespace Plugin.Maui.ChatGPT.Sample.ViewModels;

public class TextChatGptViewModel(IKeys keys)
{
    public string OpenAiApiKey { get; private set; } = keys.GetOpenAiApiKey();
}
