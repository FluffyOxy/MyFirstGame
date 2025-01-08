using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : Room
{
    public override void GenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        //������Ҫ���ɵ�ս������
        --_currentLine.battleCount;

        base.GenerateRoom(_manager, _currentLine, _index);
    }
}
