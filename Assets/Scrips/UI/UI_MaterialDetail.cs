using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MaterialDetail : MonoBehaviour
{
    [SerializeField] private Image materialIcon;
    [SerializeField] private TextMeshProUGUI materialName;
    [SerializeField] private TextMeshProUGUI materialDescription;
    [SerializeField] private TextMeshProUGUI coinAmountText;

    private int coinAmount;
    private ItemData itemData;

    public void Setup(ItemData _item, int _coinAmount)
    {
        materialIcon.sprite = _item.icon;
        materialName.text = _item.itemName;
        materialDescription.text = _item.description;
        coinAmountText.text = _coinAmount.ToString();
        gameObject.SetActive(true);

        coinAmount = _coinAmount;
        itemData = _item;
    }

    public void Purchase()
    {
        if (Inventory.instance.CanAddItem(itemData))
        {
            if (PlayerManager.instance.TrySpendCoin(coinAmount))
            {
                Inventory.instance.TryAddItem(itemData);
                GetComponentInParent<UI_TradeBlock>().TradeSuccess();
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
