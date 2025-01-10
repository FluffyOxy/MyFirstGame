using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryRoom : Room
{
    public override void GenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.Entry;

        base.GenerateRoom(_manager, _currentLine, _index);
    }
}
