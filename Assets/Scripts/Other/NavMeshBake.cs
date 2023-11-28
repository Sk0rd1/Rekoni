using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBake : MonoBehaviour
{
    void Awake()
    {
        NavMeshSurface navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }

}
