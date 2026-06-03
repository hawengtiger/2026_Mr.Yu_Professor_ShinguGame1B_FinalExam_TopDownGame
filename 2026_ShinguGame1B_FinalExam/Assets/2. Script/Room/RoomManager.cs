using UnityEngine;
using DG.Tweening;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("플레이어")]
    public Transform player;

    [Header("카메라")]
    public Camera mainCamera;

    [Header("방 크기")]
    public float roomWidth = 20f;
    public float roomHeight = 12f;

    [Header("카메라 이동")]
    public float cameraMoveTime = 0.5f;

    public float horizontalOffset = 1f;
    public float verticalOffset = 0.5f;

    public Vector2Int currentRoom = Vector2Int.zero;

    private bool isMovingRoom = false;

    private void Awake()
    {
        Instance = this;
    }

    public void MoveRoom(Vector2Int dir)
    {
        if (isMovingRoom)
            return;

        isMovingRoom = true;

        currentRoom += dir;

        Vector3 roomPos = new Vector3(
            currentRoom.x * roomWidth,
            currentRoom.y * roomHeight,
            0);

        Vector3 playerPos = roomPos;

        if (dir == Vector2Int.right)
            playerPos += Vector3.left * horizontalOffset;

        else if (dir == Vector2Int.left)
            playerPos += Vector3.right * horizontalOffset;

        else if (dir == Vector2Int.up)
            playerPos += Vector3.down * verticalOffset;

        else if (dir == Vector2Int.down)
            playerPos += Vector3.up * verticalOffset;

        player.position = playerPos;

        mainCamera.transform.DOMove(
            new Vector3(
                roomPos.x,
                roomPos.y,
                mainCamera.transform.position.z),
            cameraMoveTime)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                isMovingRoom = false;
            });
    }
}