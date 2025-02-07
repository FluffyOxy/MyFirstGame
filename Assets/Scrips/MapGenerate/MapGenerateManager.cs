using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;

public enum RoomType
{
    Entry,
    Exit,
    Battle,
    Passage,
    Reward,
    Branch
}

[Serializable]
public class Access
{
    public Transform transform;
    public bool canEnter = false;
    public bool canExit = false;
}

[Serializable]
public class LineSlot
{
    [Range(0, 100)][SerializeField] public float battleRoomRate;
    [Range(0, 100)][SerializeField] public float rewardRoomRate;
    [Range(0, 100)][SerializeField] public float branchRoomRate;
}

[Serializable]
public class BattleSlot
{
    [SerializeField] public float difficulty; 
}

[Serializable]
public class RewardSlot
{
    [SerializeField] public int rewardAmount;
    [SerializeField] public int advancedAmount;
    [Range(0, 100)][SerializeField] public float witcherRate;
    [Range(0, 100)][SerializeField] public float traderRate;
    [Range(0, 100)][SerializeField] public float advancedRewardRate;
    [Range(0, 100)][SerializeField] public float mimicRate;
    [Range(0, 100)][SerializeField] public float mimicAdvancedRewardRate;
}

[Serializable]
public class BranchSlot
{
    [SerializeField] public bool isRandom;
    [SerializeField] public Line branchLine = null;
}

[Serializable]
public class Line
{
    public RoomType lineEndRoomType;
    [HideInInspector] public bool isEndRoom = false;

    public int battleIndex { get; private set; }
    public int rewardIndex { get; private set; }
    public int branchIndex { get; private set; }

    [Header("Room Info")]
    public List<LineSlot> lineRooms;

    [Header("Battle Info")]
    public List<BattleSlot> battles;

    [Header("Reward Info")]
    public List<RewardSlot> rewards;
    public RewardSlot lineEndReward;

    [Header("Branch Info")]
    public List<BranchSlot> branches;

    public Line()
    {
        
    }

    public Line GetClone()
    {
        Line newLine = new Line();

        newLine.lineEndRoomType = lineEndRoomType;
        newLine.lineRooms = lineRooms;
        newLine.battles = battles;
        newLine.rewards = rewards;
        newLine.lineEndReward = lineEndReward;
        newLine.branches = branches;

        return newLine;
    }

    public RoomType GetNextRoomType(int _currentRoomIndex, RoomType _currentRoomType)
    {
        if(isEndRoom)
        {
            return RoomType.Exit;
        }

        if(_currentRoomType == RoomType.Battle)
        {
            ++battleIndex;
        }
        if (_currentRoomType == RoomType.Reward)
        {
            ++rewardIndex;
        }
        if(_currentRoomType == RoomType.Branch)
        {
            ++branchIndex;
        }

        if (_currentRoomIndex >= lineRooms.Count)
        {
            if (branchIndex < branches.Count)
            {
                return RoomType.Branch;
            }
            if (battleIndex < battles.Count)
            {
                return RoomType.Battle;
            }
            if (rewardIndex < rewards.Count)
            {
                return RoomType.Reward;
            }
            isEndRoom = true;
            return lineEndRoomType;
        }

        if (branchIndex >= branches.Count)
        {
            lineRooms[_currentRoomIndex].branchRoomRate = 0;
        }
        if (battleIndex >= battles.Count)
        {
            lineRooms[_currentRoomIndex].battleRoomRate = 0;
        }
        if(rewardIndex >= rewards.Count)
        {
            lineRooms[_currentRoomIndex].rewardRoomRate = 0;
        }

        float rate = lineRooms[_currentRoomIndex].battleRoomRate;
        float dice = UnityEngine.Random.Range(0, 100);

        if (dice < rate)
        {
            return RoomType.Battle;
        }
        else if(dice < (rate += lineRooms[_currentRoomIndex].rewardRoomRate))
        {
            return RoomType.Reward;
        }
        else if(dice < (rate += lineRooms[_currentRoomIndex].branchRoomRate))
        {
            return RoomType.Branch;
        }
        else
        {
            return RoomType.Passage;
        }
    }
}

//每个场景考虑使用自己的地图生成器
public class MapGenerateManager : MonoBehaviour
{
    //房间类型划分为入口、出口、战斗、通道、下分支、上分支、左分支终点、右分支终点几种。其中入口出口分别在右边和左边右出入口，战斗和通道在两边都有出入口，下分支在左右下方有出入口，上分支在左右上有出入口，所有房间都有上方的入口
    [Header("Room Prefabs")]
    [SerializeField] public List<GameObject> entryRoomPrefabs;
    [SerializeField] public List<GameObject> exitRoomPrefabs;
    [SerializeField] public List<GameObject> battleRoomPrefabs;
    [SerializeField] public List<GameObject> passageRoomPrefabs;
    [SerializeField] public List<GameObject> rewardRoomPrefabs;
    [SerializeField] public List<GameObject> branchRoomPrefabs;

    [Header("Line info")]
    [SerializeField] public Line mainLine = new Line();
    [SerializeField] public List<Line> branchLines = new List<Line>();

    [Header("Map Info")]
    [SerializeField] private Transform startTransform;
    [SerializeField] public List<GameObject> enemyList;
    [SerializeField] public float enemyGenerateYOffset = 1f;

    [Header("Reward Info")]
    [SerializeField] private List<Drop> primaryRewards;
    [SerializeField] private List<Drop> advancedRewards;
    [SerializeField] public GameObject primaryRewardChestPrefab;
    [SerializeField] public GameObject advancedRewardChestPrefab;
    [SerializeField] public GameObject mimicChestPrefab;
    [SerializeField] public GameObject traderPrefab;
    [SerializeField] public GameObject witcherPrefab;

    [Header("Room Decoration Info")]
    [SerializeField] public GameObject decorationPrefab;
    [SerializeField] public List<Sprite> decorations;
    [SerializeField] public int minDecorationAmount;
    [SerializeField] public int maxDecorationAmount;

    [Header("Branch Info")]
    [SerializeField] public float branchYOffset;

    private void Start()
    {
        int startRoomIndex = UnityEngine.Random.Range(0, entryRoomPrefabs.Count);

        Room startRoomTemp = entryRoomPrefabs[startRoomIndex].GetComponent<Room>();
        Vector3 startRoomLoc = startTransform.position - startRoomTemp.leftAccess.transform.position;

        Room startRoom = Instantiate(entryRoomPrefabs[startRoomIndex], startRoomLoc, Quaternion.identity).GetComponent<Room>();
        startRoom.GenerateRoom(this, mainLine, 0);
    }

    public List<Drop> GetPrimaryRewards(int _amount)
    {
        return GetRewards(_amount, primaryRewards);
    }

    public List<Drop> GetAdvancedRewards(int _amount)
    {
        return GetRewards(_amount, advancedRewards);
    }

    private List<Drop> GetRewards(int _amount, List<Drop> _rewardPool) 
    {
        float sumWeight = 0f;
        foreach(var item in _rewardPool)
        {
            sumWeight += item.dropChance;
        }

        List<Drop> rewards = new List<Drop>();
        for(int i = 0; i < _amount; i++)
        {
            float dice = UnityEngine.Random.Range(0f, sumWeight);
            float rate = 0f;
            foreach (var item in _rewardPool)
            {
                rate += item.dropChance;
                if(dice < rate)
                {
                    rewards.Add(item); 
                    break;
                }
            }
        }

        foreach(var item in rewards)
        {
            item.dropChance = 100f;
        }

        return rewards;
    }
}
