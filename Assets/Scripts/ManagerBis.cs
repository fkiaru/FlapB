using UnityEngine;

public class ManagerBis : MonoBehaviour
{
    // PROTOTYPE SINGLETON
    public static ManagerBis instance { private set; get; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    [System.Serializable]
    public struct Item
    {
        [SerializeField] public int id;
        [SerializeField] public string name;
    }
    public Item[] items;
    //[CreateAssetMenu(menuName = "Scripts")]
    public class ItemData: ScriptableObject
    {
        public int statckMaxCount = 1;
    }
    public ItemData i22Data;
    
}
