using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : Room
{
    public override void GenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        //减少需要生成的战斗房数
        --_currentLine.battleCount;

        base.GenerateRoom(_manager, _currentLine, _index);
    }
}
