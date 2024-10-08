namespace PossibleWeightLossEstimator;

public partial class ChangeOrDeleteUser : ContentPage
{
    private User user = null;
    
    public ChangeOrDeleteUser(User user)
	{
        this.user = user;
        InitializeComponent();
        ShowUser();
    }
    private void ShowUser()
    {
        if (user != null)
        {
            BodyWeightEntry.Text = user.BodyWeight.ToString();
            UserIdLabel.Text = $"User ID: {user.Id}";
        }
    }
    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (user != null && double.TryParse(BodyWeightEntry.Text, out double newBodyWeight))
        {
            user.BodyWeight = newBodyWeight;
            await App.DatabaseService.SaveUserAsync(user); 
            PrintMessage("Body weight updated.");
            await Navigation.PopAsync();
        }
        else
        {
            PrintMessage("Enter valid body weight.");
        }
    }
    private async void OnDeleteUserClicked(object sender, EventArgs e)
    {
        bool shouldDelete = await DisplayAlert("Confirmation", "Delete user?", "Yes", "No");

        if (shouldDelete)
        {
            bool isDeleted = await App.DatabaseService.DeleteUserAsync(user.Id);
            if (isDeleted)
            {
                PrintMessage("User deleted.");
                await Navigation.PushAsync(new CreateUser("Delete"));
            }
            else
            {
                PrintMessage("User could not be deleted.");
            }
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