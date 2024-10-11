namespace Plugin.Maui.ChatGPT.Sample.Pages;

public partial class AudioChatGptPage : ContentPage
{
	public AudioChatGptPage(AudioChatGptViewModel audioChatGptViewModel)
	{
		InitializeComponent();

		BindingContext = audioChatGptViewModel;
	}
}