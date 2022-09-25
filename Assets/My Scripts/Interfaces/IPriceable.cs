using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPriceable
{
    // return the price of this gameObject (could be an Item or maybe a Door?)
    public int GetPrice();

}
