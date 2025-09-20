using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class UI_TradeBlock : MonoBehaviour
{
    [SerializeField] private GameObject tradeSlotPrefab;
    [SerializeField] private RectTransform tradeSlotsParent;

    private bool isSetup = false;
    private List<Dialog> successDialogs;
    private List<Dialog> noCoinDialogs;
    private List<Dialog> fullBagDialogs;

    private Dialog currentDialog;
    private int dialogIndex;

    public void Setup(List<ItemData> _products, List<Dialog> _successDialog, List<Dialog> _noCoinDialogs, List<Dialog> _fullBagDialogs)
    {
        if(isSetup)
        {
            return;
        }

        foreach (ItemData product in _products)
        {
            UI_TradeSlot newSlot = Instantiate(tradeSlotPrefab, tradeSlotsParent).GetComponent<UI_TradeSlot>();
            newSlot.Setup(product, product.price);
        }

        successDialogs = _successDialog; 
        noCoinDialogs = _noCoinDialogs;
        fullBagDialogs = _fullBagDialogs;

        isSetup = true;
    }

    public void TradeSuccess()
    {
        SetCurrentDialog(successDialogs);
    }
    public void TradeNoCoin()
    {
        SetCurrentDialog(noCoinDialogs);
    }
    public void TradeFullBag()
    {
        SetCurrentDialog(fullBagDialogs);
    }
    private void SetCurrentDialog(List<Dialog> _dialogs)
    {
        dialogIndex = 0;
        currentDialog = _dialogs[UnityEngine.Random.Range(0, _dialogs.Count)];

        UI.instance.Speak(currentDialog.sentences[dialogIndex]);
        ++dialogIndex;
    }

    private void Update()
    {
        if (currentDialog != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (dialogIndex < currentDialog.sentences.Count)
                {
                    UI.instance.Speak(currentDialog.sentences[dialogIndex]);
                    ++dialogIndex;
                }
                else
                {
                    UI.instance.SpeakDone();
                    PlayerManager.instance.player.SetCanInput(false);
                    currentDialog = null;
                }
            }
        }
    }
}
