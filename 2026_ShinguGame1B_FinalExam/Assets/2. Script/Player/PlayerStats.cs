using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public TextMeshProUGUI dmgTxT, rangeTxt, cooldownTxt, moveSpeedTxt, attackDurationTxt;

    public PlayerDataSO statsData;

    public float currentHp;

    [Range(5f, 16f)]
    public float maxHp;

    [Range(1f, 1000f)]
    public float damage;

    [Range(0.11f, 1.1f)]
    public float range;

    [Range(0.1f, 1f)]
    public float cooldown;

    [Range(1f, 3f)]
    public float moveSpeed;

    [Range(0.1f, 1f)]
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

        maxHp = statsData.maxhp;
        damage = statsData.attackDMG;
        range = statsData.attackRange;
        cooldown = statsData.attackCooldown;
        moveSpeed = statsData.speed;
        attackDuration = statsData.attackTimeRange;

        currentHp = maxHp;
    }

    private void Update()
    {
        dmgTxT.text = $"DMG : {damage:F1}";
        rangeTxt.text = $"RNG : {range:F1}";
        cooldownTxt.text = $"ACT : {cooldown:F1}";
        moveSpeedTxt.text = $"SPD : {moveSpeed:F1}";
        attackDurationTxt.text = $"ATR : {attackDuration:F1}";
    }
}