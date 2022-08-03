using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{

    [SerializeField] private List<Item> items = new List<Item>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(Item item)
    {
        //adding given item to the list
        items.Add(item);

        //testing to see if item actually does what it needs to 
        items[0].ItemAction(this.gameObject);
    }
}
