using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenUpgrade : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Cheirinho");
                StartCoroutine(OpenUi());
            }
        }
    
    private IEnumerator OpenUi()
    {
        yield return new WaitForSeconds(1f);
        GameObject.FindObjectOfType<Skillpoints>().open();
    }
}
