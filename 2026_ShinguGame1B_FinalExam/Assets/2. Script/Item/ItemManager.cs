using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerInventory.Instance.AddKey(amount);

        Destroy(gameObject);
    }
}

