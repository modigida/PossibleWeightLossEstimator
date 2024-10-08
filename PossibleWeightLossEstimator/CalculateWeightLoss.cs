using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PossibleWeightLossEstimator
{
    public class CalculateWeightLoss
    {
        private const double MaintenanceCalories = 2500;
        private const double CaloriesPerKg = 7700;

        public double GetWeightLoss(string calorieDeficitLevel, double weeks)
        {
            double deficitPerDay = calorieDeficitLevel switch
            {
                "Low" => 250,
                "Medium" => 500,
                "High" => 750,
                _ => 0
            };

            double weeklyDeficit = deficitPerDay * 7;

            double totalDeficit = weeklyDeficit * weeks;

            double weightLossKg = totalDeficit / CaloriesPerKg;

            return weightLossKg;
        }
        public double GetWeeksForWeightLoss(string calorieDeficitLevel, double targetWeight)
        {
            double deficitPerDay = calorieDeficitLevel switch
            {
                "Low" => 250,
                "Medium" => 500,
                "High" => 750,
                _ => 0
            };

            double weeklyDeficit = deficitPerDay * 7;

            double totalDeficit = targetWeight * CaloriesPerKg;

            double weeks = totalDeficit / weeklyDeficit;

            return weeks;
        }
    }
}
