using Plugin.Maui.Chat.Services;

namespace Plugin.Maui.ChatGPT.Services;

public partial class TextToSpeechService : ObservableRecipient, ITextToSpeechService
{
    readonly AudioManager audioManager = new();
    AsyncAudioPlayer? audioPlayer;
    readonly OpenAiToolsLibrary.Services.TextToSpeechService openAiTextToSpeech;

    public TextToSpeechService(OpenAiToolsLibrary.Services.OpenAiClientService openAiClient)
    {
        ArgumentNullException.ThrowIfNull(openAiClient);

        openAiTextToSpeech = new(openAiClient);
    }

    [ObservableProperty]
    bool isReading;

    public async Task StartReadingAsync(string? text)
    {
        IsReading = true;

        using MemoryStream stream = await openAiTextToSpeech.ConvertTextToAudio(text);

        audioPlayer = audioManager.CreateAsyncPlayer(stream);
        audioPlayer.Volume = 1;

        await audioPlayer.PlayAsync(CancellationToken.None);

        IsReading = false;
    }

    public void StopReading()
    {
        audioPlayer?.Stop();

        IsReading = false;
    }
}
