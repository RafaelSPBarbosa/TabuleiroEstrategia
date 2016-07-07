using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class MatchMakingData
{
    public bool result;
    public ServerData[] servers;

    public static MatchMakingData CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<MatchMakingData>(jsonString);
    }
}

[Serializable]
public class ServerData
{
    public string matchmaking_id;
    public string name;
    public int max_players;
    public int current_players;
    public bool is_full;
    public bool is_password;
    public bool never_true;
    public bool in_progress;
}
