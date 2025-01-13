using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestAnimation : MonoBehaviour
{
    public void DropItemTrigger()
    {
        GetComponentInParent<DropItem>().Drop();
    }
}
