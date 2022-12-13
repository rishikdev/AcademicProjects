using System;
using UnityEngine;
using UnityEngine.AI;

public class AIChasePlayerState : AIState
{
    private float fireRate = 10;
    private float nextTimeToFire = 0f;
    private Ray ray;
    private RaycastHit hitInfo;
    private bool haltFire;
    private float haltFireTimer = 2.0f;

    private Vector3 direction;
    private float timer = 0.0f;

    public AIStateId GetId()
    {
        return AIStateId.ChasePlayer;
    }

    public void Enter(AIAgent agent)
    {
        
    }

    public void Update(AIAgent agent)
    {
        if (!agent.enabled)
            return;

        timer = timer - Time.deltaTime;
        haltFireTimer = haltFireTimer - Time.deltaTime;

        if (!agent.navMeshAgent.hasPath)
            agent.navMeshAgent.destination = agent.playerTransform.position;

        if (timer < 0.0f)
        {
            direction = (agent.playerTransform.position - agent.navMeshAgent.destination);
            direction.y = 0;

            if (direction.sqrMagnitude > agent.config.minDistance * agent.config.minDistance && agent.navMeshAgent.pathStatus != NavMeshPathStatus.PathPartial)
                agent.navMeshAgent.destination = agent.playerTransform.position;

            timer = agent.config.maxTime;
        }

        if(agent.sensor.IsInSight(agent.playerTransform.gameObject) && !agent.shootingBuilding.isPlayerDead)
        {
            var lookPos = agent.playerTransform.position - agent.transform.position;
            lookPos.y = 0;
            var desiredRotation = Quaternion.LookRotation(lookPos);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, desiredRotation, 0.5f);

            agent.weaponIK.targetTransform = agent.playerTransform;

            if (Time.time >= nextTimeToFire)
            {
                if (haltFireTimer <= 0.0f)
                {
                    haltFireTimer = 2.0f;
                    haltFire = !haltFire;
                }

                nextTimeToFire = Time.time + 1f / fireRate;

                if (!haltFire)
                    Fire(agent);
            }
        }

        if (agent.shootingBuilding.isPlayerDead)
        {
            agent.isHit = false;
            agent.stateMachine.ChangeState(AIStateId.Idle);
        }
    }

    public void Exit(AIAgent agent)
    {

    }

    private void Fire(AIAgent agent)
    {
        agent.audioSource.Play();
        agent.muzzleFlash.Emit(1);

        ray.origin = agent.raycastOrigin.position;
        ray.direction = agent.raycastOrigin.forward;

        var tracer = MonoBehaviour.Instantiate(agent.bulletTracer, ray.origin, Quaternion.identity);
        tracer.AddPosition(ray.origin);

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.transform.gameObject.layer != Properties.PLAYER_LAYER)
            {
                agent.hitEffect.transform.position = hitInfo.point;
                agent.hitEffect.transform.forward = hitInfo.normal;
                agent.hitEffect.Emit(1);
            }

            tracer.transform.position = hitInfo.point;

            if (hitInfo.transform.gameObject.layer == Properties.PLAYER_LAYER)
                agent.playerHealth.TakeDamage(10);
        }
    }
}
