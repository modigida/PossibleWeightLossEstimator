using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace PossibleWeightLossEstimator
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                Window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;

                Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#F5F5F5"));
            }
        }
    }
}
