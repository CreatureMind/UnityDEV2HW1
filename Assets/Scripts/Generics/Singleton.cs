using UnityEngine;

[DefaultExecutionOrder(-100)]
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static bool instanceCallCreation = false;
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                if (_quitting) return null;
                instanceCallCreation = true;
                var createdSingleton = new GameObject().AddComponent<T>();
                instanceCallCreation = false;
                if (!createdSingleton.DontDestoryOnLoad)
                {
                    Destroy(createdSingleton);
                    return null;
                }
                else createdSingleton.InitilizeSingleton();
            }

            return _instance;
        }
    }

    private static bool _quitting;

    protected virtual bool DontDestoryOnLoad => true;
    protected virtual string SingletonObjName => typeof(T).Name;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        if (instanceCallCreation && !DontDestoryOnLoad) return;
        
        InitilizeSingleton();
    }

    private void InitilizeSingleton()
    {
        _instance ??= this as T;
        gameObject.name = SingletonObjName;

        OnSingletonCreated();

        if (_instance.DontDestoryOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    protected virtual void OnSingletonCreated(){}

    void OnApplicationQuit()
    {
        _quitting = true;
    }
}
