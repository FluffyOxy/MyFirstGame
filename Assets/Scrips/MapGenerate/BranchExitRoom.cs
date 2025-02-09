using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchExitRoom : Room
{
    [SerializeField] private Door door;
    private void Awake()
    {
        door = GetComponentInChildren<Door>();
    }

    protected override void GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        door.otherDoor = _currentLine.lineStartDoor.transform;
    }

    protected override void GenerateNextRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        
    }

    protected override void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.BranchExit;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }
}
