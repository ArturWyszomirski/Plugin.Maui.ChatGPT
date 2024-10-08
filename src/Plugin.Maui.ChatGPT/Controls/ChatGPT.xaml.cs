namespace Plugin.Maui.ChatGPT.Controls;

public partial class ChatGPT : ContentView
{
    OpenAiClientService? openAiClient;
    ChatGptService? chatGpt;

	public ChatGPT()
	{
		InitializeComponent();

        chat.ChatMessages = [];
        chat.SendMessageCommand = new Command(SendMessageAsync);
	}

    /// <summary>
    /// Open AI API key.
    /// </summary>
    public static readonly BindableProperty OpenAiApiKeyProperty =
        BindableProperty.Create(nameof(OpenAiApiKey), typeof(string), typeof(ChatGPT), propertyChanged: OnOpenAiApiKeyChanged);

    static void OnOpenAiApiKeyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as ChatGPT;
        string? apiKey = newValue as string;

        if (string.IsNullOrEmpty(apiKey))
            throw new ArgumentException("Open AI API key must be provided.", nameof(newValue));

        control?.StartGptService(apiKey);
    }

    void StartGptService(string apiKey)
    {
        openAiClient = new(apiKey);
        chatGpt = new(openAiClient);
    }

    public string? OpenAiApiKey
    {
        get => (string)GetValue(OpenAiApiKeyProperty);
        set => SetValue(OpenAiApiKeyProperty, value);
    }

    // TODO: add canExecute on services not null
    private async void SendMessageAsync()
    {
        chat.ChatMessages.Add(new() { TextContent = chat.TextContent, Type = Chat.MessageType.Sent });
        chatGpt.AddRequestAssistantMessage(chat.TextContent);
        chat.TextContent = string.Empty;
        chat.Status = "Awaiting response...";

        string response = await chatGpt.SendRequestAsync();

        chat.Status = string.Empty;
        chatGpt.AddRequestAssistantMessage(response);
        chat.ChatMessages.Add(new() { TextContent = response, Type = Chat.MessageType.Received });
    }
}