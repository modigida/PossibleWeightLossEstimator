using Microsoft.Extensions.Logging;

namespace PossibleWeightLossEstimator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
#pragma warning disable CA1416
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
#pragma warning restore CA1416
        }
    }
}
