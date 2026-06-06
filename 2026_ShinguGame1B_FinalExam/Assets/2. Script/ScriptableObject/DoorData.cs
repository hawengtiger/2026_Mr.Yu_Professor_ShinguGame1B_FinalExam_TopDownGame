using UnityEngine;

[CreateAssetMenu(fileName = "DoorDataSO", menuName = "Scriptable Objects/DoorDataSO")]
public class DoorDataSO : ScriptableObject
{
    [Range(1, 4)]
    [Header("방 단계")]
    public int roomLevel = 1;

    [Range(8, 20)]
    [Header("방 개수")]
    public int roomCount = 8;

    [Header("문 색")]
    public Color color;     // 아이템에 닿았을 경우 증가할 값

    [Header("열쇠 필요 여부")]
    public bool needKey;

    [Header("열쇠 소모량")]
    public int keyCost = 1;

    public enum DoorType { Normal, Item , Shop, Boss}

    [Header("아이템 타입")]
    public DoorType type;
}

