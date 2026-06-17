using TMPro;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private bool isBought;

    public TextMeshPro priceText;

    private static float nextBuyTime = 0f;
    public float buyCooldown = 1f;

    private void Awake()
    {
        if (priceText == null)
        {
            priceText = GetComponentInChildren<TextMeshPro>();
        }
    }

    private void Start()
    {
        int price = GetPrice();

        if (priceText != null)
        {
            priceText.text = price.ToString();

            if (price >= 25)
                priceText.color = Color.yellow;
            else
                priceText.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBought)
            return;

        if (Time.time < nextBuyTime)
            return;


        if (!collision.CompareTag("Player"))
            return;

        PassiveItem passive = GetComponent<PassiveItem>();
        DropItem drop = GetComponent<DropItem>();

        // 하트인데 풀피면 구매 자체 불가
        if (drop != null &&
            drop.itemData.dropType == DropItemDataSO.DropItemType.Hp &&
            PlayerStats.Instance.currentHp >= PlayerStats.Instance.maxHp)
        {
            Debug.Log("풀피라 구매 불가");
            return;
        }

        int price = GetPrice();

        if (!PlayerInventory.Instance.UseCoin(price))
        {
            Debug.Log("돈 부족");
            return;
        }

        nextBuyTime = Time.time + buyCooldown;

        isBought = true;

        if (passive != null)
        {
            Vector3 spawnPos = transform.position;

            passive.PickUp();

            RoomManager.Instance.RespawnShopPassive(spawnPos);

            return;
        }

        if (drop != null)
        {
            if (drop.itemData.dropType == DropItemDataSO.DropItemType.Hp)
                RoomManager.Instance.heartPriceTMP.gameObject.SetActive(false);

            else if (drop.itemData.dropType == DropItemDataSO.DropItemType.Key)
                RoomManager.Instance.keyPriceTMP.gameObject.SetActive(false);

            drop.PickUp();
            return;
        }

        Destroy(gameObject);
    }

    int GetPrice()
    {
        PassiveItem passive = GetComponent<PassiveItem>();
        if (passive != null && passive.itemData != null)
            return passive.itemData.ItemPrice;

        DropItem drop = GetComponent<DropItem>();
        if (drop != null && drop.itemData != null)
            return drop.itemData.ItemPrice;

        return 1;
    }
}