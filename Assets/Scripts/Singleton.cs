using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    [SerializeField] private bool persistAcrossScenes = true;

    /// <summary>
    /// Make sure this is called on override when inheriting this class.
    /// </summary>
    private protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this as T;

        if(persistAcrossScenes)
            DontDestroyOnLoad(gameObject);
    }
}
