using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
  public int levelID;
  public HockeyEnvController envController;

  public void ChangeLevel()
  {
    SceneManager.LoadScene(levelID);
  }

  public void ResetScene()
  {
    if (envController != null)
    {
      envController.EndEpisode();
    }
  }
}
