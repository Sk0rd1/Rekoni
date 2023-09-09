using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBake : MonoBehaviour
{
    void Start()
    {
        NavMeshSurface navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
