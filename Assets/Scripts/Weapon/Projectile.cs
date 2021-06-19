using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform projectileBody;
    [SerializeField] LayerMask destroyable;
    [SerializeField] int damage = 50;
    [SerializeField] GameObject hitVFX;
    private float m_speed = 0f;
    private float m_aoe = 0f;
    private Vector3 m_target = Vector3.zero;

    public void SetSpeed(float speed)
    {
        m_speed = speed;
    }

    public void SetAOE(float aoe)
    {
        m_aoe = aoe;
    }

    public void SetTarget(Vector3 target)
    {
        m_target = transform.InverseTransformPoint(target);

        float angle = Vector3.SignedAngle(transform.position, m_target, Vector3.right);
        projectileBody.eulerAngles += new Vector3(0, -angle, 0);
    }

    private void Update()
    {
        if (m_target == Vector3.zero) return;

        m_target.y = 0;
        transform.Translate(m_target * m_speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy"){
            return;
        }
        else
        {
            PlayHitVFX();
            Destroy(gameObject);
        }

        if (m_aoe > 0) TryAOE(transform.position);
        else Projectile.DestroyEnemy(other, damage);
    }

    private void TryAOE(Vector3 origin)
    {
        Collider[] colliders = Physics.OverlapSphere(origin, m_aoe, destroyable);
        var instanceIDs = new List<int>();
        foreach(Collider collider in colliders)
        {
            if (instanceIDs.IndexOf(collider.gameObject.GetInstanceID()) != -1)
                continue;

            instanceIDs.Add(collider.gameObject.GetInstanceID());
            Projectile.DestroyEnemy(collider, damage);
        }
    }

    public static void DestroyEnemy(Collider obj, int damage)
    {
        var enemy = obj.GetComponent<EnemyController>();
        if(enemy)
        {
            enemy.DealDamage(damage);
        }

        var barrel = obj.GetComponent<Barrel>();
        if (barrel)
        {
            barrel.Explode();
        }
    }

    private void PlayHitVFX()
    {
        if (!hitVFX)
            return;

        GameObject vfxObj = Instantiate(hitVFX, transform.position, transform.rotation);
        Destroy(vfxObj, 0.5f);
    }
}
