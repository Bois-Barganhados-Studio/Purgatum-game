using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsAnimation : MonoBehaviour
{
    public float offsetX = 1.0f;
    public float offsetY = 1.0f;
    public float movementSpeed = 1.0f;

    private Vector3 originalPosition;

    private void Start()
    {

        originalPosition = transform.position;
        StartCoroutine(LoopMovement());
    }

    private IEnumerator LoopMovement()
    {
        while (true)
        {
            Vector3 targetPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            yield return MoveObject(targetPosition);
            transform.position = originalPosition;
        }
    }

    private IEnumerator MoveObject(Vector3 targetPosition)
    {
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / movementSpeed;
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
