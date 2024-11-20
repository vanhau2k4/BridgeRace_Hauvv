using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class StateLevel : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    public GameObject[] hiddenColumn;
    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface.BuildNavMesh();
        Invoke(nameof(OnSkin),1f);
    }

    private void OnSkin()
    {
        for (int i = 0; i < hiddenColumn.Length; i++)
        {
            hiddenColumn[i].SetActive(true);
        }
    }
}
