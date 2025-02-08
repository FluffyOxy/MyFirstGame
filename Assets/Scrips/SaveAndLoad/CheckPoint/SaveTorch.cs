using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTorch : MonoBehaviour, ISaveManager
{
    [SerializeField] private Animator backAnim;
    [SerializeField] private Animator anim;

    public void LoadData(GameData _data)
    {
        
    }

    public void SaveData(ref GameData _data)
    {
        _data.currentSceneName = SceneManager.GetActiveScene().name;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            backAnim.SetTrigger("Fire");
            anim.SetTrigger("Fire");
            SaveManager.instance.SaveGame();
        }
    }
}
