using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupTypes
{
    TripleShot,
    Speed,
    Shields
}

public class Powerup : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private PowerupTypes powerupType;

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            player.ActivatePowerup(powerupType);
            Destroy(gameObject);
        }
    }
}
