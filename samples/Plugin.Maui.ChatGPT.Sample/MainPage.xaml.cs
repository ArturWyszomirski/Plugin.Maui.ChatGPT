using Plugin.Maui.ChatGPT;

namespace Plugin.Maui.ChatGPT.Sample;

public partial class MainPage : ContentPage
{
	readonly IFeature feature;

	public MainPage(IFeature feature)
	{
		InitializeComponent();
		
		this.feature = feature;
	}
}
