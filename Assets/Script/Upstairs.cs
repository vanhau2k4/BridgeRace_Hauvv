using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Upstairs : MonoBehaviour
{
    public ColorType color;
    private SpoinBrick spoinBrick;

    private void Start()
    {
        spoinBrick = FindObjectOfType<SpoinBrick>();
        if (spoinBrick == null)
        {
            Debug.Log("asdf");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if ( other.TryGetComponent(out CharacterBase player))
        {
            if (player.hasTriggered == true)
            {
                color = player.color;
                spoinBrick.SpawnBricksInFloorAnColor(1, color);
                player.hasTriggered = false;
            }
        }
    }
}
