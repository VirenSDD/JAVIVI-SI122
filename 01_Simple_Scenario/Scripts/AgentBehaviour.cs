using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class AgentBehaviour : Agent
{
  [Header("Agent Settings")]
  public float moveSpeed = 1.0f;

  [Header("Environment Settings")]
  [SerializeField] private Transform rewardTransform;
  [SerializeField] private Material winMaterial;
  [SerializeField] private Material loseMaterial;
  [SerializeField] private MeshRenderer floorMeshRenderer;

  public override void OnEpisodeBegin()
  {
    transform.localPosition = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-7f, -3.5f));
    rewardTransform.localPosition = new Vector3(Random.Range(-5f, 5f), rewardTransform.localPosition.y, Random.Range(2f, 7f));
  }

  public override void CollectObservations(VectorSensor sensor)
  {
    sensor.AddObservation(transform.localPosition);
    sensor.AddObservation(rewardTransform.localPosition);
  }

  public override void OnActionReceived(ActionBuffers actions)
  {
    float moveX = actions.ContinuousActions[0];
    float moveZ = actions.ContinuousActions[1];
    transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
  }

  public override void Heuristic(in ActionBuffers actionsOut)
  {
    ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
    continuousActions[0] = -Input.GetAxisRaw("Vertical");
    continuousActions[1] = Input.GetAxisRaw("Horizontal");
  }

  private void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Reward")
    {
      SetReward(1f);
      floorMeshRenderer.material = winMaterial;
      EndEpisode();
    }
    else if (other.gameObject.tag == "Wall")
    {
      SetReward(-1f);
      floorMeshRenderer.material = loseMaterial;
      EndEpisode();
    }
  }
}
