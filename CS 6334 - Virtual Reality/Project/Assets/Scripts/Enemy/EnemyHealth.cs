using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f;
    [SerializeField] private float currentHealth = 100.0f;
    [SerializeField] private float blinkIntensity = 10.0f;
    [SerializeField] private float blinkDuration = 0.1f;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private float blinkTimer;
    private AIAgent agent;
    private Rigidbody[] rigidbodies;
    private EnemyHealthBar enemyHealthBar;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<AIAgent>();
        currentHealth = maxHealth;

        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyHealthBar = GetComponentInChildren<EnemyHealthBar>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(var rigidbody in rigidbodies)
        {
            HitBox hitbox = rigidbody.gameObject.AddComponent<HitBox>();
            hitbox.enemyHealth = this;
        }
    }

    public void TakeDamage(float damageAmount, Vector3 direction)
    {
        currentHealth = Mathf.Max(0.0f, currentHealth - damageAmount);
        enemyHealthBar.SetHealthBarPercentage(currentHealth / maxHealth);
        agent.isHit = true;

        if (currentHealth <= 0.0f && !isDead)
            Die(direction);

        blinkTimer = blinkDuration;
    }

    public void Die(Vector3 direction)
    {
        isDead = true;

        agent.deathTransform = transform;
        AIDeathState deathState = agent.stateMachine.GetState(AIStateId.Death) as AIDeathState;
        deathState.direction = direction;
        agent.stateMachine.ChangeState(AIStateId.Death);
    }

    private void Update()
    {
        blinkTimer = blinkTimer - Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;
        skinnedMeshRenderer.material.color = Color.white * intensity;
    }
}
