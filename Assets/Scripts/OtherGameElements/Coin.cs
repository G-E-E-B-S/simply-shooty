using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int value = 10;

    private float angularSpeed = 180f;

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * angularSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();

        if (player)
        {
            var hud = FindObjectOfType<HUD>();
            hud.AddCoins(value);

            Destroy(gameObject);
        }
    }
}
