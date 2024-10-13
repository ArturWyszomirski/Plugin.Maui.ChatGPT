/*
 * In order to make sample work:
 * In Resources/Raw directory create a file named OpenAiApiKey.txt containing valid OpenAI API key and set the Build Action as MauiAsset in file properties.
 */

namespace Plugin.Maui.ChatGPT.Sample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddTransient<TextChatGptPage>()
						.AddTransient<AudioChatGptPage>()

						.AddTransient<TextChatGptViewModel>()
						.AddTransient<AudioChatGptViewModel>();

        return builder.Build();
	}
}