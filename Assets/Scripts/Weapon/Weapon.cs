using UnityEngine;

public enum WeaponType { Pistol, MachineGun, Shotgun, MissileLauncher }

public class Weapon : MonoBehaviour
{

    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform[] projectileSpawn;
    [SerializeField] float projectileSpeed = 1f;
    [SerializeField] float areaOfEffect = 0f;
    [SerializeField] float rateOfFire = 2;
    public WeaponType weaponType;

    private GameObject m_target;
    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
    }

    public WeaponType GetWeaponType()
    {
        return weaponType;
    }

    public float GetRateOfFire() {
        return rateOfFire;
    }

    public void ShootTarget(GameObject target)
    {
        m_target = target;
        Fire();
    }

    private void Fire()
    {
        if(m_target == null) {
            return;
        }

        m_animator.SetTrigger("shoot");
        foreach(Transform spawnPoint in projectileSpawn)
        {
            Projectile projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
            projectile.SetTarget(m_target.transform.position);
            projectile.SetSpeed(projectileSpeed);
            projectile.SetAOE(areaOfEffect);
        }
    }
}
