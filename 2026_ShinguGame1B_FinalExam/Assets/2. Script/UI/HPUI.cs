using UnityEngine;

public class HPUI : MonoBehaviour
{
    public static HPUI Instance;

    public GameObject[] emptyHearts; // 빈 하트 10개
    public GameObject[] fillHearts;  // 채운 하트 10개


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
    }

    private void Start()
    {
        RefreshHP();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DecreaseHP();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            IncreaseHP();
        }
    }

    public void RefreshHP()
    {
        int maxHp = Mathf.RoundToInt(PlayerStats.Instance.maxHp);
        int currentHp = Mathf.RoundToInt(PlayerStats.Instance.currentHp);

        for (int i = 0; i < emptyHearts.Length; i++)
        {
            emptyHearts[i].SetActive(i < maxHp);
            fillHearts[i].SetActive(i < currentHp);
        }
    }

    public void IncreaseHP()
    {

        if (PlayerStats.Instance.currentHp < PlayerStats.Instance.maxHp)
        {
            PlayerStats.Instance.currentHp = PlayerStats.Instance.maxHp;
            RefreshHP();
        }

        if (PlayerStats.Instance.maxHp >= 16)
        {
            return; // 최대 HP가 16 이상이면 더 이상 증가하지 않도록
        }
        
        PlayerStats.Instance.currentHp += 1;
        PlayerStats.Instance.maxHp += 1;
        RefreshHP();
    }

    public void DecreaseHP()
    {
        if(PlayerStats.Instance.currentHp <= 0)
        {
            return; // 현재 HP가 0 이하이면 더 이상 감소하지 않도록
        }

        PlayerStats.Instance.currentHp -= 1;
        RefreshHP();
    }

    public void Heal(float amount)
    {

        PlayerStats.Instance.currentHp = Mathf.Min(PlayerStats.Instance.currentHp + amount, PlayerStats.Instance.maxHp);

        RefreshHP();
    }

    public void TakeDamage(float amount)
    {
        PlayerStats.Instance.currentHp = Mathf.Max(PlayerStats.Instance.currentHp - amount, 0);

        RefreshHP();
    }
}
