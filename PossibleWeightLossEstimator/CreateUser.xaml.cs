
using Microsoft.Maui.Platform;

namespace PossibleWeightLossEstimator;

public partial class CreateUser : ContentPage
{
    private string _previousView;
    public CreateUser(string previousView)
	{
        InitializeComponent();
        _previousView = previousView;
    }
    private async void OnCreateUserClicked(object sender, EventArgs e)
    {
        var user = await App.DatabaseService.GetSingleUserAsync();
        if (user == null)
        {
            if (double.TryParse(BodyWeightEntry.Text, out double bodyWeight))
            {
                user = new User { BodyWeight = bodyWeight };
                await App.DatabaseService.SaveUserAsync(user);
                PrintMessage("User created");

                if (_previousView == "MainPage")
                {
                    await Navigation.PopAsync();
                }
                else if (_previousView == "Delete")
                {
                    await Navigation.PopToRootAsync();

                }
            }
            else
            {
                PrintMessage("Enter valid body weight.");
            }
            BodyWeightEntry.Text = string.Empty;
        }
        else
        {
            PrintMessage("Only one user can be created.");
        }
    }
    private async void PrintMessage(string message)
    {
        DisplayMessage.Text = message;
        DisplayMessage.IsVisible = true;
        await Task.Delay(2000);
        DisplayMessage.IsVisible = false;
    }
}