using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class chaseEnemy : MonoBehaviour
{
    [Header("NavMesh")]
    [SerializeField] private Transform playerPosition;
    [SerializeField] private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        agent.SetDestination(playerPosition.position);
    }
}
