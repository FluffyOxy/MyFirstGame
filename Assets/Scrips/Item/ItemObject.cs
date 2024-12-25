using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData data;
    [SerializeField] private float pickUpCooldown = 0.5f;
    [SerializeField] public float pickUpVelocity = 7f;
    [SerializeField] public float pickUpDuration = 0.3f;
    [SerializeField] public string CannotPickUpWarning = "±³°üÒÑÂú";

    public Rigidbody2D rg => GetComponent<Rigidbody2D>();

    private void OnValidate()
    {
        if(data == null)
        {
            return;
        }    
        GetComponent<SpriteRenderer>().sprite = data.icon;
        gameObject.name = "Item object = " + data.name;
    }

    private void Start()
    {
        if (data == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = data.icon;
        gameObject.name = "Item object = " + data.name;
    }

    private void Update()
    {
        
    }

    public bool TryPickUpItem()
    {
        if(Inventory.instance.TryAddItem(data))
        {
            Invoke("DestroySelf", pickUpDuration);
            return true;
        }
        else
        {
            PlayerManager.instance.player.fx.CreatePopUpText(CannotPickUpWarning);
            return false;
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ThrowToward(Vector2 _velocity)
    {
        GetComponentInChildren<ItemObjectTrigger>().isThrowing = true;
        rg.velocity = _velocity;
        Invoke("SetIsThrowingToFalse", pickUpCooldown);
    }
    private void SetIsThrowingToFalse()
    {
        GetComponentInChildren<ItemObjectTrigger>().isThrowing = false;
    }

    public void SetItemData(ItemData _data)
    {
        data = _data;
    }
}
