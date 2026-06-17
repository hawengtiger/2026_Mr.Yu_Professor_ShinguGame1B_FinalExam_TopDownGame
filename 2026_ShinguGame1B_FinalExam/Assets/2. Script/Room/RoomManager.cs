using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;

    [Header("상점 가격 UI")]
    public TextMeshProUGUI heartPriceTMP;
    public TextMeshProUGUI keyPriceTMP;
    public TextMeshProUGUI passivePriceTMP;

    [Header("상점")]
    public GameObject shopHeartPrefab;
    public GameObject shopKeyPrefab;
    public GameObject[] shopPassivePrefabs;

    public float shopItemOffsetX = 0.9f;
    public float shopItemOffsetY = 0f;

    [Header("드랍 아이템")]
    public GameObject coinDropPrefab;
    public GameObject hpDropPrefab;
    public GameObject keyDropPrefab;

    public int minDropCount = 1;
    public int maxDropCount = 3;

    [Header("몬스터")]
    public GameObject[] normalEnemyPrefabs;
    public GameObject bossEnemyPrefab;
    public int normalEnemyCount = 3;

    private List<GameObject> spawnedEnemies = new();

    [Header("방 데이터")]
    public DoorDataSO normalRoomData;
    public DoorDataSO itemRoomData;
    public DoorDataSO shopRoomData;
    public DoorDataSO bossRoomData;

    [Header("황금방")]
    public GameObject pedestalPrefab;

    public GameObject[] passiveItemPrefabs;

    [Header("설정")]
    public int roomCount = 15;

    [Header("방 프리팹")]
    public GameObject roomPrefab;

    [Header("방 간격")]
    public float roomWidth = 3.8f;
    public float roomHeight = 2f;

    [Header("플레이어")]
    public Transform player;

    [Header("카메라")]
    public Camera mainCamera;

    public Vector2Int currentRoom = Vector2Int.zero;


    [Header("카메라 이동")]
    public float CameraMoveTime = 0.5f;
    public float horizontalOffset = 1.5f;
    public float verticalOffset = 0.7f;

    // 생성된 방 좌표
    public HashSet<Vector2Int> rooms = new();

    // 좌표 -> Room
    public Dictionary<Vector2Int, Room> roomDatas = new();

    private int currentPassivePrice;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            GenerateDungeon();
            SpawnRooms();
            AssignSpecialRooms();

            // 시작방은 항상 클리어
            roomDatas[Vector2Int.zero].isCleared = true;
            roomDatas[Vector2Int.zero].isEntered = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        HideShopPriceUI();
    }

    private void Update()
    {
        CheckRoomClear();
    }

    void EnterRoom(Vector2Int roomPos)
    {
        Room room = roomDatas[roomPos];

        if (room.isCleared)
        {
            CreateDoors();
            PaintDoors();
            return;
        }

        if (room.isEntered)
            return;

        room.isEntered = true;

        SpawnEnemies(room);
    }

    void SpawnEnemies(Room room)
    {
        CloseDoors();

        spawnedEnemies.Clear();

        Vector3 centerPos = new Vector3(
            room.gridPos.x * roomWidth,
            room.gridPos.y * roomHeight,
            0f
        );

        // 황금방, 상점방은 잡몹 생성 금지
        if (room.roomData.type == DoorDataSO.DoorType.Item ||
            room.roomData.type == DoorDataSO.DoorType.Shop)
        {
            room.isCleared = true;
            CreateDoors();
            PaintDoors();
            return;
        }

        // 보스방은 보스만 생성
        if (room.roomData.type == DoorDataSO.DoorType.Boss)
        {
            if (bossEnemyPrefab != null)
            {
                GameObject boss =
                    Instantiate(bossEnemyPrefab, centerPos, Quaternion.identity);

                spawnedEnemies.Add(boss);
            }

            return;
        }

        // 일반방만 잡몹 생성
        for (int i = 0; i < normalEnemyCount; i++)
        {
            if (normalEnemyPrefabs.Length == 0)
                return;

            GameObject enemyPrefab =
                normalEnemyPrefabs[
                    Random.Range(0, normalEnemyPrefabs.Length)];

            Vector3 spawnPos =
                centerPos + new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-0.5f, 0.5f),
                    0f);

            GameObject enemy =
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            spawnedEnemies.Add(enemy);
        }
    }

    void CheckRoomClear()
    {
        if (!roomDatas.ContainsKey(currentRoom))
            return;

        Room room = roomDatas[currentRoom];

        if (!room.isEntered || room.isCleared)
            return;

        spawnedEnemies.RemoveAll(enemy => enemy == null);

        if (spawnedEnemies.Count <= 0)
        {
            room.isCleared = true;

            SpawnDropItems(room);

            CreateDoors();
            PaintDoors();

            MiniMapManager miniMap =
    FindFirstObjectByType<MiniMapManager>();

            if (miniMap != null)
            {
                miniMap.RefreshMiniMap();
            }

            Debug.Log("방 클리어!");
        }
    }

    void SpawnDropItems(Room room)
    {
        int count = Random.Range(minDropCount, maxDropCount + 1);

        Vector3 centerPos = new Vector3(
            room.gridPos.x * roomWidth,
            room.gridPos.y * roomHeight,
            0f
        );

        for (int i = 0; i < count; i++)
        {
            GameObject prefab = GetRandomDrop();

            if (prefab == null)
                continue;

            Vector3 offset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.3f, 0.3f),
                0f
            );

            Instantiate(prefab, centerPos + offset, Quaternion.identity);
        }
    }

    GameObject GetRandomDrop()
    {
        int rand = Random.Range(0, 100);

        if (rand < 60)
            return coinDropPrefab;   // 60%

        if (rand < 85)
            return hpDropPrefab;     // 25%

        return keyDropPrefab;        // 15%
    }

    void CloseDoors()
    {
        GameObject[] doors =
            GameObject.FindGameObjectsWithTag("Door");

        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    void GenerateDungeon()
    {
        // 루프 안전장치 포함: 조건을 만족하지 못하면 맵을 처음부터 다시 만듭니다.
        int safetyNet = 0;
        bool generationSuccess = false;

        while (!generationSuccess && safetyNet < 100)
        {
            safetyNet++;
            rooms.Clear();

            Vector2Int current = Vector2Int.zero;
            rooms.Add(current);

            // 가지치기를 위해 현재 생성된 방들을 추적하는 리스트
            List<Vector2Int> openList = new List<Vector2Int> { current };

            Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            int iterations = 0;

            // 목표 방 개수만큼 채울 때까지 반복
            while (rooms.Count < roomCount && iterations < 1000)
            {
                iterations++;

                // 1. 이미 생성된 방들 중 하나를 무작위로 선택 (확장 기점)
                Vector2Int pickingRoom = openList[Random.Range(0, openList.Count)];
                Vector2Int next = pickingRoom + directions[Random.Range(0, 4)];

                // 2. 중복 검사: 이미 방이 존재하는 좌표면 패스
                if (rooms.Contains(next)) continue;

                // 3. 이웃 검사: 새로 생성될 좌표(next) 주변에 방이 몇 개 있는지 체크
                int neighborCount = 0;
                foreach (Vector2Int dir in directions)
                {
                    if (rooms.Contains(next + dir))
                    {
                        neighborCount++;
                    }
                }

                // 4. 핵심 조건: 주변에 방이 3개 이상(>=3) 있으면 소환 금지!
                // 이렇게 해야 골목이 꽉 차지 않고 막다른 끝방(Dead End)이 많이 생깁니다.
                if (neighborCount >= 3) continue;

                // 모든 조건을 통과하면 방 추가
                rooms.Add(next);
                openList.Add(next);
            }

            // 던전 생성이 끝난 후, 끝방(Dead End) 개수가 충분히 확보되었는지 임시 검사
            int deadEndCount = 0;
            foreach (Vector2Int pos in rooms)
            {
                if (pos == Vector2Int.zero) continue; // 시작방 제외

                int connections = 0;
                if (rooms.Contains(pos + Vector2Int.up)) connections++;
                if (rooms.Contains(pos + Vector2Int.down)) connections++;
                if (rooms.Contains(pos + Vector2Int.left)) connections++;
                if (rooms.Contains(pos + Vector2Int.right)) connections++;

                if (connections == 1) deadEndCount++;
            }

            // 끝방이 보스, 상점, 황금방을 배치할 수 있는 최소 3개 이상이면 루프 탈출 성공!
            if (deadEndCount >= 3)
            {
                generationSuccess = true;
            }
        }

        if (!generationSuccess)
        {
            Debug.LogError("맵 생성 실패: 조건을 만족하는 맵을 만들지 못했습니다. 설정 값을 확인하세요.");
        }
    }


    public void RespawnShopPassive(Vector3 position)
    {
        if (shopPassivePrefabs == null || shopPassivePrefabs.Length == 0)
            return;

        GameObject prefab =
            shopPassivePrefabs[
                Random.Range(0, shopPassivePrefabs.Length)];

        GameObject passive =
            Instantiate(
                prefab,
                position,
                Quaternion.identity);

        ShopItem shopItem = AddShopItem(passive);

        if (passivePriceTMP != null)
        {
            passivePriceTMP.gameObject.SetActive(true);
            passivePriceTMP.text = "25";
        }

        RefreshShopPriceColor();
    }

    void SpawnRooms()
    {
        foreach (Vector2Int pos in rooms)
        {
            Vector3 worldPos =
                new Vector3(
                    pos.x * roomWidth,
                    pos.y * roomHeight,
                    0);

            GameObject obj =
                Instantiate(
                    roomPrefab,
                    worldPos,
                    Quaternion.identity);

            Room room =
                obj.GetComponent<Room>();

            room.gridPos = pos;

            room.roomData = normalRoomData;

            roomDatas.Add(pos, room);
        }

        CreateDoors();


    }

    void CreateDoors()
    {
        foreach (var pair in roomDatas)
        {
            Vector2Int pos = pair.Key;
            Room room = pair.Value;

            bool up =
                rooms.Contains(pos + Vector2Int.up);

            bool down =
                rooms.Contains(pos + Vector2Int.down);

            bool left =
                rooms.Contains(pos + Vector2Int.left);

            bool right =
                rooms.Contains(pos + Vector2Int.right);

            room.SetDoors(
                up,
                down,
                left,
                right);
        }
    }

    void AssignSpecialRooms()
    {
        List<Vector2Int> deadEnds = new();

        foreach (Vector2Int pos in rooms)
        {
            int connections = 0;

            if (rooms.Contains(pos + Vector2Int.up))
                connections++;

            if (rooms.Contains(pos + Vector2Int.down))
                connections++;

            if (rooms.Contains(pos + Vector2Int.left))
                connections++;

            if (rooms.Contains(pos + Vector2Int.right))
                connections++;

            if (connections == 1)
            {
                deadEnds.Add(pos);
            }

        }

        if (deadEnds.Count < 3)
        {
            Debug.LogWarning(
                "끝방이 부족해서 특수방 생성 불가");

            return;
        }

        //--------------------------------
        // 보스방
        //--------------------------------

        Vector2Int bossRoom =
            deadEnds
            .OrderByDescending(
                p => Vector2Int.Distance(
                    Vector2Int.zero,
                    p))
            .First();

        roomDatas[bossRoom].roomData =
            bossRoomData;

        deadEnds.Remove(bossRoom);

        //--------------------------------
        // 상점
        //--------------------------------

        Vector2Int shopRoom =
            deadEnds[
                Random.Range(
                    0,
                    deadEnds.Count)];

        roomDatas[shopRoom].roomData =
            shopRoomData;

        deadEnds.Remove(shopRoom);

        //--------------------------------
        // 황금방
        //--------------------------------

        Vector2Int treasureRoom =
            deadEnds[
                Random.Range(
                    0,
                    deadEnds.Count)];

        roomDatas[treasureRoom].roomData =
            itemRoomData;

        PaintDoors();

        MiniMapManager miniMap =
    FindFirstObjectByType<MiniMapManager>();

        if (miniMap != null)
        {
            miniMap.CreateMiniMap();
        }

        Debug.Log($"보스방 : {bossRoom}");
        Debug.Log($"상점방 : {shopRoom}");
        Debug.Log($"황금방 : {treasureRoom}");
    }

    public void MoveRoom(Vector2Int dir)
    {
        HideShopPriceUI();

        Vector2Int nextRoom = currentRoom + dir;

        // 존재하는 방인지 체크
        if (!roomDatas.ContainsKey(nextRoom))
            return;

        currentRoom = nextRoom;

        Vector3 roomPos =
            new Vector3(
                currentRoom.x * roomWidth,
                currentRoom.y * roomHeight,
                0);

        mainCamera.transform.DOMove(
            new Vector3(
                roomPos.x,
                roomPos.y,
                mainCamera.transform.position.z), CameraMoveTime).SetEase(Ease.OutQuad);

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

        Room enteredRoom = roomDatas[currentRoom];

        if (enteredRoom.roomData.type == DoorDataSO.DoorType.Item)
        {
            SpawnTreasureRoomObject(currentRoom);
        }

        if (enteredRoom.roomData.type == DoorDataSO.DoorType.Shop)
        {
            SpawnShopItems(currentRoom);
        }

        if (enteredRoom.roomData.type == DoorDataSO.DoorType.Boss)
        {
            StartBossBattle(enteredRoom);
            return;
        }

        MiniMapManager minimap = FindFirstObjectByType<MiniMapManager>();

        if (minimap != null)
        {
            minimap.RefreshMiniMap();
        }

        EnterRoom(currentRoom);


    }

    void StartBossBattle(Room room)
    {
        if (room.isEntered)
            return;

        room.isEntered = true;

        BossIntroUI intro =
            FindFirstObjectByType<BossIntroUI>();

        Vector3 bossPos =
            new Vector3(
                room.gridPos.x * roomWidth,
                room.gridPos.y * roomHeight,
                0f);

        intro.PlayBossIntro(
            bossPos,
            (boss) =>
            {
                spawnedEnemies.Clear();
                spawnedEnemies.Add(boss);

                EnemyStats bossStats =
                    boss.GetComponent<EnemyStats>();

                if (bossStats != null &&
                    BossHPBar.Instance != null)
                {
                    BossHPBar.Instance.SetBoss(bossStats);
                }

                CloseDoors();
            });
    }

    void SpawnShopItems(Vector2Int roomPos)
    {
        Room room = roomDatas[roomPos];

        // 이미 상점 아이템은 생성돼 있으면, TMP만 다시 켜기
        if (room.isShopSpawned)
        {
            ShowShopPriceUI();
            RefreshShopPriceColor(); // 여기
            return;
        }

        room.isShopSpawned = true;

        Vector3 centerPos = new Vector3(
            roomPos.x * roomWidth,
            roomPos.y * roomHeight,
            0f);

        Vector3 leftPos = centerPos + Vector3.left * shopItemOffsetX;
        Vector3 middlePos = centerPos;
        Vector3 rightPos = centerPos + Vector3.right * shopItemOffsetX;

        GameObject heart =
            Instantiate(shopHeartPrefab, leftPos, Quaternion.identity);

        GameObject key =
            Instantiate(shopKeyPrefab, middlePos, Quaternion.identity);

        GameObject passive =
            Instantiate(
                shopPassivePrefabs[
                    Random.Range(0, shopPassivePrefabs.Length)],
                rightPos,
                Quaternion.identity);

        AddShopItem(heart);
        AddShopItem(key);
        AddShopItem(passive);

        SetupPriceTMP(heartPriceTMP, 5);
        SetupPriceTMP(keyPriceTMP, 10);
        SetupPriceTMP(passivePriceTMP, 25);

        RefreshShopPriceColor();
    }

    void ShowShopPriceUI()
    {
        if (heartPriceTMP != null)
            heartPriceTMP.gameObject.SetActive(true);

        if (keyPriceTMP != null)
            keyPriceTMP.gameObject.SetActive(true);

        if (passivePriceTMP != null)
            passivePriceTMP.gameObject.SetActive(true);
    }

    ShopItem AddShopItem(GameObject obj)
    {
        if (obj == null)
            return null;

        ShopItem shopItem = obj.GetComponent<ShopItem>();

        if (shopItem == null)
            shopItem = obj.AddComponent<ShopItem>();

        return shopItem;
    }

    void SetupPriceTMP(TextMeshProUGUI tmp, int price)
    {
        if (tmp == null)
            return;

        tmp.text = price.ToString();
        tmp.gameObject.SetActive(true);

        if (PlayerInventory.Instance.coin < price)
            tmp.color = Color.red;
        else
            tmp.color = Color.white;
    }

    public void RefreshShopPriceColor()
    {
        if (heartPriceTMP != null)
            heartPriceTMP.color =
                PlayerInventory.Instance.coin <  5? Color.red : Color.white;

        if (keyPriceTMP != null)
            keyPriceTMP.color =
                PlayerInventory.Instance.coin < 10 ? Color.red : Color.white;

        if (passivePriceTMP != null)
            passivePriceTMP.color =
                PlayerInventory.Instance.coin < 25 ? Color.red : Color.white;
    }

    void HideShopPriceUI()
    {
        if (heartPriceTMP != null)
            heartPriceTMP.gameObject.SetActive(false);

        if (keyPriceTMP != null)
            keyPriceTMP.gameObject.SetActive(false);

        if (passivePriceTMP != null)
            passivePriceTMP.gameObject.SetActive(false);
    }


    void SpawnTreasureRoomObject(Vector2Int roomPos)
    {
        Room room = roomDatas[roomPos];

        if (room.isTreasureSpawned)
            return;

        room.isTreasureSpawned = true;

        Vector3 centerPos =
            new Vector3(
                roomPos.x * roomWidth,
                roomPos.y * roomHeight,
                0f);

        // 받침대
        if (pedestalPrefab != null)
        {
            Instantiate(
                pedestalPrefab,
                centerPos + Vector3.down * 0.1f,
                Quaternion.identity);
        }

        // 랜덤 아이템
        if (passiveItemPrefabs.Length > 0)
        {
            GameObject prefab =
                passiveItemPrefabs[
                    Random.Range(
                        0,
                        passiveItemPrefabs.Length)];

            Instantiate(
                prefab,
                centerPos,
                Quaternion.identity);
        }
    }


    void PaintDoors()
    {
        foreach (var pair in roomDatas)
        {
            Vector2Int pos = pair.Key;
            Room room = pair.Value;

            PaintDoor(room, pos, Vector2Int.up);
            PaintDoor(room, pos, Vector2Int.down);
            PaintDoor(room, pos, Vector2Int.left);
            PaintDoor(room, pos, Vector2Int.right);
        }
    }

    void PaintDoor(
    Room room,
    Vector2Int pos,
    Vector2Int dir)
{
    Vector2Int nextPos = pos + dir;

    if (!roomDatas.ContainsKey(nextPos))
        return;

    Room nextRoom = roomDatas[nextPos];

    Color color = Color.white;

    if (room.roomData != normalRoomData)
    {
        color = room.roomData.color;
    }
    else if (nextRoom.roomData != normalRoomData)
    {
        color = nextRoom.roomData.color;
    }

    if (dir == Vector2Int.up)
        room.upDoor.GetComponent<SpriteRenderer>().color = color;

    else if (dir == Vector2Int.down)
        room.downDoor.GetComponent<SpriteRenderer>().color = color;

    else if (dir == Vector2Int.left)
        room.leftDoor.GetComponent<SpriteRenderer>().color = color;

    else if (dir == Vector2Int.right)
        room.rightDoor.GetComponent<SpriteRenderer>().color = color;
}

    public bool CanEnterRoom(Vector2Int roomPos)
    {
        if (!roomDatas.ContainsKey(roomPos))
            return false;

        Room room = roomDatas[roomPos];

        if (room.isUnlocked)
            return true;

        if (!room.roomData.needKey)
            return true;

        bool success =
            PlayerInventory.Instance.UseKey(
                room.roomData.keyCost);

        if (success)
        {
            room.isUnlocked = true;
            PaintDoors();
            return true;
        }

        Debug.Log("열쇠 부족");
        return false;
    }
}