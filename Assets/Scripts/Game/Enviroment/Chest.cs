using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool oppened;
    private GameObject chestItem;
    void Awake()
    {
        oppened = false;
        chestItem = gameObject.transform.GetChild(0).gameObject;
    }

    void OpenChest()
    {
        oppened = true;
        //iterar sobre cada estado do bau child 0 e 1 e mudar os frames
        chestItem.transform.GetChild(0).gameObject.SetActive(false);
        chestItem.transform.GetChild(1).gameObject.SetActive(true);
    }

    IEnumerator DropItems()
    {
        if(oppened)
        {
            yield return new WaitForSeconds(1f);
        }
        //todo: dropar itens
    }
}
