using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSo", menuName = "Scriptable Objects/EnemyDataSo")]
public class EnemyDataSo : ScriptableObject
{
    [Header("데미지")]
    public int Damage = 10;        // 아이템에 닿았을 경우 증가할 값

    [Header("속도")]
    public float Speed = 1.5f;        // 적 속도
    
    [Header("거리")]
    public int Duration = 1;        // 적 이동거리

    [Header("시간")]
    public float WaitTime = 1.2f;        // 적 시간



    public enum ItemType { GrayInkEnemy, RedInkEnemy, BlueInkEnemy}

    [Header("몬스터 타입")]
    public ItemType type;
}

