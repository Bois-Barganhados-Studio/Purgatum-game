using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Destructable : MonoBehaviour
{
    private GameObject smoke, item;

    void Awake()
    {
        item = transform.GetChild(0).gameObject;
        smoke = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
    }

    public void DestroyObject()
    {
        item.SetActive(false);
        StartCoroutine(WaitForAnimation());
    }

    private IEnumerator WaitForAnimation()
    {
        smoke.GetComponent<Animator>().SetTrigger("Run");
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}