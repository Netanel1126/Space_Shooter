using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float speed = 8;
    [SerializeField] private bool isEnemy;

    void Update()
    {
        if (isEnemy)
        {
            MoveDown();
        }
        else
        {
            MoveUp();
        }
    }

    private void MoveUp()
    {
        transform.Translate( Vector3.up * speed * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -8f)
        {
            if (transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        isEnemy = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player && isEnemy)
        {
            player.Damage();
            Destroy(gameObject);
        }
    }

}
