
using UnityEngine;

[CreateAssetMenu(fileName = "DropItemDataSO", menuName = "Scriptable Objects/DropItemDataSO")]
public class DropItemDataSO : ScriptableObject
{
    [Header("아이템 값")]
    public int ItemBuff = 1;        // 아이템에 닿았을 경우 증가할 값

    public enum DropItemType { Hp, Coin, Key }
    [Header("드랍 아이템 타입")]
    public DropItemType dropType;
}
