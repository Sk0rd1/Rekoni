using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.VisualScripting;
using UnityEngine;

public class EnHealthBirb : EnemysHealth
{
    private void Start()
    {
        isBoss = false;
        maxHealth = 50;
        renderer = GetComponentInChildren<Renderer>();
        material = renderer.materials;
        animator = GetComponent<Animator>();
        enMovBirb = GetComponent<EnMovBirb>();
        health = MaxHealth();
    }
}
