using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPointArray;

    public bool isPlayerRemainingExist;
    public Vector3 playerRemainingPosition;
    public int playerLeftCurrency;
    [SerializeField] private GameObject playerRemainingPrefab;
    


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }

        checkPointArray = FindObjectsOfType<CheckPoint>();
    }

    private void Start()
    {
        
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach (CheckPoint checkpoint in checkPointArray)
        {
            if(_data.checkpoints.TryGetValue(checkpoint.id, out bool isCheck))
            {
                if(isCheck)
                {
                    checkpoint.Check();
                }
            }
        }

        foreach(CheckPoint checkpoint in checkPointArray)
        {
            if(_data.closestCheckpointId == checkpoint.id)
            {
                PlayerManager.instance.player.transform.position = checkpoint.transform.position;
            }
        }

        isPlayerRemainingExist = _data.isPlayerRemainingExist;
        playerLeftCurrency = _data.playerLeftCurrency;
        playerRemainingPosition = _data.playerRemainingPosition;

        if(isPlayerRemainingExist)
        {
            PlayerRemaining playerRemaining = Instantiate(playerRemainingPrefab, playerRemainingPosition, Quaternion.identity).
                GetComponent<PlayerRemaining>();
            playerRemaining.Setup(playerLeftCurrency);
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.checkpoints.Clear();
        foreach(CheckPoint checkpoint in checkPointArray)
        {
            _data.checkpoints.Add(checkpoint.id, checkpoint.isCheck);
        }

        CheckPoint closestCheckpoint = TryGetClosestCheckPointToPlayer();
        if(closestCheckpoint != null)
        {
            _data.closestCheckpointId = closestCheckpoint.id;
        }

        _data.isPlayerRemainingExist = isPlayerRemainingExist;
        _data.playerLeftCurrency = playerLeftCurrency;
        _data.playerRemainingPosition = playerRemainingPosition;
    }

    public CheckPoint TryGetClosestCheckPointToPlayer()
    {
        CheckPoint closest = null;
        float minDistance = float.PositiveInfinity;
        foreach(CheckPoint checkPoint in checkPointArray)
        {
            if(checkPoint.isCheck)
            {
                float distance = Vector2.Distance(PlayerManager.instance.player.transform.position, checkPoint.transform.position);
                if (distance < minDistance)
                {
                    closest = checkPoint;
                    minDistance = distance;
                }
            }
        }
        return closest;
    }

    public void SetPauseGame(bool _isPause)
    {
        if(_isPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
