using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public void Open()
    {
        GetComponentInChildren<Animator>().SetTrigger("Open");
    }

    public void SetDrops(List<Drop> _drops)
    {
        GetComponent<DropItem>().SetDrops(_drops);
    }
}
