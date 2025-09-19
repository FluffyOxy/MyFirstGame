using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Thunder Strike Effect", menuName = "Item Effect/Thunder Strike")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikePrefab;
    [SerializeField] private DamageDataSerializable damage;
    [SerializeField] private float yOffset = 0.5f;
    public override void ExcuteEffect(EffectExcuteData _data)
    {
        ThunderStrike_EffectController thunder = 
            Instantiate(
                thunderStrikePrefab, 
                new Vector3(_data.target.transform.position.x, _data.target.transform.position.y + yOffset), 
                Quaternion.identity
            ).GetComponent<ThunderStrike_EffectController>();

        thunder.SetUp(_data.target, damage.GetDamageData());
    }
}
