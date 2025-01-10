using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [Header("Enemy Drop Info")]
    [SerializeField] protected List<Drop> drops;
    [SerializeField] protected GameObject itemObjectPrefab;
    [SerializeField] protected Vector2 maxDropVelocity;
    [SerializeField] protected Vector2 minDropVelocity;

    public void Drop()
    {
        foreach (var drop in drops)
        {
            if (Random.Range(0, 100) < drop.dropChance)
            {
                ItemObject newItemObject = Instantiate(itemObjectPrefab, transform.position, Quaternion.identity).GetComponent<ItemObject>();
                Vector2 velocity = new Vector2(Random.Range(-1, 1) * Random.Range(minDropVelocity.x, maxDropVelocity.x), Random.Range(minDropVelocity.y, maxDropVelocity.y));
                newItemObject.ThrowToward(velocity);
                newItemObject.SetItemData(drop.item);
            }
        }
    }
}
