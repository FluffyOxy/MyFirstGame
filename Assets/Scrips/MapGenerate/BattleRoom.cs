using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRoom : Room
{
    protected override void PreGenerateRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        type = RoomType.Battle;

        base.PreGenerateRoom(_manager, _currentLine, _index);
    }

    protected override void GenerateCurrentRoom(MapGenerateManager _manager, Line _currentLine, int _index)
    {
        base.GenerateCurrentRoom(_manager, _currentLine, _index);

        GenerateEnemy(
            _currentLine.battles[_currentLine.battleIndex].difficulty, 
            _manager.enemyList, 
            _manager.enemyGenerateYOffset
        );
    }

    protected void GenerateEnemy(float _enemyDifficultyAmount, List<GameObject> _enemyList, float _enemyGenerateYOffset)
    {
        float currentEnemyDifficulty = 0;
        while(currentEnemyDifficulty < _enemyDifficultyAmount)
        {
            Vector2 randomPosition = flatPositions[Random.Range(0, flatPositions.Count)];
            randomPosition = new Vector2(randomPosition.x, randomPosition.y + _enemyGenerateYOffset);
            GameObject randomEnemy = _enemyList[Random.Range(0, _enemyList.Count)];

            Enemy newEnemy = 
                Instantiate(randomEnemy, randomPosition, Quaternion.identity).GetComponent<Enemy>();
            currentEnemyDifficulty += newEnemy.difficulty;
        }
    }
}
