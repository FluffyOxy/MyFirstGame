using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("Room Info")]
    [SerializeField] public float height;
    [SerializeField] public float width;
    [SerializeField] public Access upperAccess;
    [SerializeField] public Access lowerAccess;
    [SerializeField] public Access leftAccess;
    [SerializeField] public Access rightAccess;

    public virtual void GenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        Debug.Log(_index);

        Room newRoom = null;
        //判断下一个房间类型
        RoomType roomType = _currentLine.GetNextRoomType(_index);

        Debug.Log(roomType);

        switch (roomType)
        {
            case RoomType.Battle:
                newRoom = GetNewRoomByPrefabList(_manager.battleRoomPrefabs);
                break;
            case RoomType.Passage:
                newRoom = GetNewRoomByPrefabList(_manager.passageRoomPrefabs);
                break;
            case RoomType.Exit:
                newRoom = GetNewRoomByPrefabList(_manager.exitRoomPrefabs);
                break;
        }
        newRoom.GenerateRoom(_manager, _currentLine, _index + 1);
    }

    protected Room GetNewRoomByPrefabList(List<GameObject> _list)
    {
        //选一个房间
        int battleRoomIndex = Random.Range(0, _list.Count);

        //计算房间位置
        Room nextBattleRoom = _list[battleRoomIndex].GetComponent<Room>();
        Vector3 nextBattleRoomPosition = GetNextRoomPosition(rightAccess, nextBattleRoom);

        //生成房间
        Room newBattleRoom =
            Instantiate(_list[battleRoomIndex], nextBattleRoomPosition, Quaternion.identity).GetComponent<Room>();
        return newBattleRoom;
    }

    protected Vector3 GetNextRoomPosition(Access _exitAccess, Room _nextRoom)
    {
        Transform nextRoomEnterTransform;
        if (_exitAccess == upperAccess)
        {
            nextRoomEnterTransform = _nextRoom.lowerAccess.transform;
        }
        else if (upperAccess == lowerAccess)
        {
            nextRoomEnterTransform = _nextRoom.upperAccess.transform;
        }
        else if (upperAccess == leftAccess)
        {
            nextRoomEnterTransform = _nextRoom.rightAccess.transform;
        }
        else
        {
            nextRoomEnterTransform = _nextRoom.leftAccess.transform;
        }

        return _exitAccess.transform.position - nextRoomEnterTransform.position;
    }
}
