using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
  public void turnOn()
  {
    gameObject.transform.GetChild(1).gameObject.SetActive(true);
  }

  public void turnOff()
  {
    gameObject.transform.GetChild(1).gameObject.SetActive(false);
  }
}
