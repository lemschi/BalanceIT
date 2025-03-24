using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevels : MonoBehaviour
{
    public void Anybutton ()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadSelectmount ()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadSeilGame ()
    {
        SceneManager.LoadScene(3);
    }
}
