using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace VooblyAPI_Parse
{
    public class User
    {
        private const string defImgLarge = @"/res/user_img_small.png";
        private const string defImgSmall = @"/res/user_img_large.png";

        private string rawString;
        private int uid;
        private string display_name;
        private string name;
        private DateTime account_created;
        private DateTime last_login;
        private int sex;
        private string nationid;
        private DateTime bday;
        private int level;
        private string nation;
        private string imagelarge;
        private string imagesmall;
        private int teamID;

        public string RawString { get => rawString; set => rawString = value; }
        public int Uid { get => uid; set => uid = value; }
        public string Display_name { get => display_name; set => display_name = value; }
        public string Name { get => name; set => name = value; }
        public DateTime Account_created { get => account_created; set => account_created = value; }
        public DateTime Last_login { get => last_login; set => last_login = value; }
        public int Sex { get => sex; set => sex = value; }
        public string Nationid { get => nationid; set => nationid = value; }
        public DateTime Bday { get => bday; set => bday = value; }
        public int Level { get => level; set => level = value; }
        public string Nation { get => nation; set => nation = value; }
        public string Imagelarge { get => VooblyAPI.vooblyUrl + imagelarge; set => imagelarge = value; }
        public string Imagesmall { get => VooblyAPI.vooblyUrl + imagesmall; set => imagesmall = value; }
        public int TeamID { get => teamID; set => teamID = value; }

        public int[] GetRatingsFromLadderID(int ladderid= 131)
        {
            return VooblyAPI.getInstance().getLadder(ladderid, Uid).Select(a => a.Rating).ToArray();
        }

        public User(string rawString)
        {
            this.rawString = rawString;
            int index = 0;
            string[] parts = rawString.Split(new char[] { ',' });

            this.uid = int.Parse(parts[index++]);
            this.display_name =parts[index++];
            this.name = parts[index++];
            this.account_created = StampToDate(parts[index++]);
            this.last_login = StampToDate(parts[index++]);
            this.sex = int.Parse(parts[index++]);
            this.nationid = parts[index++];

            int yyyy, mm, dd;
            dd = int.Parse(parts[index++]);
            mm = int.Parse(parts[index++]);
            yyyy = int.Parse(parts[index++]);
            this.bday = new DateTime(yyyy,mm,dd);
            this.level = int.Parse(parts[index++]);
            this.nation = parts[index++];
            this.imagesmall =  parts[index++].Replace(VooblyAPI.vooblyUrl,"").Replace(VooblyAPI.vooblyUrlSecure, "");
            this.imagelarge =  parts[index++].Replace(VooblyAPI.vooblyUrl, "").Replace(VooblyAPI.vooblyUrlSecure, "");
            if (!VooblyAPI.getInstance().isSessionValid)
            {                
                this.imagesmall = defImgSmall;
                this.imagelarge = defImgLarge;
            }

            if (!int.TryParse(parts[index++],out this.teamID))            
                this.teamID = 0;
        }

  

        public static DateTime StampToDate(double unixTimeStamp)
        {           
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static DateTime StampToDate(string unixTimeStamp)
        {          
            return StampToDate(double.Parse( unixTimeStamp));
        }

        public override string ToString()
        {
            return rawString;
        }
    }
}
