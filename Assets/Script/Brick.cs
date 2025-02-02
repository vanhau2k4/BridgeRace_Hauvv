using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public ColorType color; 
    public List<Material> listMauBrick;
    public MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        ChangeColor();
    }
    public Material GetColorMaterial(ColorType color)
    {
        return listMauBrick[(int)color];
    }

    public void ChangeColor()
    {
        Material[] mat = meshRenderer.materials;
        mat[0] = GetColorMaterial(color);
        meshRenderer.materials = mat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterBase player))
        {
            if (player.color != color)
            {
                return;
            }
            gameObject.SetActive(false);
            player.AddBrick(this);
            if(other.TryGetComponent(out Player play))
            {
                AudioManager.instance.PlaySFX("EatBrick");
            }
        }
    }
}
