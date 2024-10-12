using Plugin.Maui.Chat.Services;

namespace Plugin.Maui.ChatGPT.Services;

public partial class SpeechToTextService : ObservableRecipient, ISpeechToTextService
{
    readonly ChatGpt chatGpt;
    readonly AudioManager audioManager = new();
    readonly IAudioRecorder audioRecorder;
    readonly OpenAiToolsLibrary.Services.SpeechToTextService openAiSpeechToText;

    public SpeechToTextService(ChatGpt chatGpt, OpenAiToolsLibrary.Services.OpenAiClientService openAiClient)
    {
        ArgumentNullException.ThrowIfNull(chatGpt);
        ArgumentNullException.ThrowIfNull(openAiClient);

        openAiSpeechToText = new(openAiClient);
        audioRecorder = audioManager.CreateRecorder();
        this.chatGpt = chatGpt;
    }

    [ObservableProperty]
    bool isTranscribing;

    public async Task<string?> StartTranscriptionAsync()
    {
        IAudioSource? audioSource = new EmptyAudioSource();

        if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
        {
            await Shell.Current.DisplayAlert("Permission denied", "The app needs microphone permission to record audio.", "OK");
            return string.Empty;
        }

        IsTranscribing = true;

        if (!audioRecorder.IsRecording)
        {
            await audioRecorder.StartAsync();

            chatGpt.IsRecording = true;

            audioSource = await audioRecorder.StopAsync(When.SilenceIsDetected());
        }

        return await GetTranscriptionAsync(audioSource);   
    }
    public async Task<string?> StopTranscriptionAsync()
    {
        IAudioSource? audioSource = await audioRecorder.StopAsync(When.Immediately());

        if (audioSource is null or EmptyAudioSource) 
            return string.Empty;

        return await GetTranscriptionAsync(audioSource);
    }

    async Task<string?> GetTranscriptionAsync(IAudioSource? audioSource)
    {
        string? text = string.Empty;

        chatGpt.IsRecording = false;

        if (audioRecorder.SoundDetected)
        {
            chatGpt.Status = "Transcribing..."; // fix statuses in plugin

            using Stream? stream = audioSource?.GetAudioStream();
            text = await openAiSpeechToText.SendTranscribeRequestAsync(stream) + " ";
        }
        else
            text = string.Empty;

        IsTranscribing = false;

        return text;
    }
}
