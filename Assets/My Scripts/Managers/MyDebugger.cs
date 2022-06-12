using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebugger : MonoBehaviour
{

    [SerializeField] private GameObject objectToReEnable;

    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(ReEnableMe());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ReEnableMe()
    {
        yield return new WaitForSeconds(timer);

        //check if this gameobject is even disabled...
        if(objectToReEnable.activeInHierarchy.Equals(false))
            objectToReEnable.SetActive(true);
    }
}
