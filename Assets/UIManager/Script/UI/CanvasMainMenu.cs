using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CanvasMainMenu : UICanvas
{
    public List<GameObject> mapPrefabs;
    
    public int currentMapIndex = 0;
    public GameObject currentMapInstances;
    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
    }
    
    public void SelectMap(int index)
    {
        if (currentMapInstances != null)
        {
            Destroy(currentMapInstances);
        }
        Close(0);
        UIManager.Instance.OpenUI<CanvasGamePlay>();

        Invoke(nameof(ResetPlayer), 1f);
        Invoke(nameof(ResetNPC), 1f);
        if (spoinBrick.listBricks.Count > 0)
        {
            spoinBrick.colorCounts = new int[4] { 42, 42, 42, 42 };
            spoinBrick.listBricks.Clear();
            Brick[] bricks = FindObjectsOfType<Brick>();
            foreach (Brick brick in bricks)
            {
                Destroy(brick.gameObject);
            }
        }
        Invoke(nameof(ResetBrick), 1f);

        currentMapIndex = index;
        Invoke(nameof(SpawnMapDelayed), 1f);


    }
    void SpawnMapDelayed()
    {
        SpawnMap(currentMapIndex); 
    }
    public void SpawnMap(int index)
    {
        currentMapInstances = Instantiate(mapPrefabs[index], transform.position, Quaternion.identity);
        currentMapInstances.transform.position = Vector3.zero;
    }
    public void ResetBrick()
    {

        spoinBrick.SpawnBrick();
    }
    public void ResetNPC()
    {
        for (int i = 0; i < enemy.Length; i++)
        {

            float randomX = Random.Range(-2, 2);
            float randomZ = Random.Range(-2, 2);

            // Gán vị trí mới cho enemy[i]
            enemy[i].transform.position = new Vector3(randomX, 0, randomZ);

            if (enemy[i].listBrickHiden.Count > 0 || enemy[i].playerBrick.Count > 0)
            {

                for (int e = 0; e < enemy[i].playerBrick.Count; e++)
                {

                    enemy[i].haibrick -= 0.21f;
                }
                enemy[i].playerBrick.Clear();
                enemy[i].listBrickHiden.Clear();
                foreach (Transform child in enemy[i].transform)
                {
                    // Kiểm tra nếu child có tag hoặc tên liên quan đến "Brick"
                    if (child.CompareTag("Brick")) // Đảm bảo các brick có tag "Brick"
                    {
                        Destroy(child.gameObject); // Xóa game object
                    }
                }
            }

            enemy[i].hasTriggered = true;
            enemy[i].skinnedMeshRenderer.enabled = true;
            enemy[i].RandomizeMaxBricks();
            enemy[i].FindBrick();
            enemy[i].ResumeActions();
            enemy[i].Play = true;
        }
    }

    public void ResetPlayer()
    {
        player.controller.enabled = false;
        player.transform.position = new Vector3(6, 5, -8);
        player.controller.enabled = true;
        if (player.listBrickHiden.Count > 0 || player.playerBrick.Count > 0)
        {


            for (int i = 0; i < player.listBrickHiden.Count; i++)
            {

                player.haibrick -= 0.21f;
            }
            player.playerBrick.Clear();
            player.listBrickHiden.Clear();
            foreach (Transform child in player.transform)
            {
                // Kiểm tra nếu child có tag hoặc tên liên quan đến "Brick"
                if (child.CompareTag("Brick")) // Đảm bảo các brick có tag "Brick"
                {
                    Destroy(child.gameObject); // Xóa game object
                }
            }
        }
        player.skinnedMeshRenderer.enabled = true;
        player.joystick.isJovstick = true;
        player.hasTriggered = true;
        player.joystick.inputCanvas.gameObject.SetActive(true);
    }
}

