using UnityEngine;

public class AIIdleState : AIState
{
    private Vector3 playerDirection;
    private Vector3 agentDirection;
    private float dotProduct;
    private int waypointIndex = 0;
    private float waitTime = 5.0f;
    private bool isInvestigatingShootingSound;

    AIStateId AIState.GetId()
    {
        return AIStateId.Idle;
    }

    void AIState.Enter(AIAgent agent)
    {

    }

    void AIState.Update(AIAgent agent)
    {
        playerDirection = agent.playerTransform.position - agent.transform.position;

        if (agent.isHit && !agent.health.isDead)
        {
            agent.weaponIK.targetTransform = agent.playerTransform;
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
        }

        if (agent.playerFire.hasFired && !agent.health.isDead)
        {
            agent.navMeshAgent.SetDestination(agent.lastShootingSoundTransform);
            isInvestigatingShootingSound = true;
            waitTime = 15.0f;
        }

        if (isInvestigatingShootingSound && (agent.transform.position - agent.shootingBuilding.lastFirePosition).sqrMagnitude < 5.0f)
        {
            waitTime = waitTime - Time.deltaTime;
            if (waitTime <= 0.0f)
            {
                waitTime = 15.0f;
                isInvestigatingShootingSound = false;
                NextPoint(agent);
            }
        }

        if (playerDirection.magnitude > agent.config.maxSightDistance)
        {
            waitTime = waitTime - Time.deltaTime;

            if (waitTime <= 0.0f && !isInvestigatingShootingSound)
            {
                waitTime = 5.0f;
                NextPoint(agent);
            }

            return;
        }

        agentDirection = agent.transform.forward;
        playerDirection.Normalize();

        dotProduct = Vector3.Dot(playerDirection, agentDirection);

        if (dotProduct > 0.0f)
            agent.stateMachine.ChangeState(AIStateId.ChasePlayer);
    }

    void AIState.Exit(AIAgent agent)
    {
        
    }

    private void NextPoint(AIAgent agent)
    {
        waypointIndex = Random.Range(0, agent.waypoints.Length - 1);
        agent.navMeshAgent.SetDestination(agent.waypoints[waypointIndex].position);
    }
}
