using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILockable
{
    void AddLock();

    //void FullfillCondition();

    void RemoveLock();
}
