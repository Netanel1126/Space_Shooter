using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Game,
    MainMenu,
    CoOpGame
}

public class SceneLoader : MonoBehaviour
{

    private static SceneLoader sharedInstance;

    public static SceneLoader SharedInstance
    {
        get
        {
            if (sharedInstance == null)
            {
                sharedInstance = new SceneLoader();
            }

            return sharedInstance;
        }

        private set
        {
            sharedInstance = value;
        }
    }

    public void Load(Scenes scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }


}
