using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevels : MonoBehaviour
{
    public void Anybutton ()
    {
        Debug.Log("Load Main Menu");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(1);
    }

    public void LoadSelectmount ()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(2);
    }

    public void LoadSeilGame ()
    {
        Debug.Log("Load Main SeilGame");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(3);
    }
    public void LoadBallGame()
    {
        Debug.Log("Load Main BallGame");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(4);
    }

}
