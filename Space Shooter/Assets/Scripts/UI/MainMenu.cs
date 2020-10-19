using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneLoader.SharedInstance.Load(Scenes.Game);
    }

    public void StartNewCoOPGame()
    {
        SceneLoader.SharedInstance.Load(Scenes.CoOpGame);
    }
}
