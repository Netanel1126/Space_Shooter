using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject explosionPrefub;

    private SpawnManager spawnManager;

    private void Start()
    {
        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            _ = Instantiate(explosionPrefub, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            spawnManager.StartSpawning();
            Destroy(gameObject,0.25f);
        }
    }

}