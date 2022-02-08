using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class BirdAgent : Agent
{
  private Bird bird;
  public Level level;

  private bool isKeyDown;

  public void Awake() {
    bird = GetComponent<Bird>();
  }

  public void Start() {
    Bird.GetInstance().OnDied += flappyDied;
    Level.GetInstance().eventPipePassed += PassedAPipe;
    isKeyDown = false;
  }

  private void Update() {
    if (Input.GetMouseButtonDown(0)) {
      isKeyDown = true;
    }
  }

  public override void OnEpisodeBegin()
  {
      bird.Reset();
      level.Reset();
  }

  private void flappyDied(object sender, System.EventArgs e) {
      EndEpisode();
  }

  private void PassedAPipe() {
      AddReward(1f);
  }

  public override void OnActionReceived(ActionBuffers actions) {
    if (actions.DiscreteActions[0] == 1) {
      bird.Jump();
    }
    AddReward(.01f);
  }

  public override void Heuristic(in ActionBuffers actionsOut) {
    ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
    discreteActions[0] = isKeyDown ? 1 : 0;
    isKeyDown = false;
  }

  public override void CollectObservations(VectorSensor sensor) {
    float worldHeight = 100f;
    float birdNormalizedY = (bird.transform.position.y + (worldHeight / 2f)) / worldHeight;
    sensor.AddObservation(birdNormalizedY);

    float pipeSpawnXposition = 100f;
    Level.Pipe nextPipe =  level.GetNextPipe();
    if (nextPipe != null && nextPipe.pipeBodyTransform != null) {
        sensor.AddObservation(nextPipe.GetXPosition() / pipeSpawnXposition);
    } else {
      sensor.AddObservation(1f);
    }

    sensor.AddObservation(bird.GetVelocityY() / 200f);
  }
}