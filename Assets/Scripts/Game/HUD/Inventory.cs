using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
  public GameObject slot1;
  public GameObject slot2;

  public void setSlots(Sprite slot1, Sprite slot2, bool swap)
  {
        Debug.Log(slot1);
        Debug.Log(slot2);
        setWeapon(this.slot1, slot1);
    setWeapon(this.slot2, slot2);

    activateSlot(swap ? this.slot2 : this.slot1);
    desactivateSlot(swap ? this.slot1 : this.slot2);
  }

  public void setWeapon(GameObject slot, Sprite sprite)
  {
        if (sprite != null)
        {
            slot.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            slot.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else
            slot.transform.GetChild(0).GetComponent<Image>().color = Color.clear;
  }

  public void activateSlot(GameObject slot)
  {
    slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    //slot.GetComponent<Image>().color = new Color(255/255f, 200/255f, 71/255f, 1);
  }

  public void desactivateSlot(GameObject slot)
  {
    slot.GetComponent<RectTransform>().localScale = new Vector3(0.8f, 0.8f, 0.8f);
    //slot.GetComponent<Image>().color = new Color(255/255f, 255/255f, 255/255f, 114/255f);
  }
}
