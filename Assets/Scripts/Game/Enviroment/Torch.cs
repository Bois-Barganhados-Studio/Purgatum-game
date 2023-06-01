using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
  private float timer = 1.2f;
  private bool state = false;
  public void TurnTorch(bool state){
    this.state = state;
    StartCoroutine(SetTorch(state));
  }
  private IEnumerator SetTorch(bool state){
    yield return new WaitForSeconds(timer);
    gameObject.transform.GetChild(1).gameObject.SetActive(state);
  }
  
}
