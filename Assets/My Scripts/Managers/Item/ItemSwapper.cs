using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwapper : MonoBehaviour
{
    public static ItemSwapper instance;

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
