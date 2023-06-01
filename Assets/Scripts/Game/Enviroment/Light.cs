using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    private bool turnedOn = false;

    public void TurnThisLigth()
    {
        turnedOn = !turnedOn;
        StartCoroutine(TurnLigths());
    }

    private IEnumerator TurnLigths()
    {
        yield return new WaitForSeconds(1f);
        gameObject.transform.GetChild(0).gameObject.SetActive(turnedOn);
    }
}