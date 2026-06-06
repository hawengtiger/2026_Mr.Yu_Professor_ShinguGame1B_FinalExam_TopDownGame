using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public int keyCount;

    private void Awake()
    {
        Instance = this;
    }

    public bool UseKey(int amount)
    {
        if (keyCount < amount)
            return false;

        keyCount -= amount;

        return true;
    }

    public void AddKey(int amount)
    {
        keyCount += amount;
    }
}