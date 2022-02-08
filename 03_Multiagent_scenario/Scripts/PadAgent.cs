using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public enum Team
{
  Blue = 0,
  Red = 1
}

public class PadAgent : Agent
{
  [Header("Settings")]
  public float lateralSpeed = 1.0f;
  public float forwardSpeed = 1.0f;
  private float kickPower = 0f;
  public float touchPower = 50f;

  [HideInInspector]
  public Team team;
  private float existential;
  private BehaviorParameters behaviorParams;
  private Rigidbody agentRb;

  public override void Initialize()
  {
    HockeyEnvController envController = GetComponentInParent<HockeyEnvController>();
    if (envController == null)
    {
      Debug.LogError("Hockey Environment Controller not found in parent!");
    }
    existential = 1f / envController.MaxEnvironmentSteps;
    behaviorParams = GetComponent<BehaviorParameters>();
    if (behaviorParams.TeamId == (int) Team.Blue)
    {
      team = Team.Blue;
    }
    else
    {
      team = Team.Red;
    }
    agentRb = GetComponent<Rigidbody>();
  }

  public override void OnActionReceived(ActionBuffers actions)
  {
    AddReward(-existential);
    Vector3 dir = Vector3.zero;
    ActionSegment<int> act = actions.DiscreteActions;
    int forwardAxis = act[0];
    int lateralAxis = act[1];
    kickPower = 0f;
    
    switch (forwardAxis)
    {
      case 1:
        dir = transform.right * forwardSpeed;
        kickPower = 1f;
        break;
      case 2:
        dir = transform.right * -forwardSpeed;
        break;
    }
    switch (lateralAxis)
    {
      case 1:
        dir += transform.up * lateralSpeed;
        break;
      case 2:
        dir += transform.up * -lateralSpeed;
        break;
    }
    agentRb.AddForce(dir * HockeySettings.instance.agentRunSpeed, ForceMode.VelocityChange);
  }

  public override void Heuristic(in ActionBuffers actionsOut)
  {
    ActionSegment<int> act = actionsOut.DiscreteActions;
    if (Input.GetKey(KeyCode.W))
    {
      act[0] = 1;
    }
    else if (Input.GetKey(KeyCode.S))
    {
      act[0] = 2;
    }
    
    if (Input.GetKey(KeyCode.D))
    {
      act[1] = 1;
    }
    else if (Input.GetKey(KeyCode.A))
    {
      act[1] = 2;
    }
  }

  private void OnCollisionEnter(Collision other)
  {
    float force = touchPower * kickPower;
    if (other.gameObject.tag == "Puck")
    {
      AddReward(0.2f * HockeySettings.instance.ballTouchReward);
      Vector3 dir = other.contacts[0].point - transform.position;
      dir = dir.normalized;
      other.gameObject.GetComponent<Rigidbody>().AddForce(dir * force);
    }
  }
}
