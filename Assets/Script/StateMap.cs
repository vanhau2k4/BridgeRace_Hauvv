using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class StateMap : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
