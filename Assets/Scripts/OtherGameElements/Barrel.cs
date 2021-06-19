using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] int damage = 50;
    [SerializeField] float areaOfEffect = 3f;
    [SerializeField] float explosionDelay = 0.1f;
    [SerializeField] GameObject explosionVFX;
    [SerializeField] LayerMask destroyable;


    public void Explode()
    {
        Invoke("Explosion", explosionDelay);
    }

    private void Explosion()
    {
        Destroy(gameObject);
        PlayExplosionVFX();

        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffect, destroyable);
        var instanceIDs = new List<int>();
        foreach(Collider collider in colliders)
        {
            if (instanceIDs.IndexOf(collider.gameObject.GetInstanceID()) != -1)
                continue;

            instanceIDs.Add(collider.gameObject.GetInstanceID());
            Projectile.DestroyEnemy(collider, damage);
        }
    }

    private void PlayExplosionVFX()
    {
        if (!explosionVFX)
            return;

        GameObject vfxObj = Instantiate(explosionVFX, transform.position, transform.rotation);
        Destroy(vfxObj, 1f);
    }
}
