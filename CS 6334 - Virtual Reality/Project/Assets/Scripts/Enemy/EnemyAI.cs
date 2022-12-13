using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.hasPath)
            animator.SetFloat(Properties.ENEMY_SPEED, agent.velocity.magnitude);

        else
            animator.SetFloat(Properties.ENEMY_SPEED, 0);
    }
}
