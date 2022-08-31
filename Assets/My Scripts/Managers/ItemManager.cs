using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private GameObject itemHolder;

    public Item activeEquipmentSlot;

    public event Action swapEquipmentEvent;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            //Debug.Log("Create player stats  instance");
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwapActiveEquipment()
    {
        if(swapEquipmentEvent != null)
        {
            swapEquipmentEvent();
        }
    }


    public GameObject GetItemHolder()
    {
        return itemHolder;
    }
}
