using SQLite;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PossibleWeightLossEstimator.DatabasManager
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;
        private string dbPath; 
        public DatabaseService(string dbPath)
        {
            this.dbPath = dbPath;
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<UserSettings>().Wait();
        }

        public Task<int> SaveUserAsync(User user)
        {
            if (user.Id != 0)
            {
                return _database.UpdateAsync(user);
            }
            else
            {
                return _database.InsertAsync(user);
            }
        }
        public async Task<User> GetSingleUserAsync()
        {
            try
            {
                return await _database.Table<User>().FirstOrDefaultAsync();
            }
            catch (FormatException ex)
            {
                Debug.WriteLine($"FormatException: {ex.Message}");
                return null;
            }
        }
        public async Task<User> GetUserAsync(int id)
        {
            try
            {
                return await _database.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
            }
            catch (FormatException ex)
            {
                Debug.WriteLine($"FormatException: {ex.Message}");
                return null;
            }
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                var user = await GetUserAsync(userId);
                if (user != null)
                {
                    await _database.DeleteAsync(user);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<bool> SaveUserSettings(int userId, double weeks, string calorieDeficitLevel, double targetWeight)
        {
            try
            {
                var db = new SQLiteAsyncConnection(dbPath);

                await db.CreateTableAsync<UserSettings>();

                var settings = new UserSettings
                {
                    UserId = userId,
                    Weeks = weeks,
                    CalorieDeficitLevel = calorieDeficitLevel,
                    TargetWeight = targetWeight
                };

                await db.InsertOrReplaceAsync(settings);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public async Task<UserSettings> LoadUserSettings(int userId)
        {
            var db = new SQLiteAsyncConnection(dbPath);

            var settings = await db.Table<UserSettings>()
                                    .Where(s => s.UserId == userId)
                                    .FirstOrDefaultAsync();

            return settings;
        }
    }
}
