using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_TradeWindowEquipment : MonoBehaviour
{
    [SerializeField] private UI_ItemToolTip itemDetail;
    [SerializeField] private TextMeshProUGUI coinAmountText;

    private int coinAmount;
    private ItemData_Equipment equipment;

    public void Setup(ItemData_Equipment _itemdata, int _coinAmount)
    {
        gameObject.SetActive(true);
        itemDetail.ShowToolTip(_itemdata);
        coinAmountText.text = _coinAmount.ToString();

        coinAmount = _coinAmount;
        equipment = _itemdata;
    }

    public void Purchase()
    {
        if(Inventory.instance.CanAddItem(equipment))
        {
            if (PlayerManager.instance.TrySpendCoin(coinAmount))
            {
                Inventory.instance.TryAddItem(equipment);
                GetComponentInParent<UI_TradeBlock>().TradeSuccess();
                SceneAudioManager.instance.uiSFX.buy.Play(null);
            }
            else
            {
                GetComponentInParent<UI_TradeBlock>().TradeNoCoin();
            }
        }
        else
        {
            GetComponentInParent<UI_TradeBlock>().TradeFullBag();
        }
    }
}
