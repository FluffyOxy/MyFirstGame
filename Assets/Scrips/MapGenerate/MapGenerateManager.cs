using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public enum RoomType
{
    Entry,
    Exit,
    Battle,
    Passage
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
}

[Serializable]
public class Line
{
    public RoomType lineEndRoomType;
    public int battleCount;

    public List<LineSlot> lineRooms;

    public bool shouldEnd()
    {
        return battleCount <= 0;
    }

    public RoomType GetNextRoomType(int _currentRoomIndex)
    {
        if (_currentRoomIndex >= lineRooms.Count)
        {
            if (battleCount > 0)
            {
                return RoomType.Battle;
            }
            return lineEndRoomType;
        }


        if (battleCount <= 0)
        {
            lineRooms[_currentRoomIndex].battleRoomRate = 0;
        }

        float rate = lineRooms[_currentRoomIndex].battleRoomRate;
        float dice = UnityEngine.Random.Range(0, 100);

        if (dice < rate)
        {
            return RoomType.Battle;
        }
        else
        {
            return RoomType.Passage;
        }
    }
}

//ÿ����������ʹ���Լ��ĵ�ͼ������
public class MapGenerateManager : MonoBehaviour
{
    //�������ͻ���Ϊ��ڡ����ڡ�ս����ͨ�����·�֧���Ϸ�֧�����֧�յ㡢�ҷ�֧�յ㼸�֡�������ڳ��ڷֱ����ұߺ�����ҳ���ڣ�ս����ͨ�������߶��г���ڣ��·�֧�������·��г���ڣ��Ϸ�֧���������г���ڣ����з��䶼���Ϸ������
    [Header("Room Prefabs")]
    [SerializeField] public List<GameObject> entryRoomPrefabs;
    [SerializeField] public List<GameObject> exitRoomPrefabs;
    [SerializeField] public List<GameObject> battleRoomPrefabs;
    [SerializeField] public List<GameObject> passageRoomPrefabs;

    [Header("Line info")]
    [SerializeField] public Line mainLine;

    [SerializeField] private Transform startTransform;

    private void Start()
    {
        int startRoomIndex = UnityEngine.Random.Range(0, entryRoomPrefabs.Count);
        Room startRoom = Instantiate(entryRoomPrefabs[startRoomIndex], startTransform.position, Quaternion.identity).GetComponent<Room>();
        startRoom.GenerateRoom(this, mainLine, 0);
    }
}
