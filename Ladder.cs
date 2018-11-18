using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VooblyAPI_Parse
{
    public class Ladder
    {
        private string rawLadderstring;

        private int rank;
        private int uid;
        private string display_name;
        private int rating;
        private int wins;
        private int losses;
        private int streak;

        public int Rank { get => rank; set => rank = value; }
        public int Uid { get => uid; set => uid = value; }
        public string Display_name { get => display_name; set => display_name = value; }
        public int Rating { get => rating; set => rating = value; }
        public int Wins { get => wins; set => wins = value; }
        public int Losses { get => losses; set => losses = value; }
        public int Streak { get => streak; set => streak = value; }


        public Ladder(string rawLadderstring)
        {
            this.rawLadderstring = rawLadderstring;
            int index = 0;
            string[] parts = rawLadderstring.Split(new char[] { ',' });

            this.rank = int.Parse(parts[index++]);
            this.uid = int.Parse(parts[index++]);
            this.display_name = parts[index++];
            this.rating = int.Parse(parts[index++]);
            this.wins = int.Parse(parts[index++]);
            this.losses = int.Parse(parts[index++]);
            this.streak = int.Parse(parts[index++]);
        }
        public override string ToString()
        {
            return rawLadderstring;
        }
    }
}
