using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunWndProjectile : DamageZoomController
{
    [SerializeField] private float speed;
    [SerializeField] private bool isMagicEffectUseStatsValue = true;

    private bool isRight;

    public override void Project(EffectExcuteData _targetData)
    {
        base.Project(_targetData);
        PlayerManager.instance.player.GetStats().CalculateDamageDataWithStats(ref damage, isMagicEffectUseStatsValue, false);

        isRight = PlayerManager.instance.player.transform.position.x < _targetData.target.transform.position.x;
    }

    protected override void Update()
    {
        base.Update();

        if(isRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.right, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position - transform.right, speed * Time.deltaTime);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D _collision)
    {
        base.OnTriggerEnter2D(_collision);

        if (_collision.GetComponent<Enemy>() != null)
        {
            Enemy target = _collision.GetComponent<Enemy>();
            target.OpenCounterAttackWindow();
            target.TryToBeStuuned();
            target.CloseCounterAttackWindow();
        }
    }
}
