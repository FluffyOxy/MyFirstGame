using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole_HotKeyController : MonoBehaviour
{
    private KeyCode hotKey;

    private TextMeshProUGUI myText;
    private SpriteRenderer sr;

    private Enemy enemy;
    private BlackHoleController blackHole;

    private bool isOverlaped;

    public void SetupHotKey(KeyCode _hotKey, BlackHoleController _blackHole, Enemy _enemy)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();

        hotKey = _hotKey;
        myText.text = hotKey.ToString();

        enemy = _enemy;
        blackHole = _blackHole;

        isOverlaped = true;
        sr.color = Color.clear;
        myText.color = Color.clear;
    }

    private void Update()
    {
        if(isOverlaped)
        {
            if(!isOverlapOtherHotKeyLabel())
            {
                myText.color = Color.white;
                sr.color = Color.black;
                isOverlaped = false;
            }
        }

        if(Input.GetKeyDown(hotKey))
        {
            blackHole.TargetEnemy(enemy);
            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }

    private bool isOverlapOtherHotKeyLabel()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, sr.size.x);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<BlackHole_HotKeyController>() != null)
            {
                if (hit.GetComponent<BlackHole_HotKeyController>().myText.color != Color.clear)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
