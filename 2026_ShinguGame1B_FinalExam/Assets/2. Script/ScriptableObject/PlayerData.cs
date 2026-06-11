using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO",menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    [Range(0.11f, 1.1f)]
    public float attackRange = 0.11f;

    [Range(0.1f, 1f)]
    public float attackTimeRange = 0.1f;

    [Range(0.1f, 1f)]
    public float attackCooldown = 1f;

    [Range(1f, 1000f)]
    public float attackDMG = 1f;

    [Range(5f, 16f)]
    public float maxhp = 5f;

    [Range(1f, 3f)]
    public float speed = 1f;
}