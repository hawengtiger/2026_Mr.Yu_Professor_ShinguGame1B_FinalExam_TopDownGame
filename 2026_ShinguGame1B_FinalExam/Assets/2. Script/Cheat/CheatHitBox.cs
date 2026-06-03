using UnityEngine;

public class CheatHitBox : MonoBehaviour
{
    public GameObject hitBox;

    private bool isHitbox = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitBox.SetActive(isHitbox);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SetAllTargetsActive(isHitbox);
        }
    }

    void SetAllTargetsActive(bool isActive)
    {
        isHitbox = !isActive;
        hitBox.SetActive(isHitbox);
    }
}
