using UnityEngine;

public class WhiteAttack : MonoBehaviour
{
    public float lifeTime = 0.15f;

    public void Init(Vector2 start, Vector2 end)
    {
        Vector2 center = (start + end) * 0.5f;

        float distance = Vector2.Distance(start, end);

        transform.position = center;

        Vector2 dir = (end - start).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.localScale = new Vector3(distance, 0.2f, 1);

        Destroy(gameObject, lifeTime);
    }
}