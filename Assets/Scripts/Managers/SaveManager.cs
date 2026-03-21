using UnityEngine;
using System.IO;
using UnityEngine.Events;

public static class SaveManager
{
    private static readonly string SAVE_FILE_PATH = Path.Combine(Application.persistentDataPath, "saveData.json");

    public static SaveData saveData;

    public static event UnityAction OnSaveLoaded;
    public static event UnityAction OnSaveWritten;

    public static void LoadSaveData()
    {
        if (!File.Exists(SAVE_FILE_PATH))
        {
            saveData = new SaveData();

            OnSaveLoaded?.Invoke();
            return;
        }

        string json = File.ReadAllText(SAVE_FILE_PATH);
        saveData = JsonUtility.FromJson<SaveData>(json);

        OnSaveLoaded?.Invoke();
    }

    public static void WriteSaveData()
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SAVE_FILE_PATH, json);

        OnSaveWritten?.Invoke();
    }
}
