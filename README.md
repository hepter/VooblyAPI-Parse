# Usage

```
VooblyAPI vapi = VooblyAPI.getInstance();
/*
is only necessary if the user needs to obtain a profile picture.
Other usage:
vapi.createVooblySession("VOOBLY_API_KEY"); 
*/
//vapi.createVooblySession("VOOBLY_API_KEY", "VooblUsername", "passwd"); 

//Getting Lobbies -
Lobby[] lobbies = vapi.getLobbies();
//Finding required lobby and getting third Ladder(RM 1v1) via LinQ
int rmLobbyid = lobbies.Where(a => a.Name == "RM/DM - Medieval Siege").Single().Ladders[2];

//Searcing UserID by Name and getting ladder information.
int rmRate = vapi.getLadder(rmLobbyid, vapi.findUsers("hepter")[0].Uid)[0].Rating;


```
