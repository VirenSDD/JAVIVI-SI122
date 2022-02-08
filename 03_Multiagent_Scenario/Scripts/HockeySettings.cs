using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HockeySettings : MonoBehaviour
{
  public static HockeySettings instance;
  public float agentRunSpeed = 5f;
  public float ballTouchReward = 1.0f;

  private void Awake()
  {
    if (instance == null) instance = this;
    else if (instance != this) Destroy(this);
  }
}
