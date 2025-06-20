using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour, ISaveManager
{
    [SerializeField] private string defaultSeneName;
    private string sceneName;
    [SerializeField] private GameObject continueButton = null;
    [SerializeField] private UI_DarkScreen fadeScreen;
    [SerializeField] private bool isHaveContinueButton = true;

    private void Start()
    {
        if(isHaveContinueButton)
        {
            if (!SaveManager.instance.HaveSaveData())
            {
                continueButton.gameObject.SetActive(false);
            }
            else
            {
                continueButton.gameObject.SetActive(true);
            }
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceenWithFadeEffect(fadeScreen.fadeDuration));
    }

    public void NewGame()
    {
        SaveManager.instance.DeleteSaveData();
        sceneName = defaultSeneName;

        StartCoroutine(LoadSceenWithFadeEffect(fadeScreen.fadeDuration));
    }

    private IEnumerator LoadSceenWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadData(GameData _data)
    {
        if(_data.currentSceneName != string.Empty)
        {
            sceneName = _data.currentSceneName;
        }
    }

    public void SaveData(ref GameData _data)
    {
        
    }
}
