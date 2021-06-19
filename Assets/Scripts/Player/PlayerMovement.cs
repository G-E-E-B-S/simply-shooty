using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public delegate void Callback();

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    [SerializeField] GameObject deathVFX;

    private NavMeshAgent m_agent;
    private DynamicJoystick m_joystick;
    private Animator m_animator;

    private Vector3 m_orgRotation;
    private Vector3 m_lookTarget;
    private bool stopInput = false;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_animator = GetComponentInChildren<Animator>();
        m_orgRotation = transform.eulerAngles;
        m_lookTarget = Vector3.zero;
        ResetLookTarget();
    }

    private void Update()
    {
        if (stopInput)
        {
            SetAnimState(0, 0);
            return;
        }

        TryStep();
    }

    private void TryStep()
    {
        if (!m_joystick)
        {
            m_joystick = FindObjectOfType<DynamicJoystick>();
            return;
        }

        Vector3 move = new Vector3(m_joystick.Vertical, 0, -m_joystick.Horizontal);

        if (move == Vector3.zero)
        {
            SetAnimState(0, 0);
            return;
        }

        Step(move);
    }

    private void Step(Vector3 movePos)
    {
        m_agent.Move(movePos * Time.deltaTime * speed);
        SetAnimState(movePos.x, movePos.z);
        if (m_lookTarget != Vector3.zero) transform.LookAt(m_lookTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyController>();
        if (enemy)
        {
            stopInput = true;
            m_agent.isStopped = true;
            PlayDeathVFX();
            FindObjectOfType<GameEndHandler>().DeclareLoss();
            Destroy(gameObject);
        }
    }

    private void PlayDeathVFX()
    {
        if (!deathVFX)
            return;

        GameObject vfxObj = Instantiate(deathVFX, transform.position, transform.rotation);
        Destroy(vfxObj, 1.5f);
    }

    private void SetAnimState(float x, float z)
    {
        m_animator.SetFloat("Blend", x);
        m_animator.SetFloat("BlendSide", z);
    }

    public void SetLookTarget(Vector3 target)
    {
        m_lookTarget = target;
    }

    public void ResetLookTarget()
    {
        m_lookTarget = Vector3.zero;
        transform.eulerAngles = m_orgRotation;
    }

    public void PlayVictorySequence(Callback callback)
    {
        stopInput = true;
        m_agent.isStopped = true;
        GetComponent<PlayerAttack>().UnsetWeapon();
        transform.LookAt(Vector3.left);
        m_animator.SetBool("isGameEnding", true);
        StartCoroutine(CallCallback(callback));
    }

    private IEnumerator CallCallback(Callback callback)
    {
        yield return new WaitForSeconds(5f);
        callback();
    }
}
