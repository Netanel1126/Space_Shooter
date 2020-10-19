using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private float enemyWaitTime = 5f;
    [SerializeField] private float powerupWaitTimeMin = 3f;
    [SerializeField] private float powerupWaitTimeMax = 7f;
    [SerializeField] private GameObject enemyPrefub;
    [SerializeField] private GameObject[] powerupsPrefub;
    [SerializeField] private GameObject enemyContainer;

    private bool stopSpawn;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!stopSpawn)
        {
            this.Spawn(enemyPrefub, enemyContainer);
            yield return new WaitForSeconds(enemyWaitTime);
        }
    }

    private IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!stopSpawn)
        {
            int index = Random.Range(0, powerupsPrefub.Length);
            this.Spawn(powerupsPrefub[index], null);
            yield return new WaitForSeconds(Random.Range(powerupWaitTimeMin, powerupWaitTimeMax));
        }
    }

    private void Spawn(GameObject prefub, GameObject parantContainer)
    {
        Vector3 pos = new Vector3(Random.Range(-8f, 8f), 7, 0);
        GameObject gameObject = Instantiate(prefub, pos, Quaternion.identity);

        if (parantContainer)
            gameObject.transform.parent = parantContainer.transform;
    }

    public void OnPlayerDeath()
    {
        stopSpawn = true;
    }
}
