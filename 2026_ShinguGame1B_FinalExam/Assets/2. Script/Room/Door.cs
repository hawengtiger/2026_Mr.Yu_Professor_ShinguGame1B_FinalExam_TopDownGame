using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2Int moveDirection;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        Vector2Int targetRoom =
            RoomManager.Instance.currentRoom + moveDirection;

        if (!RoomManager.Instance.CanEnterRoom(targetRoom))
            return;

        RoomManager.Instance.MoveRoom(moveDirection);
    }
}