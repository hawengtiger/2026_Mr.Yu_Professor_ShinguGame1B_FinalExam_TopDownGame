using UnityEngine;

public class CheatRoom : MonoBehaviour
{
    public GameObject[] targets;

    void Start()
    {
        // Door 태그 찾기
        targets = GameObject.FindGameObjectsWithTag("Door");
    }

    void Update()
    {
        // 1번 키를 누르면 모두 비활성화
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetAllTargetsActive(false);
        }

        // 2번 키를 누르면 모두 활성화
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetAllTargetsActive(true);
        }
    }

    void SetAllTargetsActive(bool isActive)
    {
        foreach (GameObject target in targets)
        {
            if (target != null) // 오브젝트가 파괴되었을 경우를 대비한 예외 처리
            {
                target.SetActive(isActive);
            }
        }
    }
}