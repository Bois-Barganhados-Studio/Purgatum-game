using System.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;
    public string[] staticDirections = { "Idle N", "Idle NW", "Idle W", "Idle SW", "Idle S", "Idle SE", "Idle E", "Idle NE" };
    public string[] runDirections = { "Running N", "Running NW", "Running W", "Running SW", "Running S", "Running SE", "Running E", "Running NE" };
    public string[] AttackDirections = { "Attacking N", "Attacking NW", "Attacking W", "Attacking SW", "Attacking S", "Attacking SE", "Attacking E", "Attacking NE" };
    public string[] DieDirection = { "Dying N", "Dying NW", "Dying W", "Dying SW", "Dying S", "Dying SE", "Dying E", "Dying NE" };
    private int lastDirection;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void die(Transform target)
    {
        lastDirection = DirectionToIndex(target);
        //string[] DieDirection = { "Dying N", "Dying NW", "Dying W", "Dying SW", "Dying S", "Dying SE", "Dying E", "Dying NE" };
        anim.Play(DieDirection[lastDirection]);
    }

    public void idle(Transform target)
    {
        lastDirection = DirectionToIndex(target);
        //lastDirection = DirectionToIndex(_direction);//MARKER Get the index of the slcie from the direction vector
        anim.Play(staticDirections[lastDirection]);
    }

    public void moving(Transform target)
    {
        lastDirection = DirectionToIndex(target);
        //lastDirection = DirectionToIndex(_direction);//MARKER Get the index of the slcie from the direction vector
        anim.Play(runDirections[lastDirection]);
    }

    public void attacking(Transform target)
    {
        lastDirection = DirectionToIndex(target);
        //lastDirection = DirectionToIndex(_direction);//MARKER Get the index of the slcie from the direction vector
        anim.Play(staticDirections[lastDirection]);
    }

    public int DirectionToIndex(Transform target)
    {
        float angulo = (float)Math.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg;
        int index = 0;
        angulo -= 90.0f;   
        if(angulo < 0)
        {
            angulo += 360;
        }
        if(angulo == 0) angulo = 360;
        
        
        index = (int)Mathf.Round(angulo / 45) % 8;
        UnityEngine.Debug.Log("x: " + (target.position.x - transform.position.x) + " Y: " + (target.position.y - transform.position.y) + " angulo: " + angulo + " index: " + index);
        return index;
        
    }
}