using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_ExplodeState : EnemyState
{
    Enemy_Bomberman enemy;
    private bool haveExploded = false;
    public EnemyBomberman_ExplodeState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Bomberman _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("EnemyBomberman_ExplodeState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.isExplode && !haveExploded)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.explodeCore.position, enemy.explodeRadius);
            foreach (Collider2D hit in colliders)
            {
                if (hit.GetComponent<Player>() != null)
                {
                    hit.GetComponent<Player>().cs.TakeDamage(enemy.explodeDamage.GetDamageData(), enemy.transform);
                }
            }
            haveExploded = true;
        }
    }
}
