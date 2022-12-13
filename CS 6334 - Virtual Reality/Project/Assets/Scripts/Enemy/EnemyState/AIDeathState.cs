using UnityEngine;

public class AIDeathState : AIState
{
    public Vector3 direction;
    private float timer = 5.0f;

    AIStateId AIState.GetId()
    {
        return AIStateId.Death;
    }

    void AIState.Enter(AIAgent agent)
    {
        agent.navMeshAgent.SetDestination(agent.deathTransform.position);

        agent.ragdoll.ActivateRagdoll();
        direction.y = 1;
        agent.ragdoll.ApplyForce(direction * agent.config.dieForce);

        agent.healthBar.gameObject.SetActive(false);
        agent.skinnedMeshRenderer.updateWhenOffscreen = true;
        agent.weaponIK.enabled = false;

        agent.shootingBuilding.UpdateCurrentEnemyCount(-1);
    }

    void AIState.Exit(AIAgent agent)
    {
        
    }

    void AIState.Update(AIAgent agent)
    {

        timer = timer - Time.deltaTime;
        if (timer <= 0.0f)
        {
            agent.shootingBuilding.InstantiateOneEnemy();
            MonoBehaviour.Destroy(agent.gameObject);
        }
    }
}
