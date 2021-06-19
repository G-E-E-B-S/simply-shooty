using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float maxShootingDistance = 10f;
    [SerializeField] GameObject pistolRef;
    [SerializeField] GameObject machinegunRef;
    [SerializeField] GameObject shotgunRef;
    [SerializeField] GameObject launcherRef;

    private Animator m_animator;
    private PlayerMovement m_player;
    private Weapon m_equippedWeapon;
    private float m_rofModifier;
    private bool m_canShoot;

    private void Start()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_player = GetComponent<PlayerMovement>();
        m_rofModifier = 1;
        m_canShoot = true;
    }

    private void Update() {
        if (!m_equippedWeapon || !m_canShoot) return;

        m_canShoot = false;
        StartCoroutine(EnableShooting());

        GameObject[] allEnemies= GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies != null && allEnemies.Length > 0)
        {
            GameObject target = allEnemies[0];
            foreach (GameObject tmpTarget in allEnemies)
            {
                if (Vector2.Distance(transform.position, tmpTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                {
                    target = tmpTarget;
                }
            }
            if (Vector2.Distance(transform.position, target.transform.position) < maxShootingDistance)
            {
                m_animator.SetBool("canShoot", true);
                m_player.SetLookTarget(target.transform.position);
                m_equippedWeapon.ShootTarget(target);
            }
            else {
                m_animator.SetBool("canShoot", false);
                m_player.ResetLookTarget();
            }
        }
        else
        {
            m_animator.SetBool("canShoot", false);
            m_player.ResetLookTarget();
        }
    }

    public void SetWeapon(WeaponType type)
    {
        UnsetWeapon();
        switch(type)
        {
            case WeaponType.Pistol:
                pistolRef.SetActive(true);
                m_equippedWeapon = pistolRef.GetComponent<Weapon>();
                m_animator.SetTrigger("pistolAcquired");
                break;
            case WeaponType.MachineGun:
                machinegunRef.SetActive(true);
                m_equippedWeapon = machinegunRef.GetComponent<Weapon>();
                m_animator.SetTrigger("mgAcquired");
                break;
            case WeaponType.Shotgun:
                shotgunRef.SetActive(true);
                m_equippedWeapon = shotgunRef.GetComponent<Weapon>();
                m_animator.SetTrigger("shotgunAcquired");
                break;
            case WeaponType.MissileLauncher:
                launcherRef.SetActive(true);
                m_equippedWeapon = launcherRef.GetComponent<Weapon>();
                m_animator.SetTrigger("launcherAcquired");
                break;
        }
    }

    public void UnsetWeapon()
    {
        pistolRef.SetActive(false);
        machinegunRef.SetActive(false);
        shotgunRef.SetActive(false);
        launcherRef.SetActive(false);
    }

    public void SetROFModifier(float modifier)
    {
        m_rofModifier = modifier;
    }

    private IEnumerator EnableShooting()
    {
        var rof = m_equippedWeapon.GetRateOfFire() / m_rofModifier;
        yield return new WaitForSeconds(rof);
        m_canShoot = true;
    }
}
