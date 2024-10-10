namespace Plugin.Maui.ChatGPT.Controls;

public partial class ChatGPT : Chat.Controls.Chat
{
    #region Fields
    OpenAiToolsLibrary.Services.OpenAiClientService? openAiClient;
    OpenAiToolsLibrary.Services.ChatGptService? chatGpt;
    #endregion

    #region Constructor
    public ChatGPT()
	{
		InitializeComponent();

        ChatMessages = [];
    }
    #endregion

    #region Bindable properties
    /// <summary>
    /// Open AI API key.
    /// </summary>
    public static readonly BindableProperty OpenAiApiKeyProperty =
        BindableProperty.Create(nameof(OpenAiApiKey), typeof(string), typeof(ChatGPT), propertyChanged: OnOpenAiApiKeyChanged);

    public string? OpenAiApiKey
    {
        get => (string)GetValue(OpenAiApiKeyProperty);
        set => SetValue(OpenAiApiKeyProperty, value);
    }
    #endregion

    #region Private methods
    static void OnOpenAiApiKeyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as ChatGPT;
        string? apiKey = newValue as string;

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Open AI API key must be provided.", nameof(newValue));

        control?.StartOpenAiService(apiKey);
    }

    void StartOpenAiService(string apiKey)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(apiKey);

        openAiClient = new(apiKey);
        chatGpt = new(openAiClient);

        ArgumentNullException.ThrowIfNull(chatGpt);

        SendMessageCommand = new Command(async () => await SendMessageAsync());

        if (IsSpeechToTextEnabled)
        {
            SpeechToTextService = new SpeechToTextService(AudioService, openAiClient);
            TextToSpeechService = new TextToSpeechService(openAiClient);
        }
    }

    // TODO: add canExecute on services not null
    private async Task SendMessageAsync()
    {
        ChatMessages.Add(new()
        {
            Author = "User",
            TextContent = TextContent,
            Type = Chat.MessageType.Sent
        });

        chatGpt.AddRequestAssistantMessage(TextContent);
        TextContent = string.Empty;
        Status = "Awaiting response...";

        string response = await chatGpt.SendRequestAsync();

        Status = string.Empty;
        chatGpt.AddRequestAssistantMessage(response);

        ChatMessages.Add(new()
        {
            Author = "Assistant",
            TextContent = response,
            Type = Chat.MessageType.Received
        });
    }
    #endregion
}