using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallback : MonoBehaviour
{
    void EndDodge()
    {
        gameObject.transform.parent.GetComponent<PlayerObject>().EndDodge();
    }

    void EndDeath()
    {
        //gameObject.transform.parent.GetComponent<EnemyObject>().EndDeath();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
