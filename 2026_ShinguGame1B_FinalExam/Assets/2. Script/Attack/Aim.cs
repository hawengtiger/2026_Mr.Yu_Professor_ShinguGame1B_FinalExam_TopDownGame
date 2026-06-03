using UnityEngine;

public class Aim : MonoBehaviour
{
    void Update()
    {
        Vector3 mouse =  Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouse.z = 0;

        Vector2 dir = mouse - transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}