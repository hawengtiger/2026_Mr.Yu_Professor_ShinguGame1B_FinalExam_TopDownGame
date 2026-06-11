using UnityEngine;

public class HPUI : MonoBehaviour
{
    public GameObject[] emptyHearts; // 빈 하트 10개
    public GameObject[] fillHearts;  // 채운 하트 10개

    private void Start()
    {
        RefreshHP();
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
}