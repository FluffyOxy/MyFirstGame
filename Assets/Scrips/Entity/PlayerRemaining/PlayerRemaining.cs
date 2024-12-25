using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRemaining : MonoBehaviour
{
    private int currencyAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            PlayerManager.instance.AddCurrencyAmount(currencyAmount);
            GameManager.instance.isPlayerRemainingExist = false;
            Destroy(gameObject);
        }
    }

    public void Setup(int _currencyAmount)
    {
        currencyAmount = _currencyAmount;
    }
}
