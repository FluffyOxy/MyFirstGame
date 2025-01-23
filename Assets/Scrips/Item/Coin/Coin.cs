using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int maxAmount = 100;
    [SerializeField] private float pickUpCooldown;

    public void Setup(int _coinAmount)
    {
        GetComponentInChildren<CoinTrigger>().coinAmount = _coinAmount;
    }

    public void ThrowToward(Vector2 _velocity)
    {
        GetComponentInChildren<CoinTrigger>().isThrowing = true;
        GetComponent<Rigidbody2D>().velocity = _velocity;
        Invoke("SetIsThrowingToFalse", pickUpCooldown);
    }
    private void SetIsThrowingToFalse()
    {
        GetComponentInChildren<CoinTrigger>().isThrowing = false;
    }
}
