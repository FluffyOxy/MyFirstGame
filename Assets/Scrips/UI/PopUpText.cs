using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    [SerializeField] private TextMeshPro myText;

    [SerializeField] private float popInSpeed;
    [SerializeField] private float popOutSpeed;
    [SerializeField] private float popOutBeginAlpha;
    [SerializeField] private float colorFadeSpeed;

    [SerializeField] private float lifeDuration;

    private float textTimer;

    public void SetUp(string _text)
    {
        myText.text = _text;
        textTimer = lifeDuration;
    }

    void Update()
    {
        textTimer -= Time.deltaTime;
        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorFadeSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (alpha < popOutBeginAlpha)
            {
                transform.position =
                    Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), popOutSpeed * Time.deltaTime);
            }
            if(alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position =
                    Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + 1), popInSpeed * Time.deltaTime);
        }
    }
}
