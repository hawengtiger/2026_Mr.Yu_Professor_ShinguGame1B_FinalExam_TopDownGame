using TMPro;
using UnityEngine;
using System.IO;

public class PlayerInventory : MonoBehaviour
{
    [System.Serializable]
    public class InventorySaveData
    {
        public int coin;
        public int key;
    }

    public static PlayerInventory Instance;

    string SavePath =>
    Path.Combine(
        Application.persistentDataPath,
        "inventory.json");

    [Header("재화")]
    public int coin;
    public int key;

    [Header("UI")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI keyText;

    private void Awake()
    {
        Instance = this;

        Load();
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
        if (Input.GetKeyDown(KeyCode.F1))
        {
            DeleteSave();
        }
    }
    public void AddCoin(int amount)
    {
        if (coin >= 99999)
            return;

        coin += amount;

        Save();

        RefreshUI();

        if (RoomManager.Instance != null)
            RoomManager.Instance.RefreshShopPriceColor();
    }

    public void AddKey(int amount)
    {
        if (key >= 99999)
            return;

        key += amount;

        Save();

        RefreshUI();
    }

    public bool UseKey(int amount)
    {
        if (key < amount)
            return false;

        key -= amount;

        Save();

        RefreshUI();

        return true;
    }

    public bool UseCoin(int amount)
    {
        if (coin < amount)
            return false;

        coin -= amount;

        Save();

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

    void Save()
    {
        InventorySaveData data =
            new InventorySaveData();

        data.coin = coin;
        data.key = key;

        string json =
            JsonUtility.ToJson(data, true);

        File.WriteAllText(
            SavePath,
            json);
    }

    void Load()
    {
        if (!File.Exists(SavePath))
            return;

        string json =
            File.ReadAllText(SavePath);

        InventorySaveData data =
            JsonUtility.FromJson<InventorySaveData>(json);

        coin = data.coin;
        key = data.key;
    }
    
    void DeleteSave() //임시 데이터 저장 파괴
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
        }

        coin = 0;
        key = 0;

        RefreshUI();

        Debug.Log("인벤토리 저장 삭제 완료");
    }
}