using ElementVisuaElementHotReload.Platforms;

namespace ElementVisuaElementHotReload;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        object value = builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }).ConfigureMauiHandlers(handlers => {
                    handlers.AddHandler(typeof(MyElementParentControl), typeof(MyElementParentControlHandler));
					handlers.AddHandler(typeof(MyVisualElementParentControl), typeof(MyVisualElementParentControlHandler));

                });

		return builder.Build();
	}
}
