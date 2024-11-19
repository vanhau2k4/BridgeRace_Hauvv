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
    private int currentMapIndex = 0;
    private GameObject currentMapInstance;
    public Button Play1;
    public Button Play2;
    public Button Play3;
    private void ResetNPC()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].Play = true;
            enemy[i].transform.position = new Vector3(-5.3f, 0, -8);
            if (enemy[i].listBrickHiden.Count > 0 || enemy[i].playerBrick.Count > 0)
            {
                for (int e = 0; e < enemy[i].listBrickHiden.Count; e++)
                {
                    Brick brick = enemy[i].listBrickHiden[e];
                    brick.gameObject.SetActive(true);
                }
                for (int e = 0; e < enemy[i].playerBrick.Count; e++)
                {
                    GameObject brick = enemy[i].playerBrick[e];
                    brick.SetActive(false);
                    enemy[i].haibrick -= 0.21f;
                }
                enemy[i].listBrickHiden.Clear();
            }
            enemy[i].hasTriggered = true;
            enemy[i].skinnedMeshRenderer.enabled = true;
            enemy[i].ResumeActions();
        }
    }

    private void ResetPlayer()
    {
        player.controller.enabled = false;
        player.transform.position = new Vector3(6, 5, -8);
        player.controller.enabled = true;
        if (player.listBrickHiden.Count > 0 || player.playerBrick.Count > 0)
        {
            for (int i = 0; i < player.listBrickHiden.Count; i++)
            {
                Brick brick = player.listBrickHiden[i];
                brick.gameObject.SetActive(true);
            }
            for (int i = 0; i < player.playerBrick.Count; i++)
            {
                GameObject brick = player.playerBrick[i];
                brick.SetActive(false);
                player.haibrick -= 0.21f;
            }
            player.listBrickHiden.Clear();
        }
        player.skinnedMeshRenderer.enabled = true;
        player.joystick.isJovstick = true;
        player.hasTriggered = true;
        player.joystick.inputCanvas.gameObject.SetActive(true);
    }

    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
    }
    public void Quaylai()
    {
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }
        SpawnMap(currentMapIndex);
    }

     public void NextMap()
    {
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }

        currentMapIndex = (currentMapIndex + 1) % mapPrefabs.Count;
        SpawnMap(currentMapIndex);
    }

    public void SpawnMap(int index)
    {
        currentMapInstance = Instantiate(mapPrefabs[index], transform.position, Quaternion.identity);
        currentMapInstance.transform.position = Vector3.zero;
    }
    public void SelectMap(int index)
    {
        if (currentMapInstance != null)
        {
            Destroy(currentMapInstance);
        }

        currentMapIndex = index;
        SpawnMap(currentMapIndex);

        Close(0);
        UIManager.Instance.OpenUI<CanvasGamePlay>();
        ResetNPC();
        ResetPlayer();
        ResetStais();

    }

    private void ResetStais()
    {
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
        spoinBrick.SpawnBrick();
    }
}

