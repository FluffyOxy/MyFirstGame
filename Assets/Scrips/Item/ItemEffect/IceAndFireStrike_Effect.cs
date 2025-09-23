using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ice And Fire Strike Effect", menuName = "Item Effect/Ice And Fire Strike")]
public class IceAndFireStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject IceAndFireStrikePrefab;
    [SerializeField] private DamageDataSerializable damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration;
    [SerializeField] private Vector2 generateOffset;

    public override void ExcuteEffect(EffectExcuteData _data)
    {
        Player player = PlayerManager.instance.player;
        IceAndFireStrike_EffectController iceAndFire = 
            Instantiate(
                IceAndFireStrikePrefab, 
                new Vector2(player.transform.position.x + player.facingDir * generateOffset.x, player.transform.position.y + generateOffset.y), 
                player.transform.rotation
            ).GetComponent<IceAndFireStrike_EffectController>();
        iceAndFire.SetUp(damage.GetDamageData(), moveSpeed, duration);
    }
}
