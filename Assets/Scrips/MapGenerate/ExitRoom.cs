using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoom : Room
{
    public override void GenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.Exit;
    }
}
