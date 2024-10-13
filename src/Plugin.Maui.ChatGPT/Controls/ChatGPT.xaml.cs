namespace Plugin.Maui.ChatGPT.Controls;

public partial class ChatGpt : Chat.Controls.Chat
{
    #region Fields
    OpenAiToolsLibrary.Services.OpenAiClientService? openAiClient;
    OpenAiToolsLibrary.Services.ChatGptService? chatGpt;
    #endregion

    #region Constructor
    public ChatGpt()
	{
		InitializeComponent();

        SendMessageCommand = new Command(async () => await SendMessageAsync(), () => chatGpt is not null);

        PropertyChanged += ChatGpt_PropertyChanged;

        ChatMessages = [];
    }

    private void ChatGpt_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(IsSpeechToTextEnabled) && IsSpeechToTextEnabled == true && openAiClient is not null)
            StartSpeechServices();
    }
    #endregion

        #region Bindable properties
        /// <summary>
        /// Open AI API key.
        /// </summary>
    public static readonly BindableProperty OpenAiApiKeyProperty =
        BindableProperty.Create(nameof(OpenAiApiKey), typeof(string), typeof(ChatGpt), propertyChanged: OnOpenAiApiKeyChanged);

    public string? OpenAiApiKey
    {
        get => (string)GetValue(OpenAiApiKeyProperty);
        set => SetValue(OpenAiApiKeyProperty, value);
    }
    #endregion

    #region Private methods
    static async void OnOpenAiApiKeyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = bindable as ChatGpt;
        string? apiKey = newValue as string;

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new ArgumentException("Open AI API key must be provided.", nameof(newValue));

        try
        {
            control?.StartOpenAiService(apiKey);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("OpenAI client", "Unable to connect to OpenAI service.", "Ok");
            Debug.WriteLine(ex);
        }
    }

    void StartOpenAiService(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        openAiClient = new(apiKey);
        chatGpt = new(openAiClient);

        ArgumentNullException.ThrowIfNull(chatGpt);

        if (IsSpeechToTextEnabled)
            StartSpeechServices();
    }

    void StartSpeechServices()
    {
        SpeechToTextService = new SpeechToTextService(this, openAiClient);
        TextToSpeechService = new TextToSpeechService(openAiClient);

        HandsFreeModeCommand = new Command(async () => await StartOrStopHandsFreeModeAsync());
    }

    async Task StartOrStopHandsFreeModeAsync()
    {
        if (SpeechToTextService.IsTranscribing)
            return;

        IsHandsFreeModeOn = !IsHandsFreeModeOn;

        if (TextToSpeechService.IsReading)
            TextToSpeechService.StopReading();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            while (IsHandsFreeModeOn)
            {
                TextContent += await SpeechToTextService.StartTranscriptionAsync();

                if (string.IsNullOrWhiteSpace(TextContent))
                    break;

                await SendMessageAsync();
                await TextToSpeechService.StartReadingAsync(ChatMessages.Last().TextContent);
            }

            IsHandsFreeModeOn = false;
        });
    }

    private async Task SendMessageAsync()
    {
        ArgumentNullException.ThrowIfNull(chatGpt);

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