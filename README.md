# Voobly API Parse
This tool is a simple tool that makes it easy for you to get the necessary information via Voobly using api key.


# Usage

Create Instance
```
VooblyAPI vapi = VooblyAPI.getInstance();
```



This only necessary if the user needs to obtain a profile picture.
```
vapi.createVooblySession("VOOBLY_API_KEY", "VooblUsername", "passwd"); 
```

Other usage
```
vapi.createVooblySession("VOOBLY_API_KEY"); 
```

Getting Lobbies 
```
Lobby[] lobbies = vapi.getLobbies();
```
Finding required lobby and getting third Ladder(RM 1v1) via LinQ

```
int rmLobbyid = lobbies.Where(a => a.Name == "RM/DM - Medieval Siege").Single().Ladders[2];
```


Searcing UserID by Name and getting ladder information.
```
int rmRate = vapi.getLadder(rmLobbyid, vapi.findUsers("hepter")[0].Uid)[0].Rating;
```



