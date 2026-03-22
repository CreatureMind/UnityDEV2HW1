using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SongSaveData
{
    public Dictionary<int, int> scoreForDifficulty;
}

[System.Serializable]
public struct SaveData
{
    public Dictionary<string, SongSaveData> songsData;

    public uint selectedCharacterID;

    
}
