using UnityEngine;

public class DropItem : MonoBehaviour
{
    public DropItemDataSO itemData;

    bool isPickedUp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GetComponent<ShopItem>() != null)
            return;

        if (isPickedUp) return;

        if (!collision.CompareTag("Player"))
            return;

        PickUp();
    }

    public void PickUp()
    {
        ApplyDrop();
    }

    void ApplyDrop()
    {
        switch (itemData.dropType)
        {
            case DropItemDataSO.DropItemType.Hp:

                if (PlayerStats.Instance.currentHp >= PlayerStats.Instance.maxHp)
                {
                    isPickedUp = false;
                    return;
                }
                SoundManager.Instance.PlaySFX("Heart");
                HPUI.Instance.Heal(itemData.ItemBuff);

                Destroy(gameObject);
                break;

            case DropItemDataSO.DropItemType.Coin:
                SoundManager.Instance.PlaySFX("Coin");
                PlayerInventory.Instance.AddCoin(itemData.ItemBuff);

                Destroy(gameObject);
                break;

            case DropItemDataSO.DropItemType.Key:
                SoundManager.Instance.PlaySFX("Key");
                PlayerInventory.Instance.AddKey(itemData.ItemBuff);

                Destroy(gameObject);
                break;
        }
    }
}