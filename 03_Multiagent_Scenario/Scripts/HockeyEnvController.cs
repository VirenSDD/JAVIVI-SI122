using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class HockeyEnvController : MonoBehaviour
{
  [System.Serializable]
  public class PlayerInfo
  {
    public PadAgent agent;
    [HideInInspector]
    public Vector3 startingPos;
    [HideInInspector]
    public Rigidbody rb;
  }

  public int MaxEnvironmentSteps = 25000;
  public GameObject puck;
  public PlayerInfo[] agentsList;

  private SimpleMultiAgentGroup blueTeam;
  private SimpleMultiAgentGroup redTeam;
  private Rigidbody puckRb;
  private Vector3 puckStartingPosition;
  private int resetTimer;

  private void Start()
  {
    blueTeam = new SimpleMultiAgentGroup();
    redTeam = new SimpleMultiAgentGroup();
    puckRb = puck.GetComponent<Rigidbody>();
    puckStartingPosition = puck.transform.localPosition;
    foreach (var item in agentsList)
    {
      item.startingPos = item.agent.transform.localPosition;
      item.rb = item.agent.GetComponent<Rigidbody>();
      if (item.agent.team == Team.Blue)
      {
        blueTeam.RegisterAgent(item.agent);
      }
      else
      {
        redTeam.RegisterAgent(item.agent);
      }
    }
    ResetScene();
  }

  private void FixedUpdate()
  {
    resetTimer += 1;
    if (resetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
    {
      EndEpisode();
    }
  }

  public void ScoredGoal(Team scoredTeam)
  {
    if (scoredTeam == Team.Blue)
    {
      blueTeam.AddGroupReward(-1);
      redTeam.AddGroupReward(1 - (float) resetTimer / MaxEnvironmentSteps);
    }
    else
    {
      redTeam.AddGroupReward(-1);
      blueTeam.AddGroupReward(1 - (float)resetTimer / MaxEnvironmentSteps);
    }
    blueTeam.EndGroupEpisode();
    redTeam.EndGroupEpisode();
    ResetScene();
  }

  public void ResetScene() 
  {
    resetTimer = 0;
    foreach (var item in agentsList)
    {
      item.agent.transform.localPosition = item.startingPos;
      item.rb.velocity = Vector3.zero;
    }
    puck.transform.localPosition = puckStartingPosition;
    puckRb.velocity = Vector3.zero;
  }

  public void EndEpisode()
  {
    blueTeam.GroupEpisodeInterrupted();
    redTeam.GroupEpisodeInterrupted();
    ResetScene();
  }
}
