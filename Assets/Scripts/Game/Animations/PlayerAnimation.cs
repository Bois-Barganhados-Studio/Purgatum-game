using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    public string[] staticDirections = { "Idle N", "Idle NW", "Idle W", "Idle SW", "Idle S", "Idle SE", "Idle E", "Idle NE" };
    public string[] runDirections = { "Running N", "Running NW", "Running W", "Running SW", "Running S", "Running SE", "Running E", "Running NE" };
    //public string[] dodgeDirections = ;
    public string[] AttackDirections = { "Attacking N", "Attacking NW", "Attacking W", "Attacking SW", "Attacking S", "Attacking SE", "Attacking E", "Attacking NE" };

    int lastDirection;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        //float result1 = Vector2.SignedAngle(Vector2.up, Vector2.right);
        //Debug.Log("R1 " + result1);

        //float result2 = Vector2.SignedAngle(Vector2.up, Vector2.left);
        //Debug.Log("R2 " + result2);

        //float result3 = Vector2.SignedAngle(Vector2.up, Vector2.down);
        //Debug.Log("R3 " + result3);
    }

    //MARKER each direction will match with one string element
    //MARKER We used direction to determine their animation
    public void SetMoveDirection(Vector2 _direction)
    {
        string[] directionArray = null;

        if (_direction.magnitude < 0.01)//MARKER Character is static. And his velocity is close to zero
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = runDirections;

            lastDirection = DirectionToIndex(_direction);//MARKER Get the index of the slcie from the direction vector
        }

        anim.Play(directionArray[lastDirection]);
    }

    public void SetDodgeDirection(Vector2 _direction)
    { 
        string[] animDirection = { "Rolling N", "Rolling NW", "Rolling W", "Rolling SW", "Rolling S", "Rolling SE", "Rolling E", "Rolling NE" };
        anim.Play(animDirection[DirectionToIndex(_direction)]);
    }

    public void SetAttackDirection(Vector2 _direction)
    {
        string[] directionArray = null;

        if (_direction.magnitude < 0.01)//MARKER Character is static. And his velocity is close to zero
        {
            directionArray = staticDirections;
        }
        else
        {
            directionArray = AttackDirections;

            lastDirection = DirectionToIndex(_direction);//MARKER Get the index of the slcie from the direction vector
           
        }
        anim.Play(directionArray[lastDirection]);
    }

    public void SetDyingDirection(Vector2 _direction)
    {
        string[] dyingDirection = { "Dying N", "Dying NW", "Dying W", "Dying SW", "Dying S", "Dying SE", "Dying E", "Dying NE" };
        anim.Play(dyingDirection[DirectionToIndex(_direction)]);
    }
        //MARKER Converts a Vector2 direction to an index to a slcie around a circle
        //CORE this goes in a counter-clock direction
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