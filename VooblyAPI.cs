using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace VooblyAPI_Parse
{

    public class VooblyAPI
    {
        public const string vooblyUrl = @"http://www.voobly.com";
        public static VooblyAPI VAPI;

        private const string LoginURLFormat = "https://www.voobly.com/login/auth?username={0}&password={1}";
        private const string uidFormat = @"&uid={0}";
        private const string uidlistFormat = @"&uidlist={0}";
        private const string startFormat = @"&start={0}";
        private const string limitFormat = @"&limit={0}";
        private const string keyFormat = @"&key={0}";

        private const string baseUrl = @"http://www.voobly.com/api/";        
        private const string validateUrl = @"validate";
        private const string userUrl = @"user/";
        private const string ladderUrl = @"ladder/";
        private const string finduserUrl = @"finduser/";
        private const string findusersUrl = @"findusers/";
        private const string lobbiesUrl = @"lobbies/";

        private const char NL = '\n';


        private const int AOE2GameID = 13;//AoE2Conquerors GameID
        private string apikey = "";
        private bool currentApiKeyisValid;
        private bool sessionIsValid;






        internal string getKeyURL => string.Format(keyFormat, apikey);

        public string Apikey
        {
            get
            {
                if (apikey.Length != 32 || !currentApiKeyisValid)                
                    throw new Exception("Invalid API Key");
                
                return apikey;
            }

            set
            {
                apikey = value;                
                currentApiKeyisValid = isKeyValid();
            }
        }

        public bool isSessionValid { get => sessionIsValid; set => sessionIsValid = value; }

        private VooblyAPI(){}

        public VooblyAPI(string apikey) => this.apikey = apikey;

        private bool isKeyValid() => (request(baseUrl + validateUrl + getKeyURL) == "valid-key") ? true : false;

        public static VooblyAPI getInstance()=> (VAPI == null)? new VooblyAPI() : VAPI;
        

        public bool createVooblySession(string username, string password)
        {
            request(vooblyUrl);
            string result = request(string.Format(LoginURLFormat, username, password), "POST");
            sessionIsValid = result.Contains("logout");
            return sessionIsValid;
        }


        public bool createVooblySession(string apiKey,string username,string password)
        {
            Apikey = apiKey;
            request(vooblyUrl);            
            string result = request(string.Format(LoginURLFormat, username, password), "POST");
            sessionIsValid = result.Contains("logout");
            return sessionIsValid;
        }





        public User getUser(int userid)
        {
            string requestStr = baseUrl + userUrl + userid + getKeyURL;
            return new User(request(requestStr).Split(NL)[1]);
        }



        public Ladder[] getLadder(int ladderid)
        {
            string requestStr = baseUrl + ladderUrl + ladderid + getKeyURL;
             return request(requestStr).Split(NL).Skip(1).Where(a=>a!="").Select(a => new Ladder(a)).ToArray();
        }
        public Ladder[] getLadder(int ladderid, int startIndex, int maxResult = 5)
        {
            string requestStr = baseUrl + ladderUrl + ladderid + getKeyURL + string.Format(startFormat, startIndex) + string.Format(limitFormat, maxResult);
             return request(requestStr).Split(NL).Skip(1).Where(a=>a!="").Select(a => new Ladder(a)).ToArray();
        }
        public Ladder[] getLadder(int ladderid, int userid)
        {
            string requestStr = baseUrl + ladderUrl + ladderid + getKeyURL + string.Format(uidFormat, userid);
            return request(requestStr).Split(NL).Skip(1).Where(a=>a!="").Select(a => new Ladder(a)).ToArray();
        }
        public Ladder[] getLadder(int ladderid, int[] useridlist)
        {
            string requestStr = baseUrl + ladderUrl + ladderid + getKeyURL + string.Format(uidlistFormat, string.Join(",", useridlist));
             return request(requestStr).Split(NL).Skip(1).Where(a=>a!="").Select(a => new Ladder(a)).ToArray();
        }
        public Ladder[] getLadder(int ladderid, int userid, int startIndex, int maxResult = 5)
        {
            string requestStr = baseUrl + ladderUrl + ladderid + getKeyURL + string.Format(uidFormat, userid) + string.Format(startFormat, startIndex) + string.Format(limitFormat, maxResult);
             return request(requestStr).Split(NL).Skip(1).Where(a=>a!="").Select(a => new Ladder(a)).ToArray();
        }
        public Ladder[] getLadder(int ladderid, int[] useridlist, int startIndex, int maxResult = 5)
        {
            string requestStr = baseUrl + ladderUrl + ladderid + getKeyURL + string.Format(uidlistFormat, string.Join(",", useridlist)) + string.Format(startFormat, startIndex) + string.Format(limitFormat, maxResult);
             return request(requestStr).Split(NL).Skip(1).Where(a=>a!="").Select(a => new Ladder(a)).ToArray();
        }





        public User[] findUsers(string usersname)
        {
            List<User> users = new List<User>();      

            string requestStr = baseUrl + findusersUrl + usersname + getKeyURL;
            string responseStr = request(requestStr);

            foreach (string str in responseStr.Split(NL).Skip(1))
            {
                if (str == ""|| str.Split(new char[] { ',' })[1]=="") continue;
                users.Add(getUser(int.Parse(str.Split(new char[] { ',' })[1])));
            }
            return users.ToArray();
        }

        public User[] findUsers(string[] usersnames)
        {
            List<User> users=new List<User>();
            string usersStr = string.Join(",", usersnames);

            string requestStr = baseUrl + findusersUrl + usersStr + getKeyURL;
            string responseStr= request(requestStr);

            foreach (string str in responseStr.Split(NL).Skip(1))
            {
                string uid = str.Split(new char[] { ',' })[1];
                if (uid == "") continue;
                users.Add(getUser(int.Parse(uid)));
            }
            return users.ToArray();
        }

        public Lobby[] getLobbies(int gameid=AOE2GameID)
        {
            List<Lobby> Lobbies = new List<Lobby>();           

            string requestStr = baseUrl + lobbiesUrl + gameid + getKeyURL;
            string responseStr = request(requestStr);

            foreach (string str in responseStr.Split(NL).Skip(1))
            {
                if (str == "") continue;
                Lobbies.Add(new Lobby(str));
            }
               

            return Lobbies.ToArray();
        }

        private CookieContainer cookieContainer = new CookieContainer();
        private string request(string URL,string method="GET")
        {            
            HttpWebRequest request;
            byte[] data=null;
            if (method== "GET")
            {
                request = (HttpWebRequest)WebRequest.Create(URL);                
            }
            else
            {
                string url, dataStr;
                url = URL.Split(new char[] { '?' })[0];
                dataStr = URL.Split(new char[] { '?' })[1];
                request = (HttpWebRequest)WebRequest.Create(url);                
                data = Encoding.UTF8.GetBytes(dataStr);
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                request.ContentLength = data.Length;

            }
          
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = method;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; rv:36.0) Gecko/20100101 Firefox/36.0";
            if (method.Equals("POST",StringComparison.OrdinalIgnoreCase))
            {
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            WebResponse response = (HttpWebResponse)request.GetResponse();
            string resultStr = new StreamReader(response.GetResponseStream(),Encoding.UTF8).ReadToEnd();
            if (resultStr== "too-busy")
            {
                throw new Exception("Server too busy");
            }
            return resultStr;
        }

    }
}
