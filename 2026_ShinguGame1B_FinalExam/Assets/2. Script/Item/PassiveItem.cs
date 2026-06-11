using UnityEngine;

public class PassiveItem : MonoBehaviour
{
    public ItemDataSo itemData;

    private bool isPickedUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPickedUp) return;

        if (!collision.CompareTag("Player"))
            return;

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
                stats.maxHp += itemData.hp;
                stats.currentHp += itemData.hp;
                break;

            case ItemDataSo.PassiveItemType.White_DMG:
                stats.damage += itemData.dmg;
                break;

            case ItemDataSo.PassiveItemType.Tape_AR:
                stats.range += itemData.range;
                break;

            case ItemDataSo.PassiveItemType.Water_CT:
                stats.cooldown -= itemData.cooltime;
                stats.cooldown =
                    Mathf.Max(0.1f, stats.cooldown);
                break;

            case ItemDataSo.PassiveItemType.Dry_ATR:
                stats.attackDuration += itemData.attackTime;
                break;

            case ItemDataSo.PassiveItemType.Shoes_SPD:
                stats.moveSpeed += itemData.speed;
                break;
        }
    }
}