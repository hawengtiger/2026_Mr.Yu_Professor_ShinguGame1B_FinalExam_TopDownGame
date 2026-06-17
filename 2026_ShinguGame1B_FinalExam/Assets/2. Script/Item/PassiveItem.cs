using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public ItemDataSo itemData;

    private bool isPickedUp = false;

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
        if (isPickedUp) return;

        PlayerStats stats = PlayerStats.Instance;

        if (stats == null)
        {
            Debug.Log("PlayerStats 없음");
            return;
        }

        if (itemData == null)
        {
            Debug.Log("ItemDataSO 없음");
            return;
        }

        isPickedUp = true;

        ApplyPassive(stats);

        if (ItemGetUI.Instance != null)
        {
            ItemGetUI.Instance.Show(itemData);
        }

        AttackController attack =
            FindFirstObjectByType<AttackController>();

        if (attack != null)
        {
            attack.RefreshStats();
        }

        Debug.Log("패시브 적용됨: " + itemData.passiveType);

        Destroy(gameObject);
    }

    void ApplyPassive(PlayerStats stats)
    {
        switch (itemData.passiveType)
        {
            case ItemDataSo.PassiveItemType.Ink_Hp:
                SoundManager.Instance.PlaySFX("Passive");
                HPUI.Instance.IncreaseHP();
                break;

            case ItemDataSo.PassiveItemType.White_DMG:
                SoundManager.Instance.PlaySFX("Passive");
                stats.damage *= itemData.dmg;
                break;

            case ItemDataSo.PassiveItemType.Tape_AR:
                SoundManager.Instance.PlaySFX("Passive");
                stats.range += itemData.range;
                break;

            case ItemDataSo.PassiveItemType.Water_CT:
                SoundManager.Instance.PlaySFX("Passive");
                stats.cooldown -= itemData.cooltime;
                stats.cooldown = Mathf.Max(0.1f, stats.cooldown);
                break;

            case ItemDataSo.PassiveItemType.Dry_ATR:
                SoundManager.Instance.PlaySFX("Passive");
                stats.attackDuration += itemData.attackTime;
                break;

            case ItemDataSo.PassiveItemType.Shoes_SPD:
                SoundManager.Instance.PlaySFX("Passive");
                stats.moveSpeed += itemData.speed;
                break;
        }
    }
}