using Arrows;
using UnityEngine;

public class SpecificCharacterEnabler : MonoBehaviour
{
    [SerializeField] private GameObject walkingCharacter;
    [SerializeField] private GameObject dancingCharacter;

    void Awake()
    {
        ArrowsSpawner.OnTrackStarted += TrackStarted;
        InGameUI.OnMenuClosed += TrackEnded;

        TrackEnded();
    }

    void TrackStarted()
    {
        walkingCharacter.SetActive(false);
        dancingCharacter.SetActive(true);
    }

    void TrackEnded()
    {
        walkingCharacter.SetActive(true);
        dancingCharacter.SetActive(false);
    }
}
