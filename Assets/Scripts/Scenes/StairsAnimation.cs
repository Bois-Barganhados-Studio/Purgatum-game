using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsAnimation : MonoBehaviour
{
    public GameObject stairs;
    private float offsetX = -100.0f;
    private float offsetY = -152.0f;
    private float movementSpeed = 50.0f;
    private int count = 0;

    private Vector3 originalPosition;
    private Vector3 lastOriginalPosition;

    public void Start()
    {
        StartAnimation();
    }

    public void StartAnimation() 
    {
        if (count == 0)
        {
            lastOriginalPosition = stairs.transform.position;
            count++;
        } else{
            stairs.transform.position = lastOriginalPosition;
        }
        originalPosition = stairs.transform.position;
        StartCoroutine(LoopMovement());
    }

    private IEnumerator LoopMovement()
    {
        while (true)
        {
            Vector3 targetPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            yield return MoveObject(targetPosition);
            stairs.transform.position = originalPosition;
        }
    }

    private IEnumerator MoveObject(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(stairs.transform.position, targetPosition);
        float duration = distance / movementSpeed;
        float startTime = Time.time;
        Vector3 startPosition = stairs.transform.position;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            stairs.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        stairs.transform.position = targetPosition;
    }
}
