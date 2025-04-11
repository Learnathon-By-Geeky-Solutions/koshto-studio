
//enemy core script
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyCore : MonoBehaviour
{
    [Header("Shared Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 5f;

    [HideInInspector] public Transform player;

    [HideInInspector] public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }
}
