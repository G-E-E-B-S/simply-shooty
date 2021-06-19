using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] Weapon[] weapons;

    private Weapon m_spawnedWeapon;
    private float bobSpeed = 0.25f;
    private Vector2 bobHeight = new Vector2(1f, 1.2f);
    private bool bobUp = true;

    private void Start()
    {
        Weapon randWeaponPrefab = weapons[Random.Range(0, weapons.Length)];
        m_spawnedWeapon = Instantiate(randWeaponPrefab, transform.position, Quaternion.identity);
        m_spawnedWeapon.transform.parent = transform;
    }

    private void Update()
    {
        if (transform.position.y < bobHeight[1] && bobUp)
        {
            bobSpeed = Mathf.Abs(bobSpeed);
            transform.Translate(Vector3.up * bobSpeed * Time.deltaTime);
        }
        else if (transform.position.y > bobHeight[0])
        {
            bobUp = false;
            bobSpeed = -Mathf.Abs(bobSpeed);
            transform.Translate(Vector3.up * bobSpeed * Time.deltaTime);
        }
        else
        {
            bobUp = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerAttack>();

        if (player)
        {
            player.SetWeapon(m_spawnedWeapon.GetWeaponType());
            Destroy(gameObject);
        }
    }
}
