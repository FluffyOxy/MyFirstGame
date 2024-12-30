using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBomberman_ExplodeStunnedState : Enemytate
{
    Enemy_Bomberman enemy;
    private bool haveExploded = false;
    public EnemyBomberman_ExplodeStunnedState(Enemy _enemyBase, EnemyStateMachine _enemyStateMachine, string _animBoolName, Enemy_Bomberman _enemy) : base(_enemyBase, _enemyStateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("EnemyBomberman_ExplodeStunnedState");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if(enemy.isExplode && !haveExploded)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.explodeCore.position, enemy.explodeRadius);
            foreach(Collider2D hit in colliders)
            {
                if(hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>() != enemy)
                {
                    hit.GetComponent<Enemy>().cs.TakeDamage(enemy.stunnedExplodeDamage.GetDamageData(), enemy.transform);
                }
            }
            haveExploded = true;
        }
    }
}
