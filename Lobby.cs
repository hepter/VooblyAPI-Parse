using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VooblyAPI_Parse
{
    public class Lobby
    {
        private int lobbyid;
        private string name;
        private int players_online;
        private int max_players;
        private int[] ladders;

        private string rawLobblyStr;
        public int Lobbyid { get => lobbyid; set => lobbyid = value; }
        public string Name { get => name; set => name = value; }
        public int Players_online { get => players_online; set => players_online = value; }
        public int Max_players { get => max_players; set => max_players = value; }
        public int[] Ladders { get => ladders; set => ladders = value; }


        public Lobby(string rawLobblyStr)
        {
            this.rawLobblyStr = rawLobblyStr;
            int index = 0;
          
            string[] parts = rawLobblyStr.Split(new char[] { ',' });

            this.lobbyid = int.Parse(parts[index++]);
            this.name =parts[index++];
            this.players_online = int.Parse(parts[index++]);
            this.max_players = int.Parse(parts[index++]);
            this.Ladders = parts[index].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(a=>int.Parse(a)).ToArray();

            //foreach (string ladderTempStr in parts[index].Split(new char[] { '|' },StringSplitOptions.RemoveEmptyEntries))
            //{
            //    if (ladderTempStr == "") continue;
            //    VooblyAPI api = VooblyAPI.getInstance();
            //    ladders.Add(api.getLadder(int.Parse(ladderTempStr)));
            //}
        }

        public override string ToString()
        {
            return rawLobblyStr; 
        }

    }
}
