using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                var createdSingleton = new GameObject().AddComponent<T>();
                if (!createdSingleton.DontDestoryOnLoad)
                {
                    Destroy(createdSingleton);
                    return null;
                }

                createdSingleton.InitilizeSingleton();
            }

            return _instance;
        }
    }

    protected virtual bool DontDestoryOnLoad => true;
    protected virtual string SingletonObjName => typeof(T).Name;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

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
}
