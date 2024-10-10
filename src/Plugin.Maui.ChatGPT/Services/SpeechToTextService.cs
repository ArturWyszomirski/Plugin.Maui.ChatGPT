using Plugin.Maui.Chat.Services;

namespace Plugin.Maui.ChatGPT.Services;

public partial class SpeechToTextService : ObservableRecipient, ISpeechToTextService
{
    readonly IAudioService audioService;
    IAudioSource? audioSource;
    OpenAiToolsLibrary.Services.SpeechToTextService openAiSpeechToText;

    public SpeechToTextService(IAudioService audioService, OpenAiToolsLibrary.Services.OpenAiClientService openAiClient)
    {
        ArgumentNullException.ThrowIfNull(audioService);
        ArgumentNullException.ThrowIfNull(openAiClient);

        openAiSpeechToText = new(openAiClient);
        this.audioService = audioService;
    }

    [ObservableProperty]
    bool isTranscribing;

    public async Task<string?> StartTranscriptionAsync()
    {
        string text = string.Empty;

        if (await Permissions.RequestAsync<Permissions.Microphone>() != PermissionStatus.Granted)
        {
            await Shell.Current.DisplayAlert("Permission denied", "The app needs microphone permission to record audio.", "OK");
            return text;
        }
        
        audioSource = await audioService.StartRecordingUntilSilenceDetectedAsync();

        IsTranscribing = true;

        return await GetTranscriptionAsync();
    }

    public async Task<string?> StopTranscriptionAsync()
    {
        await audioService.StopRecordingAsync();

        return await GetTranscriptionAsync();
    }

    private async Task<string?> GetTranscriptionAsync()
    {
        using Stream? stream = audioSource?.GetAudioStream();
        string? text = await openAiSpeechToText.SendTranscribeRequestAsync(stream);

        IsTranscribing = false;

        return text;
    }
}
