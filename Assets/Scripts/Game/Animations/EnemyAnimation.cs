using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void die(Vector2 _direction)
    {
        string[] animDirection = { "Dying N", "Dying NW", "Dying W", "Dying SW", "Dying S", "Dying SE", "Dying E", "Dying NE" };
        anim.Play(animDirection[DirectionToIndex(_direction)]);
    }

    //MARKER Converts a Vector2 direction to an index to a slcie around a circle
    //CORE this goes in a counter-clock direction
    // TODO - Animator class hiararchy
    public int DirectionToIndex(Vector2 _direction)
    {
        Vector2 norDir = _direction.normalized;//MARKER return this vector with a magnitude of 1 and get the normalized to an index

        float step = 360 / 8;//MARKER 45 one circle and 8 slices//Calcuate how many degrees one slice is 
        float offset = step / 2;//MARKER 22.5//OFFSET help us easy to calcuate and get the correct index of the string array

        float angle = Vector2.SignedAngle(Vector2.up, norDir);//MARKER returns the signed angle in degrees between A and B

        angle += offset;//Help us easy to calcuate and get the correct index of the string array

        if (angle < 0)//avoid the negative number 
        {
            angle += 360;
        }

        float stepCount = angle / step;
        return Mathf.FloorToInt(stepCount);
    }
}