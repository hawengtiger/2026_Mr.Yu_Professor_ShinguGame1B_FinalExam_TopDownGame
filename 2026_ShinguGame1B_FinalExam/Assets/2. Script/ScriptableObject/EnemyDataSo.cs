using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSo", menuName = "Scriptable Objects/EnemyDataSo")]
public class EnemyDataSo : ScriptableObject
{
    [Header("데미지")]
    public int Damage = 10;        // 아이템에 닿았을 경우 증가할 값

    public enum ItemType { GrayInkEnemy, RedInkEnemy, BlueInkEnemy}

    [Header("몬스터 타입")]
    public ItemType type;
}

