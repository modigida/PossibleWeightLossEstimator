using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PossibleWeightLossEstimator
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double BodyWeight { get; set; }
    }
}
