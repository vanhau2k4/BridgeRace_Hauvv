using System;
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
        player.controller.enabled = false;
        player.skinnedMeshRenderer.enabled = false;
        foreach (var npc in enemy)
        {
            npc.skinnedMeshRenderer.enabled = false;
        }
        if (currentMapInstances != null)
        {
            Destroy(currentMapInstances);
        }
        Close(0);
        UIManager.Instance.OpenUI<CanvasGamePlay>();
        currentMapIndex = index;
        Invoke(nameof(SpawnMapDelayed), 0.9f);
        Invoke(nameof(ResetPlayer), 1.1f);
        Invoke(nameof(ResetNPC), 1.1f);
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
        Time.timeScale = 1f;
        
        AudioManager.instance.PlayMusic("PlayGame");
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
        foreach (var npc in enemy)
        {
            float randomX = UnityEngine.Random.Range(-3, 3);
            float randomZ = UnityEngine.Random.Range(-3, 3);

            // Gán vị trí mới cho enemy[i]
            npc.transform.position = new Vector3(randomX, 0, randomZ);
            // Kiểm tra và xóa các viên gạch
            if (npc.listBrickHiden.Count > 0 || npc.playerBrick.Count > 0)
            {
                float totalSubtract = 0.21f * npc.playerBrick.Count;
                npc.haibrick = Mathf.Max(0, npc.haibrick - totalSubtract); // Đảm bảo không bị âm
                npc.playerBrick.Clear();
                npc.listBrickHiden.Clear();

                // Lưu danh sách các Brick trước khi xóa
                List<GameObject> bricksToDestroy = new List<GameObject>();
                foreach (Transform child in npc.transform)
                {
                    if (child.CompareTag("Brick"))
                    {
                        bricksToDestroy.Add(child.gameObject);
                    }
                }
                foreach (var brick in bricksToDestroy)
                {
                    Destroy(brick);
                }
            }

            // Reset trạng thái NPC
            npc.RandomizeMaxBricks();
            npc.FindBrick();
            npc.ResumeActions();
        }
    }
    
    public void ResetPlayer()
    {
        player.transform.position = new Vector3(0,0, 0);
        if (player.listBrickHiden.Count > 0 || player.playerBrick.Count > 0)
        {


            for (int i = 0; i < player.listBrickHiden.Count; i++)
            {

                player.haibrick -= 0.21f;
            }
            player.playerBrick.Clear();
            player.listBrickHiden.Clear();
            List<GameObject> bricksToDestroy = new List<GameObject>();
            foreach (Transform child in player.transform)
            {
                if (child.CompareTag("Brick"))
                {
                    bricksToDestroy.Add(child.gameObject);
                }
            }
            foreach (var brick in bricksToDestroy)
            {
                Destroy(brick);
            }
        }
        player.controller.enabled = true;
        player.rb.useGravity = false;
        player.skinnedMeshRenderer.enabled = true;
        player.joystick.isJovstick = true;
        player.hasTriggered = true;
        player.joystick.inputCanvas.gameObject.SetActive(true);
    }
}

