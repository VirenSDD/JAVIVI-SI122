using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
  public HockeyEnvController envController;
  public Team team;

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Puck")
    {
      envController.ScoredGoal(team);
      if (GameManager.instance != null)
      {
        GameManager.instance.UpdateScore(team);
      }
    }
  }
}
