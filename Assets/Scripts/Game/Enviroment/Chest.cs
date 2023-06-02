using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool oppened;
    private GameObject smoke;
    void Awake()
    {
        oppened = false;
        smoke = gameObject.transform.GetChild(2).gameObject.transform.GetChild(0).gameObject;
        transform.GetChild(1).gameObject.SetActive(false);
    }
    public void Open(int luck)
    {
        if(oppened)
        {
            return;
        }
        oppened = true;
        StartCoroutine(DropItems(luck));
    }

    private IEnumerator DropItems(int luck)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        smoke.GetComponent<Animator>().SetTrigger("Run");
        // int level = GameObject.FindObjectOfType<RogueLikeController>().GetActualLevel();
        int level = 1;
        if (oppened)
        {
            yield return new WaitForSeconds(1f);
        }
        List<MonoBehaviour> items = DropGenerator.GenerateDrop(luck, level);
        float pos = 0.2f;
        foreach (MonoBehaviour item in items)
        {
            item.gameObject.SetActive(true);
            item.gameObject.transform.position = new Vector3(gameObject.transform.position.x + pos, gameObject.transform.position.y - 0.1f, gameObject.transform.position.z);
            pos += 0.2f;
        }
    }
}
