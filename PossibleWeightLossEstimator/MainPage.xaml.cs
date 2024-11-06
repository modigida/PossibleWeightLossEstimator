
using System.Globalization;
using System.Runtime.CompilerServices;

namespace PossibleWeightLossEstimator
{
    public partial class MainPage : ContentPage
    {
        private User user = null;
        public double Weeks { get; set; }
        public double TargetWeight { get; set; }
        public string CalorieDeficitLevel { get; set; }

        private CalculateWeightLoss weightLossCalculator = new CalculateWeightLoss();
        private readonly double[] allowedKgValues = { 0, 2.5, 5, 7.5, 10, 12.5, 15, 17.5, 20, 22.5, 25, 27.5, 30 };
        private readonly double[] allowedWeekValues = { 4, 8, 12, 16, 20, 24, 28, 32, 36, 40, 44, 48, 52 };
        private bool isUpdating = false;
        public MainPage()
        {
            InitializeComponent();
            Weeks = 4;
            CalorieDeficitLevel = "Low";
        }
        private async Task GetUser()
        {
            this.user = await App.DatabaseService.GetSingleUserAsync();
            if (user == null)
            {
                await Navigation.PushAsync(new CreateUser("MainPage"));
            }
        }
        public void UpdateWeekSlider()
        {
            double weeksLeft = weightLossCalculator.GetWeeksForWeightLoss(CalorieDeficitLevel, TargetWeight);
            double updatedValue = GetNearestAllowedWeekValue(weeksLeft);
            Weeks = weeksLeft;
            weekSlider.Value = updatedValue;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                amountOfWeeksLabel.Text = $"{updatedValue} weeks";
            });
        }
        public void UpdateKgSlider()
        {
            double weightLoss = weightLossCalculator.GetWeightLoss(CalorieDeficitLevel, Weeks);
            double updatedValue = GetNearestAllowedKgValue(weightLoss);
            TargetWeight = weightLoss;
            kgSlider.Value = updatedValue;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                amountOfKgLabel.Text = $"{updatedValue} kg";
            });
        }
        private double GetNearestAllowedKgValue(double value)
        {
            double closestValue = allowedKgValues[0];
            double minDifference = Math.Abs(value - closestValue);

            foreach (var allowedValue in allowedKgValues)
            {
                double difference = Math.Abs(value - allowedValue);
                if (difference < minDifference)
                {
                    closestValue = allowedValue;
                    minDifference = difference;
                }
            }
            return closestValue;
        }
        private double GetNearestAllowedWeekValue(double value)
        {
            double closestValue = allowedWeekValues[0];
            double minDifference = Math.Abs(value - closestValue);

            foreach (var allowedValue in allowedWeekValues)
            {
                double difference = Math.Abs(value - allowedValue);
                if (difference < minDifference)
                {
                    closestValue = allowedValue;
                    minDifference = difference;
                }
            }
            return closestValue;
        }
        private void OnSliderWeekChanged(object sender, ValueChangedEventArgs e)
        {
            if (isUpdating) return;
            isUpdating = true;

            double newValue = GetNearestAllowedWeekValue(e.NewValue);
            Weeks = newValue;
            weekSlider.Value = newValue;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                amountOfWeeksLabel.Text = $"{newValue} weeks";
            });

            UpdateKgSlider();
            UpdateTargetWeight();

            isUpdating = false;
        }
        private void OnSliderKgChanged(object sender, ValueChangedEventArgs e)
        {
            if (isUpdating) return;
            isUpdating = true;

            double newValue = GetNearestAllowedKgValue(e.NewValue);
            TargetWeight = newValue;
            kgSlider.Value = newValue;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                amountOfKgLabel.Text = $"{newValue} kg";
            });

            UpdateWeekSlider();
            UpdateTargetWeight();

            isUpdating = false;
        }
        private void OnSliderDeficitChanged(object sender, ValueChangedEventArgs e)
        {
            int roundedValue = (int)Math.Round(e.NewValue);
            CalorieDeficitLevel = e.NewValue switch
            {
                1 => "Low",
                2 => "Medium",
                3 => "High",
                _ => "Unknown"
            };

            deficitSlider.Value = roundedValue;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                kcalLabel.Text = $"{CalorieDeficitLevel} calorie deficit level";
            });

            if (CalorieDeficitLevel != "Unknown")
            {
                UpdateWeekSlider();
                UpdateTargetWeight();
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this, false);

            await GetUser();

            if (this.user != null)
            {
                var settings = await App.DatabaseService.LoadUserSettings(this.user.Id);

                if (settings != null)
                {
                    Weeks = settings.Weeks;
                    CalorieDeficitLevel = settings.CalorieDeficitLevel;
                    TargetWeight = settings.TargetWeight;

                    weekSlider.Value = Weeks;
                    kgSlider.Value = TargetWeight;

                    deficitSlider.Value = settings.CalorieDeficitLevel switch
                    {
                        "Low" => 1,
                        "Medium" => 2,
                        "High" => 3,
                        _ => 1
                    };

                    UpdateKgSlider();
                    UpdateWeekSlider();
                    UpdateTargetWeight();
                }
                else
                {
                    weekSlider.Value = 4;
                    kgSlider.Value = 0;
                    deficitSlider.Value = 1;
                    targetWeightLabel.Text = $"Currect weight {user.BodyWeight}";
                }
            }
        }
        private void UpdateTargetWeight()
        {
            double estimatedWeight = user.BodyWeight - TargetWeight;
            var targetDate = DateTime.Today.AddDays(Weeks * 7);

            string daySuffix = targetDate.Day switch
            {
                1 or 21 or 31 => "st",
                2 or 22 => "nd",
                3 or 23 => "rd",
                _ => "th"
            };

            string formattedDate = $"{targetDate.ToString($"MMMM dd'{daySuffix}'")}";

            targetWeightLabel.Text = $"You will reach {estimatedWeight.ToString("f0")} kg by {formattedDate}";
        }
        private async void OnSaveSettingsClicked(object sender, EventArgs e)
        {
            if (this.user != null)
            {
                if (await App.DatabaseService.SaveUserSettings(this.user.Id, Weeks, CalorieDeficitLevel, TargetWeight))
                {
                    DisplayMessage.Text = "Settings saved";
                    DisplayMessage.IsVisible = true;
                    await Task.Delay(2000);
                    DisplayMessage.IsVisible = false;
                }
            }
        }
        private async void OnEditUserClicked(object sender, EventArgs e)
        {
            if (this.user != null)
            {
                await Navigation.PushAsync(new ChangeOrDeleteUser(this.user, this));
            }
        }
    }
}
