using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StairsCheck : MonoBehaviour
{
    public ColorType color;
    public List<Material> listMauBrick;
    public MeshRenderer meshRenderer;


    Player player;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void AddBrick(Brick brick)
    {
        meshRenderer.enabled = true;
        ChangeStairColor(brick.color);
    }
    // Thay đổi màu của cầu thang theo màu của gạch
    public void ChangeStairColor(ColorType newColor)
    {
        color = newColor;
        Material[] mats = meshRenderer.materials;
        mats[0] = listMauBrick[(int)newColor]; 
        meshRenderer.materials = mats;
    }

    public Material ColorType(ColorType color)
    {
        return listMauBrick[(int)color];
    }
}
