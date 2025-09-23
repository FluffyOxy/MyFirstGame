using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FireFloor Effect", menuName = "Item Effect/FireFloor")]
public class FireFloorEffect : ItemEffect
{
    [SerializeField] private GameObject fireFloorCollectionPrefab;

    public override void ExcuteEffect(EffectExcuteData _targetData)
    {
        Player player = PlayerManager.instance.player;
        FireFloorCollectionController newCollection =
            Instantiate(fireFloorCollectionPrefab, player.transform.position, Quaternion.identity).GetComponent<FireFloorCollectionController>();
        newCollection.Setup(_targetData);
    }
}
