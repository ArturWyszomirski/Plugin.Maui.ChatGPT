namespace Plugin.Maui.ChatGPT.Sample.Pages;

public partial class TextChatGptPage : ContentPage
{
	public TextChatGptPage(TextChatGptViewModel textChatGptViewModel)
	{
		InitializeComponent();

		BindingContext = textChatGptViewModel;
	}
}