using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    public ColorType color;

    public List<Material> listMauBrick;
    public MeshRenderer meshRenderer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterBase characted))
        {
            color = characted.color;
            Material[] mats = meshRenderer.materials;
            mats[0] = listMauBrick[(int)color];
            meshRenderer.materials = mats;
        }
    }
}
