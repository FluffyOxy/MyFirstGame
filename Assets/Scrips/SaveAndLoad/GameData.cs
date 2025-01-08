using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public bool isNewGame;

    public SerializableDictionary<string, int> items;
    public SerializableDictionary<string, int> equipment;

    public SerializableDictionary<string, bool> skillTree;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public bool isPlayerRemainingExist;
    public Vector3 playerRemainingPosition;
    public int playerLeftCurrency;

    public bool isPlayerHealthBarActive;

    public float bgmVolume;
    public float sfxVolume;
    public float envVolume;

    public GameData() 
    { 
        this.currency = 0;
        isNewGame = true;
        items = new SerializableDictionary<string, int>();
        equipment = new SerializableDictionary<string, int>();
        skillTree = new SerializableDictionary<string, bool>();
        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;

        isPlayerRemainingExist = false;

        bgmVolume = 0.3688f;
        sfxVolume = 0.3688f;
        envVolume = 0.3688f;

        isPlayerHealthBarActive = false;
    }
}
