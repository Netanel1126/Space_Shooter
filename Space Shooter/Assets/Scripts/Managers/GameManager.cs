using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool isCoOp;

    public int Score { private set; get; }
    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
            if (Score >= highScore)
            {
                PlayerPrefs.SetInt("highscore", highScore);
            }
        }
    }

    private int highScore;
    private bool isGameOver;

    public static GameManager SharedInstance { get; private set; }

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        Score = 0;
        highScore = PlayerPrefs.GetInt("highscore");
    }

    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneLoader.SharedInstance.Load(isCoOp ? Scenes.CoOpGame : Scenes.Game);
        }
        else if (IsGameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.SharedInstance.Load(Scenes.MainMenu);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void AddScore(int num)
    {
        Score += num;
        if(Score > highScore)
        {
            highScore = Score;
        }
    }

    public int GetHighScore()
    {
        return highScore;
    }
}
