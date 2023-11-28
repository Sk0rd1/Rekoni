using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnHealthDragon : EnemysHealth
{
    private void Start()
    {
        isBoss = false;
        maxHealth = 50;
        renderer = GetComponentInChildren<Renderer>();
        material = renderer.materials;
        animator = GetComponent<Animator>();
        enMov = GetComponent<EnMovDragon>();
        health = MaxHealth();
    }
}