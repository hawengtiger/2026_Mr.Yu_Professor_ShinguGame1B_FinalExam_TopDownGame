using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2Int moveDirection;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        RoomManager.Instance.MoveRoom(moveDirection);
    }
}