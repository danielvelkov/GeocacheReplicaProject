using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models.WrappedModels
{
    public class UserRanking
    {
        public int Rank { get; set; }
        public int Points { get; set; }
        public string UserName { get; set; }
        public int FoundTreasures { get; set; }
        public int HiddenTreasures { get; set; }
        public int Joined { get; set; }
    }
}
