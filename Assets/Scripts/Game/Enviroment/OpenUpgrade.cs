using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUpgrade : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ChangeUi(true));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChangeUi(false));
        }
    }

    private IEnumerator ChangeUi(bool open)
    {
        yield return new WaitForSeconds(0.2f);
        GameObject skillpointsObject = GameObject.Find("CanvasHUD").transform.GetChild(4).gameObject;
        if (skillpointsObject != null)
        {
            Skillpoints skillpoints = skillpointsObject.GetComponent<Skillpoints>();
            if (skillpoints != null)
            {
                if (open)
                {
                    skillpoints.open();
                }
                else
                {
                    skillpoints.close();
                }
            }
            else
            {
                Debug.LogWarning("Skillpoints script não encontrado no objeto Skillpoints");
            }
        }
        else
        {
            Debug.LogWarning("Objeto Skillpoints não encontrado na cena");
        }
    }
}