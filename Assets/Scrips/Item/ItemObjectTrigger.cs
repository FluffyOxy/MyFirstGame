using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject item => GetComponentInParent<ItemObject>();
    public bool isThrowing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isThrowing && collision.GetComponent<Player>() != null && !collision.GetComponent<Player>().isDead)
        {
            item.rg.velocity = new Vector2(0, item.pickUpVelocity);
            item.TryPickUpItem();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isThrowing && collision.GetComponent<Player>() != null)
        {
            isThrowing = false;
        }
    }
}
