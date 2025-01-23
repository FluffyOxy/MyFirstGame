using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    public static PlayerManager instance;
    [SerializeField] public Player player;

    public int currency;
    public int coin;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        
    }

    public bool TrySpendMoney(int _money)
    {
        if(_money <= currency)
        {
            currency -= _money;
            
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetCurrencyAmount()
    {
        return currency;
    }

    public void AddCurrencyAmount(int _amount)
    {
        currency += _amount;
        
    }

    public void Die()
    {
        currency = 0;
    }

    public void LoadData(GameData _data)
    {
        currency = _data.currency;
        coin = _data.coin;
        player.isHealthBarActive = _data.isPlayerHealthBarActive;
        player.healthBar.SetActive(player.isHealthBarActive);
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = currency;
        _data.coin = coin;
        _data.isPlayerHealthBarActive = player.isHealthBarActive;
    }

    public int GetCoinAmount()
    {
        return coin;
    }

    public bool TrySpendCoin(int _coin)
    {
        if (_coin <= coin)
        {
            coin -= _coin;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddCoin(int _coin)
    {
        coin += _coin;
    }
}
