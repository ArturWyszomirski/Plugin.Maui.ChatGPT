﻿namespace Plugin.Maui.ChatGPT.Sample;

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

        builder.Services.AddSingleton<IKeys, Keys>()
						.AddTransient<TextChatGptPage>()
						.AddTransient<AudioChatGptPage>()

						.AddTransient<TextChatGptViewModel>()
						.AddTransient<AudioChatGptViewModel>();

        return builder.Build();
	}
}