using UnityEngine;

[CreateAssetMenu(fileName = "Songs", menuName = "DDR/Songs")]
public class SongWrapperSO : ScriptableObject
{
    public SongSO[] songs;
}
