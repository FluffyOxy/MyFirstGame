using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTorch : MonoBehaviour, ISaveManager
{
    [SerializeField] private Animator backAnim;
    [SerializeField] private Animator anim;
    [SerializeField] private string loadSceneName = string.Empty;

    private bool isSaving = false;

    public void LoadData(GameData _data)
    {
        
    }

    public void SaveData(ref GameData _data)
    {
        if (isSaving)
        {
            if (loadSceneName == string.Empty)
            {
                _data.currentSceneName = SceneManager.GetActiveScene().name;
            }
            else
            {
                _data.currentSceneName = loadSceneName;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            backAnim.SetTrigger("Fire");
            anim.SetTrigger("Fire");

            isSaving = true;
            SaveManager.instance.SaveGame();
            isSaving = false;
        }
    }
}
