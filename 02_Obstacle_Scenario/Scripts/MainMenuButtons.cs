using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public string SceneID;

    // Update is called once per frame
    public void ChangeLevel()
    {
        SceneManager.LoadScene(SceneID);  
    }
}
