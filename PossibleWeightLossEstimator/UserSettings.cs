using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PossibleWeightLossEstimator
{
    public class UserSettings
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double Weeks { get; set; }
        public string CalorieDeficitLevel { get; set; }
        public double TargetWeight { get; set; }
    }
}
