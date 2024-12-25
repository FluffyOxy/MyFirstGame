using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class InventoryItem
{
    public ItemData data;
    public int stackSize;

    public InventoryItem(ItemData _newItemData)
    {
        this.data = _newItemData;
        AddStack(1);
    }

    public void AddStack(int count)
    {
        stackSize += count;
    }

    public void RemoveStack(int count) 
    {
        if(stackSize >= count)
        {
            stackSize -= count;
        }
        else
        {
            Assert.IsTrue(stackSize >= count, "Check Stack Size Before Remove!");
        }
    }
}
