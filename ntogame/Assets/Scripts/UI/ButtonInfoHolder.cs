using UnityEngine;
using UnityEngine.UI;

public class ButtonInfoHolder : MonoBehaviour
{
    [SerializeField] private GameObject KeyNameHolderPrefab;

    [SerializeField] private KeyMapper KeyMapper;
    [SerializeField] private RectTransform Content;
    [SerializeField] private Text Describtion;

    private KeyNameHolder[] KeyNameHolders = new KeyNameHolder[0];
    
    public string[] _Keys
    {
        get
        {
            string[] keys = new string[KeyNameHolders.Length];

            for(int i = 0; i < KeyNameHolders.Length; i++)
            {
                keys[i] = KeyNameHolders[i]._Key;
            }

            return keys;
        }
    }

    public void SetKeys(KeyCode[] keys)
    {
        foreach(KeyCode key in keys)
        {
            KeyNameHolder newHolder = Instantiate(KeyNameHolderPrefab, Content).GetComponent<KeyNameHolder>();
            KeyNameHolders = StaticTools.ExpandMassive(KeyNameHolders, newHolder);
            newHolder.SetInfo(this, key.ToString());
        }
    }

    public void CreateKey()
    {
        KeyNameHolder newHolder = Instantiate(KeyNameHolderPrefab, Content).GetComponent<KeyNameHolder>();
        KeyNameHolders = StaticTools.ExpandMassive(KeyNameHolders, newHolder);

        newHolder.SetInfo(this, "");
        UpdatePositions();

        newHolder.ChangeKey();
    }

    public void Delete()
    {
        if(KeyNameHolders.Length < 1)
        {
            return;
        }

        Destroy(KeyNameHolders[KeyNameHolders.Length - 1].gameObject);
        KeyNameHolders = StaticTools.ReduceMassive(KeyNameHolders, KeyNameHolders.Length - 1);
    }

    public void UpdatePositions()
    {
        float x = 0;
        
        foreach(KeyNameHolder holder in KeyNameHolders)
        {
            RectTransform transform = holder.GetComponent<RectTransform>();

            x += transform.sizeDelta.x / 2;

            transform.anchoredPosition = new Vector2(x, 0);

            x += transform.sizeDelta.x / 2+10;
        }

        Content.sizeDelta = new Vector2(x, 0);
    }
}