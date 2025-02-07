using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTextComponent : MonoBehaviour
{
    [Header("PopUp Text")]
    [SerializeField] private GameObject popUpTextPrefab;
    [TextArea][SerializeField] private string message;
    [SerializeField] private float lifeDuration;
    [SerializeField] private Transform popTransform;
    private PopUpText popUpText = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            CreatePopUpText();
        }
    }
    private void CreatePopUpText()
    {
        popUpText = Instantiate(popUpTextPrefab, popTransform.position, Quaternion.identity).GetComponent<PopUpText>();

        popUpText.SetUp(message, lifeDuration);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            FinishPopUpText();
        }
    }
    public void FinishPopUpText()
    {
        popUpText.PopOut();
    }
}
