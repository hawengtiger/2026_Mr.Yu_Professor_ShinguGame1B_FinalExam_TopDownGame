using UnityEngine;

public class DropItem : MonoBehaviour
{
    public DropItemDataSO itemData;

    bool isPickedUp;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPickedUp) return;

        if (!collision.collider.CompareTag("Player"))
            return;

        isPickedUp = true;

        ApplyDrop();
    }

    void ApplyDrop()
    {
        switch (itemData.dropType)
        {
            case DropItemDataSO.DropItemType.Hp:

                if (PlayerStats.Instance.currentHp >=
                    PlayerStats.Instance.maxHp)
                {
                    isPickedUp = false;
                    return;
                }

                PlayerStats.Instance.currentHp += itemData.ItemBuff;

                PlayerStats.Instance.currentHp =
                    Mathf.Min(
                        PlayerStats.Instance.currentHp,
                        PlayerStats.Instance.maxHp);

                PlayerInventory.Instance.RefreshUI();

                Destroy(gameObject);
                break;

            case DropItemDataSO.DropItemType.Coin:

                PlayerInventory.Instance.AddCoin(itemData.ItemBuff);

                Destroy(gameObject);
                break;

            case DropItemDataSO.DropItemType.Key:

                PlayerInventory.Instance.AddKey(itemData.ItemBuff);

                Destroy(gameObject);
                break;
        }
    }
}