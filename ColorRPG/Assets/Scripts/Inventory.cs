using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public delegate void OnItemChanged();
    public OnItemChanged onIntemChangedCallback;

    public List<Item> items = new List<Item>();
    public int space = 12;

    public Item healthPotionRef;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance of Inventory found");
            return;
        }

        instance = this;
    }

    public void Update()
    {
        //For Testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            Add(healthPotionRef);
        }
    }

    public void Add(Item item)
    {
        if (items.Count >= space)
        {
            Debug.Log("Not enough room in inventory");
            return;
        }

        items.Add(item);


        if (onIntemChangedCallback != null)
        {
            onIntemChangedCallback.Invoke();
        }
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        
        //Call any events subscribed to OnItemChanged
        if (onIntemChangedCallback != null)
        {
            onIntemChangedCallback.Invoke();
        }
    }
}
