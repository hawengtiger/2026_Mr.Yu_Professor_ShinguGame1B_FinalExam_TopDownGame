using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    [Header("미니맵 색상")]
    public Color normalColor = Color.gray;
    public Color currentColor = Color.white;
    public Color startColor = Color.black;
    public Color shopColor = new Color(1f, 0.5f, 0f);
    public Color itemColor = Color.yellow;
    public Color bossColor = Color.red;

    public RectTransform mapParent;
    public GameObject roomIconPrefab;

    public float roomIconGap = 20f;

    private Dictionary<Vector2Int, MiniMapRoom> icons = new();

    public void CreateMiniMap()
    {
        Debug.Log("미니맵 생성 시작");
        Debug.Log("방 개수 : " + RoomManager.Instance.rooms.Count);

        foreach (Vector2Int pos in RoomManager.Instance.rooms)
        {
            Debug.Log("미니맵 방 생성 : " + pos);

            GameObject obj =
                Instantiate(roomIconPrefab, mapParent);

            RectTransform rect =
                obj.GetComponent<RectTransform>();

            rect.anchoredPosition =
                new Vector2(pos.x * roomIconGap, pos.y * roomIconGap);

            MiniMapRoom icon =
                obj.GetComponent<MiniMapRoom>();

            icons.Add(pos, icon);
        }

        RefreshMiniMap();
    }

    public void RefreshMiniMap()
    {
        foreach (var pair in icons)
        {
            Vector2Int pos = pair.Key;
            MiniMapRoom icon = pair.Value;

            Room room = RoomManager.Instance.roomDatas[pos];

            bool isStartRoom = pos == Vector2Int.zero;

            bool isCurrentRoom = pos == RoomManager.Instance.currentRoom;

            bool isNeighborRoom = Vector2Int.Distance(pos, RoomManager.Instance.currentRoom) == 1;

            if (!room.isCleared && !isCurrentRoom && !isNeighborRoom)
            {
                icon.SetVisible(false);
                continue;
            }
            icon.SetVisible(true);

            if (isCurrentRoom)
            {
                icon.SetColor(currentColor);
            }
            else if (isStartRoom)
            {
                icon.SetColor(startColor);
            }
            else if (room.roomData.type == DoorDataSO.DoorType.Shop)
            {
                icon.SetColor(shopColor);
            }
            else if (room.roomData.type == DoorDataSO.DoorType.Item)
            {
                icon.SetColor(itemColor);
            }
            else if (room.roomData.type == DoorDataSO.DoorType.Boss)
            {
                icon.SetColor(bossColor);
            }
            else
            {
                icon.SetColor(normalColor);
            }
        }
    }
}