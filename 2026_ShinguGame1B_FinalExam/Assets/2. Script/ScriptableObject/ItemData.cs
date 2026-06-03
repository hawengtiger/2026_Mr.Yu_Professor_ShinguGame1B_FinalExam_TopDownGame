using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public class ItemDataSo : ScriptableObject
{
    [Header("아이템 값")]
    public int ItemBuff = 1;        // 아이템에 닿았을 경우 증가할 값

    public enum ItemType { Coin, Key, Ink, White, Tape, Dry, Shoes}

    [Header("아이템 타입")]
    public ItemType type;
}

