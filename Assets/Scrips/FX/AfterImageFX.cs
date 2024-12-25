using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageFX : MonoBehaviour
{
    private SpriteRenderer sr;
    private float colorLooseSpeed;

    public void Setup(float _looseSpeed, Sprite _image, bool isFacingLeft)
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = _image;
        colorLooseSpeed = _looseSpeed;
        if(isFacingLeft)
        {
            transform.Rotate(0, 180, 0);
        }
    }

    private void Update()
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - colorLooseSpeed * Time.deltaTime);
        if(sr.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}
