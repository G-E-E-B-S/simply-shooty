using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{

    [SerializeField] float minAlertDistance = 8f;
    [SerializeField] int health = 100;
    [SerializeField] int frenzyPoints = 4;
    [SerializeField] Coin coinPrefab;
    [SerializeField] int coinCount = 1;
    [SerializeField] float coinDropRate = 0.01f;
    [SerializeField] Image healthbar;
    [SerializeField] GameObject deathVFX;

    private Animator m_animator;
    private NavMeshAgent m_agent;
    private PlayerMovement m_player;
    private HUD m_hud;
    private int m_maxHealth;

    private void Start() {
        m_maxHealth = health;
        m_player = FindObjectOfType<PlayerMovement>();
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponentInChildren<Animator>();
        m_animator.SetBool("isEnemy", true);
    }

    private void Update() {
        if (m_player && Vector3.Distance(m_player.transform.position, transform.position) < minAlertDistance) {
            m_agent.SetDestination(m_player.transform.position);
            if (m_agent.remainingDistance > m_agent.stoppingDistance)
            {
                var dest = m_agent.desiredVelocity;
                m_animator.SetFloat("Blend", dest.x);
                m_animator.SetFloat("BlendSide", dest.z);
            }
        }
        else
        {
            m_animator.SetFloat("Blend", 0);
            m_animator.SetFloat("BlendSide", 0);
        }
    }

    public void DealDamage(int damage)
    {
        health -= damage;
        healthbar.fillAmount = (float)health/m_maxHealth;

        if (health <= 0) DestroySelf();
    }

    private void DestroySelf()
    {
        PlayDeathVFX();

        if (!m_hud) m_hud = FindObjectOfType<HUD>();
        m_hud.AddFrenzyPoints(frenzyPoints);

        bool canDropCoin = coinDropRate >= Random.Range(0f, 1f);
        if (canDropCoin) DropCoin();

        Destroy(gameObject);
    }

    private void PlayDeathVFX()
    {
        if (!deathVFX)
            return;

        GameObject vfxObj = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(vfxObj, 0.5f);
    }

    private void DropCoin()
    {
        for (int i = 0; i < coinCount; ++i)
        {
            Instantiate(coinPrefab, transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
        }
    }
}
