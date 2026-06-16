using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("재화")]
    public int coin;
    public int key;

    [Header("UI")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI keyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RefreshUI();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha5))
        {
            AddCoin(5);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            AddKey(5);
        }
    }
    public void AddCoin(int amount)
    {
        if (coin >= 99999)
        {
            return; // 코인이 99999 이상이면 더 이상 증가하지 않도록
        }

        coin += amount;
        RefreshUI();

        if (RoomManager.Instance != null)
            RoomManager.Instance.RefreshShopPriceColor();
    }

    public void AddKey(int amount)
    {
        if (key >= 99999)
        {
            return; // 열쇠가 99999 이상이면 더 이상 증가하지 않도록
        }

        key += amount;
        RefreshUI();
    }

    public bool UseKey(int amount)
    {
        if (key < amount)
            return false;

        key -= amount;
        RefreshUI();

        return true;
    }

    public bool UseCoin(int amount)
    {
        if (coin < amount)
            return false;

        coin -= amount;
        RefreshUI();

        if (RoomManager.Instance != null)
            RoomManager.Instance.RefreshShopPriceColor();

        return true;
    }

    public void RefreshUI()
    {
        coinText.text = coin.ToString();
        keyText.text = key.ToString();
    }
}