using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class SpoinBrick : MonoBehaviour
{
    public GameObject brickPrefab;

    public List<Material> listMauBrick;
    public Transform[] floors;
    // Số lượng cố định cho mỗi màu (ví dụ: tổng số gạch muốn tạo là 168)
    public int[] colorCounts = new int[4] { 42, 42, 42, 42 }; 

    public static SpoinBrick Instance;
    public List<Brick> listBricks = new List<Brick>();
    public int totalBricks;
    public Brick brickComponent;
   
    void Awake()
    {
        Instance = this; 
    }



    public void SpawnBrick()
    {
        Vector3 startPos = floors[0].transform.position;
        totalBricks = 0; 
        int totalToSpawn = 168; 

        while (totalBricks < totalToSpawn)
        {
            Vector3 spawnPos = startPos + new Vector3(totalBricks % 14, 0, totalBricks / 14);
            GameObject brick = Instantiate(brickPrefab, spawnPos, Quaternion.identity);

            
            ColorType randomColorType = GetRandomColorType();
            if (randomColorType != ColorType.None) 
            {
                brickComponent = brick.GetComponent<Brick>();
                if (brickComponent != null)
                {
                    brickComponent.color = randomColorType; 
                    brickComponent.ChangeColor(); 
                }
                totalBricks++;
                listBricks.Add(brickComponent);
            }
            else
            {
                Destroy(brick);
            }
        }
    }
    public void SpawnBricksInFloorAnColor(int floor, ColorType color)
    {
        for (int i = 0; i < listBricks.Count; i++)
        {
            if (listBricks[i].color == color)
            {
                Brick brick = listBricks[i];
                brick.transform.position = brick.transform.position - floors[floor - 1].position;
                brick.transform.position = brick.transform.position + floors[floor].position;
            }
        }
    }
        // Phương thức để lấy màu ngẫu nhiên và cập nhật số lượng
    private ColorType GetRandomColorType()
    {
        int colorIndex = Random.Range(0, colorCounts.Length);
        if (colorCounts[colorIndex] > 0)
        {
            colorCounts[colorIndex]--; 
            return (ColorType)colorIndex; 
        }
        else
        {
            return GetRandomColorType(); 
        }
    }
}
