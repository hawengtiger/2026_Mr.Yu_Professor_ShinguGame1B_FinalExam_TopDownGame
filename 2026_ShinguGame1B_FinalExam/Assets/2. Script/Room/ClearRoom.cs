/*using UnityEngine;

public class ClearRoom : MonoBehaviour
{
    public Transform player;
    public Camera cam;

    public Transform nextRoomSpawn;
    public Transform nextCameraPos;

    public RoomManager targetRoom;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        // 방이 안깨졌으면 이동 불가
        if (!targetRoom.isClear)
            return;

        player.position = nextRoomSpawn.position;

        cam.transform.position = new Vector3(nextCameraPos.position.x, nextCameraPos.position.y, cam.transform.position.z);
    }
}*/