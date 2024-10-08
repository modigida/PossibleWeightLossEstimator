using PossibleWeightLossEstimator.DatabasManager;

namespace PossibleWeightLossEstimator
{
    public partial class App : Application
    {
        public static DatabaseService DatabaseService { get; private set; }
        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyApp.db");

            DatabaseService = new DatabaseService(dbPath);
            MainPage = new NavigationPage(new MainPage());
        }
    }
}
