using System;
using System.Collections.Generic;
using TMPro;
using UI.Base;
using UnityEngine;

public class CharacterSelectionManager : BaseMenu
{
    [SerializeField] private SelectedCharacterParent characterParent;
    private Character[] characters;
    [SerializeField] private TMP_Text nameTextOutline;
    [SerializeField] private TMP_Text nameTextFill;
    [SerializeField] private BathroomDoor door;

    private int selectedCharacter = 0;

    private bool doorClosing;

    void Start()
    {
        characters = characterParent.GetCharacters();

        selectedCharacter = Array.IndexOf(characters, characterParent.GetSelectedCharacter());

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
        nameTextOutline.text = characters[selectedCharacter].characterName;
        nameTextFill.text = characters[selectedCharacter].characterName;

        nameTextFill.color = characters[selectedCharacter].characterColor;
    }

    void DoorOpened()
    {
        doorClosing = false;
    }

    void DoorClosed()
    {
        characterParent.SelectCharacter(characters[selectedCharacter]);
        UpdateInfoText();

        doorClosing = false;

        door.OpenDoor();
    }
    
    public void OnUse()
    {
        SaveManager.saveData.selectedCharacterID = characters[selectedCharacter].characterID;

        SaveManager.WriteSaveData();
    }

    public override void ShowMenu()
    {
        // backfip :sunglasses:
    }

    public override void HideMenu()
    {
        //b to bar
    }

    public override void EscapePressed()
    {
        //b to bar
    }
}
