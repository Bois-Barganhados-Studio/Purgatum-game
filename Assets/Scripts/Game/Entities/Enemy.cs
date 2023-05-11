using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public const int LAYER = 7;

    public Enemy(int hp)
        : base(hp)
    {
    }
}