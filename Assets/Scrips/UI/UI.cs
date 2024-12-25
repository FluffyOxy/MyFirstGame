using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    [Header("ToolTip")]
    public UI_ItemToolTip itemToolTip;
    public UI_StatsDescriptionToolTip statsDescriptionToolTip;
    public UI_WarningToolTip warningToolTip;
    public UI_SkillToolTip skillToolTip;
    [Header("Windows")]
    public UI_CraftWindow craftWindow;
    [Header("UI")]
    [SerializeField] public GameObject charactorUI;
    [SerializeField] public GameObject skillTreeUI;
    [SerializeField] public GameObject craftUI;
    [SerializeField] public GameObject optionUI;
    [SerializeField] public GameObject inGame;
    [Header("FadeScreen")]
    [SerializeField] private UI_DarkScreen darkScreen;
    [SerializeField] private UI_DeadMessage deadMessage;
    [SerializeField] private float deadMessageDelay;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        ActivateAllUI();
    }

    void Start()
    {
        SwitchTo(inGame);
        itemToolTip.HideToolTip();
        statsDescriptionToolTip.HideToolTip();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.U)) 
        {
            SwitchWithKeyTo(charactorUI);
        }
        else if(Input.GetKeyDown(KeyCode.I)) 
        {
            SwitchWithKeyTo(skillTreeUI);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(craftUI);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchWithKeyTo(optionUI);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchTo(inGame);
        }
    }

    public void Die()
    {
        darkScreen.FadeOut();
        deadMessage.gameObject.SetActive(true);
        deadMessage.Invoke("FadeIn", deadMessageDelay);
    }

    public void ActivateAllUI()
    {
        charactorUI.SetActive(true);
        skillTreeUI.SetActive(true);
        craftUI.SetActive(true);
        optionUI.SetActive(true);
    }

    public void HideAllUI()
    {
        charactorUI.SetActive(false);
        skillTreeUI.SetActive(false);
        craftUI.SetActive(false);
        optionUI.SetActive(false);
    }

    public void SwitchTo(GameObject _menu)
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_DarkScreen>() != null;
            if (!isFadeScreen)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            
        }

        if(_menu != null)
        {
            _menu.SetActive(true);
        }

        if(GameManager.instance != null)
        {
            if(_menu != inGame)
            {
                GameManager.instance.SetPauseGame(true);
            }
            else
            {
                GameManager.instance.SetPauseGame(false);
            }
        }
    }

    private void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            SwitchTo(inGame);
        }
        else
        {
            SwitchTo(_menu);
        }
    }

    public void RestartScene()
    {
        deadMessage.FadeOut();
        GameManager.instance.Invoke("RestartScene", deadMessage.fadeDuration);
    }
}