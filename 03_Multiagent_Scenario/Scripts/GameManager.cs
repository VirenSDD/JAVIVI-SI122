using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;
  public Text redScoreText;
  public Text blueScoreText;
  private int redScore = 0;
  private int blueScore = 0;

  private void Awake()
  {
    if (instance == null) instance = this;
    else if (instance != this) Destroy(this);
  }

  private void Start()
  {
    redScoreText.text = "0";
    blueScoreText.text = "0";
  }

  public void UpdateScore(Team scored)
  {
    if (scored == Team.Blue)
    {
      redScore++;
      redScoreText.text = redScore.ToString();
    }
    else
    {
      blueScore++;
      blueScoreText.text = blueScore.ToString();
    }
  }
}
