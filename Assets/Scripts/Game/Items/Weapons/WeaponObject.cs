using System.Collections;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public Weapon weapon;
    public SpriteRenderer sRenderer;
    private Sprite[] vfxSprites;
    public Sprite[] VfxSprites { get { return vfxSprites; } }

    public void Awake()
    {
        if (sRenderer == null)
            sRenderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void Init(Weapon w, Sprite sprite, Sprite[] vfxSprites, bool active)
    {
        weapon = w;
        sRenderer.sprite = sprite;
        this.vfxSprites = vfxSprites;
        gameObject.SetActive(active);
        sRenderer.color = Color.white;
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public Weapon.TYPE GetWeaponType()
    {
        return weapon.Type;
    }

    private bool dropCancelled { get; set; }

    public void CancelDrop()
    {
        dropCancelled = true;
    }

    public void Drop(int seconds)
    {
        dropCancelled = false;
        StartCoroutine(DropCountDown(seconds));
    }

    private IEnumerator DropCountDown(int seconds)
    {

        if (seconds > 6)
            yield return new WaitForSeconds(seconds - 6);
        var oneSec = new WaitForSeconds(1);
        for (int i = 0; i < 6; i++)
        {
            sRenderer.enabled = !sRenderer.enabled;
            yield return oneSec;
        }
        if (!dropCancelled)
            Destroy(gameObject);
    }

    public Sprite getWSprite()
    {
        Debug.Log("at get getWS - sRenderer: " + sRenderer);
        return sRenderer.sprite;
    }

    public void SetSpriteColor(Color newColor)
    {
        sRenderer.color = newColor;
    }
}