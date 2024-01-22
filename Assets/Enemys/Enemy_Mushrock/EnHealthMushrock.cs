using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnHealthMushrock : EnemysHealth
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
