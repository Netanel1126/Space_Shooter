using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject laserPrefub;

    private Animator animator;
    private AudioSource audioSource;
    private bool isShooting = false;
    private float fireRate = 3f;
    private float canFire = -1;
    private bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        Move();
        canFire += Time.deltaTime;
        if(canFire >= fireRate && !isDead)
        {
            canFire = 0;
            fireRate = Random.Range(3f, 7f);
            Shoot();
        }

    }

    private void Move()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -5f)
        {
            float x = Random.Range(-8f, 8f);
            transform.position = new Vector3(x, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        if (player || other.tag == "Laser")
        {
            if (player)
            {
                player.Damage();
            }
            else
            { 
                GameManager.SharedInstance.AddScore(10);
                Destroy(other.gameObject);
            }
            Die();
        }
    }

    private void Die()
    {
        speed = 0;
        isDead = true;
        audioSource.clip = explosionSound;
        audioSource.Play();
        animator.SetTrigger("OnEnemyDeath");
        Destroy(gameObject, 2.8f);
    }

    private void Shoot()
    {
        isShooting = true;
        GameObject laser = Instantiate(laserPrefub, this.transform.position, Quaternion.identity);
        Laser[] lasers = laser.GetComponentsInChildren<Laser>();
        foreach (Laser l in lasers)
        {
            l.AssignEnemyLaser();
        }
        isShooting = false;
    }
}
