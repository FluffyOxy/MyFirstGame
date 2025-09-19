using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    [HideInInspector] public int coinAmount;
    [HideInInspector] public bool isThrowing;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!isThrowing)
        {
            if (_collision.GetComponent<Player>() != null)
            {
                PlayerManager.instance.AddCoin(coinAmount);
                Destroy(GetComponentInParent<Coin>().gameObject);
            }
        }
    }
}
