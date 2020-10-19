using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject laserPrefub;
    [SerializeField] private GameObject tripleShot;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int lives = 3;
    [SerializeField] private GameObject shieldPrefub;
    [SerializeField] private GameObject[] engineDamageArray;
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip powerupSound;
    [SerializeField] private bool isPlayer2;

    private float canFire = -1;
    private float speedMultiplier = 2f;
    private SpawnManager spawnManager;
    private bool tripleShotActive = false;
    private bool isSpeedMultiplierActive = false;
    private bool isShieldActive = false;
    private AudioSource audioSource;
    private bool canDamage = true;

    private void Start()
    {
        if (!GameManager.SharedInstance.isCoOp)
            transform.position = Vector3.zero;

        spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        CalculateMovment();
        CheckWalls();

#if UNITY_ANDROID || UNITY_IPHONE
        if (Input.GetKey(KeyCode.Space) || CrossPlatformInputManager.GetButtonDown("Fire") && Time.time > canFire)
        {
            Shoot();
        }
#else
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)) && Time.time > canFire && !isPlayer2)
        {
            Shoot();
        }
        else if (Input.GetKey(KeyCode.RightShift) && isPlayer2 && Time.time > canFire)
        {
            Shoot();
        }
#endif
    }

    private void CalculateMovment()
    {
        float horizontal, vertical;

#if UNITY_ANDROID || UNITY_IPHONE
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        vertical = CrossPlatformInputManager.GetAxis("Vertical");
#else
        if (GameManager.SharedInstance.isCoOp)
        {
            if (!isPlayer2)
            {
                horizontal = Input.GetAxis("Player1Horizontal");
                vertical = Input.GetAxis("Player1Vertical");
            }
            else
            {
                horizontal = Input.GetAxis("Player2Horizontal");
                vertical = Input.GetAxis("Player2Vertical");
            }
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
        }
#endif

        transform.Translate(new Vector3(horizontal, vertical, 0) * speed * Time.deltaTime);
    }

    private void CheckWalls()
    {
        Vector3 newPos = transform.position;

        newPos.y = Mathf.Clamp(newPos.y, -3.8f, 0);

        if (newPos.x >= 11.3f || newPos.x <= -11.3f)
        {
            newPos.x *= -1;
        }

        transform.position = newPos;
    }

    private void Shoot()
    {
        Vector3 pos = this.transform.position;
        GameObject prefub;
        if (!tripleShotActive)
        {
            pos.y += 1.05f;
            prefub = this.laserPrefub;
        }
        else
        {
            prefub = this.tripleShot;
        }
        canFire = Time.time + fireRate;
        Instantiate(prefub, pos, Quaternion.identity);
        PlaySound(laserSound);
    }


    public void Damage()
    {
        if (!canDamage)
        {
            return;
        }

        if (!isShieldActive)
        {
            lives--;
            StartCoroutine(WasDamaged());
            UIManager.SharedInstance.OnHealthChanged(lives);
            if (lives < 1)
            {
                GameManager.SharedInstance.IsGameOver = true;
                spawnManager.OnPlayerDeath();
                PlaySound(explosionSound);
                Destroy(gameObject);
            }
            else
            {
                ActivateDamageIndicator();
            }
        }
        else
        {
            shieldPrefub.SetActive(false);
            isShieldActive = false;
        }
    }

    private void ActivateDamageIndicator()
    {
        GameObject prefub = null;
        do
        {
            prefub = engineDamageArray[UnityEngine.Random.Range(0, engineDamageArray.Length)];
        }
        while (prefub.activeSelf);
        prefub.SetActive(true);
    }

    public void ActivatePowerup(PowerupTypes powerup)
    {
        PlaySound(powerupSound);

        switch (powerup)
        {
            case PowerupTypes.TripleShot:
                TripleShotActive();
                break;
            case PowerupTypes.Speed:
                SpeedUP();
                break;
            case PowerupTypes.Shields:
                ActivateShield();
                break;
        }
    }

    private void TripleShotActive()
    {
        StopCoroutine("TripleShotPowerDownRoutine");
        StartCoroutine("TripleShotPowerDownRoutine");
    }

    private void SpeedUP()
    {
        StopCoroutine("SpeedUPRoutine");
        StartCoroutine("SpeedUPRoutine");
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        shieldPrefub.SetActive(true);
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        this.tripleShotActive = true;
        yield return new WaitForSeconds(5f);
        this.tripleShotActive = false;
    }

    private IEnumerator SpeedUPRoutine()
    {
        this.speed *= isSpeedMultiplierActive ? 1 : speedMultiplier;
        this.isSpeedMultiplierActive = true;
        yield return new WaitForSeconds(5f);
        this.speed /= speedMultiplier;
        this.isSpeedMultiplierActive = false;
    }

    private void PlaySound(AudioClip sound)
    {
        audioSource.clip = sound;
        audioSource.Play();
    }

    private IEnumerator WasDamaged()
    {
        canDamage = false;
        yield return new WaitForSeconds(0.5f);
        canDamage = true;
    }
}
