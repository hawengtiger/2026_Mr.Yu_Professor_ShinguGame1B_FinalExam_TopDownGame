using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int gridPos;

    public DoorDataSO roomData;

    public bool isTreasureSpawned;
    
    [Header("문")]
    public GameObject upDoor;
    public GameObject downDoor;
    public GameObject leftDoor;
    public GameObject rightDoor;

    [Header("문 색상")]
    public SpriteRenderer upDoorSR;
    public SpriteRenderer downDoorSR;
    public SpriteRenderer leftDoorSR;
    public SpriteRenderer rightDoorSR;

    public void SetDoors(
        bool up,
        bool down,
        bool left,
        bool right)
    {
        upDoor.SetActive(up);
        downDoor.SetActive(down);
        leftDoor.SetActive(left);
        rightDoor.SetActive(right);
    }
}