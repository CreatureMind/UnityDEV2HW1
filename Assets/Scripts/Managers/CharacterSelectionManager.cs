using System;
using System.Collections.Generic;
using TMPro;
using UI.Base;
using UnityEngine;

public class CharacterSelectionManager : BaseMenu
{
    [SerializeField] private AnimationController controller;
    private Character[] characters;
    [SerializeField] private TMP_Text nameTextOutline;
    [SerializeField] private TMP_Text nameTextFill;
    [SerializeField] private BathroomDoor door;

    private int selectedCharacter = 0;

    private bool doorClosing;

    void Start()
    {
        characters = characterParent.GetCharacters();
        SoundManager.instance.StopAllSounds();
        SoundManager.instance.PlayVFX("ToiletAmbiance");
        SoundManager.instance.PlayVFX("ToiletMusic");
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
        SoundManager.instance.PlayVFX("DoorChangeCharacter");

        characterParent.SelectCharacter(characters[selectedCharacter]);
        UpdateInfoText();

        doorClosing = false;

        door.OpenDoor();
    }
    
    public void OnUse()
    {
        SoundManager.instance.PlayVFX("Punch");
        SaveManager.saveData.selectedCharacterID = characters[selectedCharacter].characterID;

        SaveManager.WriteSaveData();
    }
}
