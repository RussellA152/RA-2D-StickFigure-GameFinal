using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSystemDemo : MonoBehaviour
{
    public delegate void onEnableDelegate();
    public static event onEnableDelegate onEnableOccurred;     // Use this variable to add events/methods onto
    /*   Usage(must have both enable and disable) :
        * -----------------------------------------------*/
    private void OnEnable()
    {
        UpdateSystemDemo.onEnableOccurred += OnEnable;

        if (onEnableOccurred != null)
            onEnableOccurred();
    }
    private void OnDisable()
    {
        UpdateSystemDemo.onEnableOccurred -= OnEnable;
    }


    /*
    private void Update()
    {
        if (UpdateOccurred != null)                   // If there are methods attached to the UpdateOccurred event...
            UpdateOccurred();                         // Call all of those methods at the same time in one Update() function
    }
    */

}
