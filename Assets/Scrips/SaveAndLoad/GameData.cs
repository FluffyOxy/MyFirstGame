using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float HP;
    public int currency;
    public int coin;
    public bool isNewGame;

    public SerializableDictionary<string, int> items;
    public SerializableDictionary<string, int> equipment;

    public SerializableDictionary<string, bool> skillTree;

    public bool isPlayerRemainingExist;
    public Vector3 playerRemainingPosition;
    public int playerLeftCurrency;

    public bool isPlayerHealthBarActive;

    public float bgmVolume;
    public float sfxVolume;
    public float envVolume;

    public string currentSceneName;

    public GameData()
    {
        HP = -1;
        currency = 0;
        isNewGame = true;
        items = new SerializableDictionary<string, int>();
        equipment = new SerializableDictionary<string, int>();
        skillTree = new SerializableDictionary<string, bool>();

        isPlayerRemainingExist = false;

        bgmVolume = 0.3688f;
        sfxVolume = 0.3688f;
        envVolume = 0.3688f;

        isPlayerHealthBarActive = false;
        currentSceneName = string.Empty;
    }
}
