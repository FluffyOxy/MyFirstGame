using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObject item => GetComponentInParent<ItemObject>();
    public bool isThrowing = false;

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!isThrowing && _collision.GetComponent<Player>() != null && !_collision.GetComponent<Player>().isDead)
        {
            item.rg.velocity = new Vector2(0, item.pickUpVelocity);
            item.TryPickUpItem();
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (isThrowing && _collision.GetComponent<Player>() != null)
        {
            isThrowing = false;
        }
    }
}
