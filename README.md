![nuget.png](https://raw.githubusercontent.com/ArturWyszomirski/Plugin.Maui.ChatGPT/main/nuget.png)
# Plugin.Maui.ChatGPT

`Plugin.Maui.ChatGPT` provides text and voice communication with OpenAI's ChatGPT.

## Install Plugin

[![NuGet](https://img.shields.io/nuget/v/Plugin.Maui.ChatGPT.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.Maui.ChatGPT/)

Available on [NuGet](http://www.nuget.org/packages/Plugin.Maui.ChatGPT).

Install with the dotnet CLI: `dotnet add package Plugin.Maui.ChatGPT`, or through the NuGet Package Manager in Visual Studio.

### Supported Platforms

| Platform | Minimum Version Supported |
|----------|---------------------------|
| iOS      | 11+                       |
| macOS    | 10.15+                    |
| Android  | 5.0 (API 21)              |
| Windows  | 11 and 10 version 1809+   |

## API Usage

`Plugin.Maui.ChatGPT` is based on `Plugin.Maui.Chat` and derives most of its properties and functionalities.
Go to `Plugin.Maui.Chat`'s [documentation](https://github.com/ArturWyszomirski/Plugin.Maui.Chat) for more information about customization options.
`Plugin.Maui.ChatGPT` utilizes OpenAI services for speech-to-text and text-to-speech conversion.

### Permissions

Before you can start using `Plugin.Maui.ChatGPT`, you will need to request the permissions for recording audio on each platform.

### Dependency Injection

This NuGet depends on `MAUI Community Toolkit`, so you will first need to chain up the `MAUI Community Toolkit` in app builder.

```csharp
builder.UseMauiCommunityToolkit();
```

### XAML setup

To use `Chat` you need to register `Plugin.Maui.ChatGPT.Controls` namespace by adding below line to XAML file opening tag and provide an OpenAI's API key in `OpenAiApiKey` property.

> [!WARNING]
> Make sure you are adding `Plugin.Maui.ChatGPT.Controls` namespace, not the `Plugin.Maui.ChatGPT`.

```xaml
<ContentPage ...
             xmlns:chat="clr-namespace:Plugin.Maui.ChatGPT.Controls;assembly=Plugin.Maui.ChatGPT"
             ...>
    <chat:ChatGpt OpenAiApiKey="<your_api_key>"/>
```

And that's it. You can now communicate with ChatGPT.

### Text messaging

Text messaging is enabled straight away. All you have to do is steps described above.

### Voice messaging

Voice message related functionalites are utilizing OpenAI's `whisper-1` for speech-to-text and `tts-1` for text-to-speech.

#### Speech-to-text 

To enable transcription of voice messages:
1. Enable speech-to-text: `IsSpeechToTextEnabled="True"`.
2. Make audio recorder icon visible: `IsAudioRecorderVisible="True"`.

```xaml
<ContentPage ...
             xmlns:chat="clr-namespace:Plugin.Maui.ChatGPT.Controls;assembly=Plugin.Maui.ChatGPT"
             ...>
    <chat:ChatGpt OpenAiApiKey="<your_api_key>"
                  IsSpeechToTextEnabled="True"
                  IsAudioRecorderVisible="True"/>
```

This will result in showing up the audio recorder button. 
Tapping it starts audio recording that will stop when user stops speaking or press audio recorder button one more time. 
After recording is done, it will transcribed to text.

> [!WARNING]
> Silence detection works only on Android and Windows.

#### Text-to-speech

To enable text reading all that have to be done is setting `IsTextReaderVisible="True"`.

```xaml
<ContentPage ...
             xmlns:chat="clr-namespace:Plugin.Maui.ChatGPT.Controls;assembly=Plugin.Maui.ChatGPT"
             ...>
    <chat:ChatGpt OpenAiApiKey="<your_api_key>"
                  IsTextReaderVisible="True"/>
```

This will result in showing up a text reader icon in right-top of received message. Tap it and text will be read. Tap again to stop.

#### Hands-free mode

Hands-free mode provides automatization of transcription and reading. To make it work:
1. Enable speech-to-text: `IsSpeechToTextEnabled="True"`.
2. Make hands-free mode icon visible: `IsHandsFreeModeVisible="True"`.

```xaml
<ContentPage ...
             xmlns:chat="clr-namespace:Plugin.Maui.ChatGPT.Controls;assembly=Plugin.Maui.ChatGPT"
             ...>
    <chat:ChatGpt OpenAiApiKey="<your_api_key>"
                  IsSpeechToTextEnabled="True"
                  IsHandsFreeModeVisible="True"/>
```

This will result in showing up the hands-free mode icon. 
When pressed, audio recording will start, the the audio will be transcribed and send to ChatGPT. 
When response is received it will be read out loud and, after that, the loop goes back to recording user's message.
The process continues until silence is detected or user manually turn off hands-free mode.

> [!WARNING]
> Since silence detection works only on Android and Windows, hands-free mode is also available on those platforms.

Of course you can build combinations of above features. The ultimate XAML enabling all described above functionalities would look like this:

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:chatGpt="clr-namespace:Plugin.Maui.ChatGPT.Controls;assembly=Plugin.Maui.ChatGPT"
             xmlns:viewModel="clr-namespace:Plugin.Maui.ChatGPT.Sample.ViewModels"
             x:Class="Plugin.Maui.ChatGPT.Sample.Pages.AudioChatGptPage"
             Title="AudioChatGptPage"
             x:DataType="viewModel:AudioChatGptViewModel">

    <chatGpt:ChatGpt OpenAiApiKey="{Binding OpenAiApiKey}" 
                     IsSpeechToTextEnabled="True"
                     IsAudioRecorderVisible="True"
                     IsHandsFreeModeVisible="True"
                     IsTextReaderVisible="True"
                     Margin="10"/>

</ContentPage>
```

### UI customization

Many properties of UI elements can be set as you wish.
For more comprehensive description go to `Plugin.Maui.Chat`[documentation](https://github.com/ArturWyszomirski/Plugin.Maui.Chat).

Below you'll find list of all UI related properties:

#### Sent message

- `SentMessageBackgroundColor` - Sent message background color.
- `IsSentMessageAuthorVisible` - Sent message author label visibility.
- `SentMessageAuthorTextColor` - Sent message author text color.
- `IsSentMessageTimestampVisible` - Sent message timestamp label visibility.
- `SentMessageTimestampTextColor` - Sent message timestamp text color.
- `SentMessageContentTextColor` - Sent message content text color.

#### Received message

##### UI properties

- `ReceivedMessageBackgroundColor` - Received message background color.
- `IsReceivedMessageAuthorVisible` - Received message author label visibility.
- `ReceivedMessageAuthorTextColor` - Received message author text color.
- `IsReceivedMessageTimestampVisible` - Received message timestamp label visibility.
- `ReceivedMessageTimestampTextColor` - Received message timestamp text color.
- `ReceivedMessageContentTextColor` - Received message text color.
- `ReceivedMessageAudioContentColor` - Received audio content button color.

##### Text reader

- `TextReaderIcon` - Text reader button icon.
- `TextReaderColor` - Text reader button color.
- `IsTextReaderVisible` - Determines whether text-to-speech is visible.

#### System message

- `SystemMessageBackgroundColor` - System message background color.
- `SystemMessageTextColor` - System message text color.

#### Status label

- `Status` - Status shown just above the user message entry field e.g. "John Doe is typing..."
- `IsStatusVisible` - Determines whether status label is visible.
- `StatusTextColor` - Status text color.

#### Message contents entry

##### Text content

- `TextContent` - Message typed by user.
- `TextContentColor` - Text content color.

#### Audio recorder

- `IsAudioRecorderVisible` - Determines whether start/stop record toggle button is visible.
- `AudioRecorderIcon` - Start/stop record toggle button icon.
- `AudioRecorderColor` - Start/stop record toggle button color.
- `IsSpeechToTextEnabled` - Determines whether speech-to-text is enabled.

##### Hands-free mode button

- `IsHandsFreeModeVisible` - Determines whether hands-free mode toggle button is visible.
- `HandsFreeModeIcon` - Hands-free mode toggle button icon.
- `HandsFreeModeColor` - Hands-free mode toggle button color.

##### Background color

- `MessageEntryBackgroundColor` - Message entry background color.

#### Send message button

> [!NOTE]
> Send message button is by default disabled if message is empty or `Plugin.Maui.ChatGPT` is recording, transcribing or hand-free mode is on.

- `IsSendMessageEnabled` - Determines whether send message button is enabled.
- `IsSendMessageVisible` - Determines whether send message button is visible.
- `SendMessageIcon` - Send message button icon.
- `SendMessageColor` - Send message button color.

### State properties

Those state properties are set by related services but can be overwritten if necessary.

- `IsRecording` - True when audio recording is on.
- `IsTranscribing` - True when speech transcription is on.
- `IsPlaying` - True when audio playing is on.
- `IsReading` - True when text is being read.
- `IsHandsFreeModeOn` - True when text is being read.

### Chat messages

List of chat message is enabled in `ChatMessage` property.

- `ChatMessages` - List of chat messages.

## Credits

All icons comes from [Flaticon](https://www.flaticon.com) Uicons series.

Icon was resized and coloured with [Online PNG Tools](https://onlinepngtools.com).