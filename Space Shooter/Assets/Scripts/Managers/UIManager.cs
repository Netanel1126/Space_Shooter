using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text restartText;
    [SerializeField] private Image livesImage;
    [SerializeField] private Sprite[] livesSprits;
    [SerializeField] private Text highScoreText;

    public static UIManager SharedInstance { get; private set; }

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        livesImage.sprite = livesSprits[3];
        gameOverText.gameObject.SetActive(false);
    }

    private void Update()
    {
        score.text = "Score: " + GameManager.SharedInstance.Score;
        highScoreText.text = "Best: " + GameManager.SharedInstance.GetHighScore();
        restartText.gameObject.SetActive(GameManager.SharedInstance.IsGameOver);
    }

    public void OnHealthChanged(int health)
    {
        livesImage.sprite = livesSprits[health];
        if (health <= 0)
        {
            gameOverText.gameObject.SetActive(true);
            StartCoroutine(GameOverFlicker());
        }
    }

    private IEnumerator GameOverFlicker()
    {
        gameOverText.text = "GAME OVER";
        yield return new WaitForSeconds(0.5f);
        gameOverText.text = "";
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(GameOverFlicker());
    }
}
