using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public PlayerDataSO statsData;

    public float currentHp;
    public float maxHp;
    public float damage;
    public float range;
    public float cooldown;
    public float moveSpeed;
    public float attackDuration;

    private void Awake()
    {
        // 이미 인스턴스가 존재한다면? (기존에 이미 있으면)
        if (Instance != null)
        {
            Destroy(gameObject); // 새로 만들어진 자신을 파괴
            return;
        }

        // 없다면 나를 인스턴스로 등록
        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬이 넘어가도 파괴되지 않음

        maxHp = statsData.maxhp;
        damage = statsData.attackDMG;
        range = statsData.attackRange;
        cooldown = statsData.attackCooldown;
        moveSpeed = statsData.speed;
        attackDuration = statsData.attackTimeRange;

        currentHp = maxHp;
    }
}