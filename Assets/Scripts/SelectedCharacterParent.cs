using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct CharacterWithObjects
{
    public Character characterOptions;
    public GameObject characterObj;

    public override bool Equals(object obj)
    {
        if (obj is not CharacterWithObjects character) return false;

        return characterOptions == character.characterOptions;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

public class SelectedCharacterParent : MonoBehaviour
{
    [SerializeField] private CharacterWithObjects[] characters;
    [SerializeField] private Character selectedCharacter;
    public GameObject SelectedCharacterObj {
        get
        {
            foreach (var character in characters)
            {
                if (character.characterOptions != selectedCharacter) continue;
                
                return character.characterObj;
            }

            return null;
        }
    }

    public event UnityAction<SelectedCharacterParent> OnCharacterChanged;

    void Awake()
    {
        SaveManager.LoadSaveData();

        foreach (var character in characters)
        {
            if (character.characterOptions.characterID != SaveManager.saveData.selectedCharacterID)
                continue;

            selectedCharacter = character.characterOptions;
            break;
        }

        OnCharacterChanged?.Invoke(this);

        DisableInvalidCharacters();
    }

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
        OnCharacterChanged?.Invoke(this);
        DisableInvalidCharacters();
    }

    void DisableInvalidCharacters()
    {
        foreach (var character in characters)
        {
            character.characterObj.SetActive(character.characterOptions == selectedCharacter);
        }
    }

    public Character[] GetCharacters() => characters.Select(x => x.characterOptions).ToArray();

    public Character GetSelectedCharacter() => selectedCharacter;
}
