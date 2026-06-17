using UnityEngine;

public class PhenoraBoss : MonoBehaviour
{
    public GameObject inkPrefab;

    public Transform firePoint;

    public float attackCooldown = 2f;

    float timer;

    private void Update()
    {
        if (BossIntroUI.IsPlaying)
            return;

        timer += Time.deltaTime;

        if (timer >= attackCooldown)
        {
            timer = 0f;

            Shoot();
        }
    }

    void Shoot()
    {
        if (PlayerStats.Instance == null)
            return;

        GameObject ink =
            Instantiate(
                inkPrefab,
                firePoint.position,
                Quaternion.identity);

        Ink_Fly rocket =
            ink.GetComponent<Ink_Fly>();

        if (rocket != null)
        {
            rocket.SetTarget(
                PlayerStats.Instance.transform);
        }
    }
}