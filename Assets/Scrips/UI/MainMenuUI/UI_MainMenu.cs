using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private UI_DarkScreen fadeScreen;

    private void Start()
    {
        if(!SaveManager.instance.HaveSaveData())
        {
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceenWithFadeEffect(fadeScreen.fadeDuration));
    }

    public void NewGame()
    {
        StartCoroutine(LoadSceenWithFadeEffect(fadeScreen.fadeDuration));
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceenWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }

    public void ExitGame()
    {
        //Application.Quit();
    }

}
