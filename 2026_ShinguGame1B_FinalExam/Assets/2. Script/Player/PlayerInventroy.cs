using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    [Header("¿Á»≠")]
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

    public void AddCoin(int amount)
    {
        coin += amount;
        RefreshUI();
    }

    public void AddKey(int amount)
    {
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

    public void RefreshUI()
    {
        coinText.text = coin.ToString();
        keyText.text = key.ToString();
    }
}