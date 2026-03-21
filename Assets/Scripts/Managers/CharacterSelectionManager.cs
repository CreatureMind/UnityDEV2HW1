using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    [SerializeField] private AnimationController controller;
    private Character[] characters;
    [SerializeField] private List<TMP_Text> nameTexts;
    [SerializeField] private BathroomDoor door;

    private int selectedCharacter = 0;

    private bool doorClosing;

    void Start()
    {
        characters = controller.GetCharacters();

        selectedCharacter = Array.IndexOf(characters, controller.GetSelectedCharacter());

        UpdateInfoText();
        door.OnDoorClosed += DoorClosed;
        door.OnDoorOpened += DoorOpened;
        door.OpenDoor();
    }

    public void OnArrowClicked(bool isLeft)
    {
        selectedCharacter += isLeft ? -1 : 1;

        if (selectedCharacter == -1)
            selectedCharacter = characters.Length - 1;
        else if (selectedCharacter == characters.Length)
            selectedCharacter = 0;

        if (!doorClosing)
        {
            door.CloseDoor();
            doorClosing = true;
        }
    }

    void UpdateInfoText()
    {
        nameTexts.ForEach(x => x.text = characters[selectedCharacter].characterName);
    }

    void DoorOpened()
    {
        doorClosing = false;
    }

    void DoorClosed()
    {
        controller.SelectCharacter(characters[selectedCharacter]);
        UpdateInfoText();

        doorClosing = false;

        door.OpenDoor();
    }
    
    public void OnUse()
    {
        SaveManager.saveData.selectedCharacterID = characters[selectedCharacter].characterID;

        SaveManager.WriteSaveData();
    }
}
