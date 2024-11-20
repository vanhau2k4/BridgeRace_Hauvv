using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool isDestroyOnClose = false;
    public Player player {  get; private set; }
    public Enemy[] enemy {  get; private set; }
    public SpoinBrick spoinBrick;

    


    private void Awake()
    {
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBotton = rect.offsetMin;
            Vector2 rightBotton = rect.offsetMax;

            leftBotton.y = 0f;
            rightBotton.y = -100f;

            rect.offsetMin = leftBotton;
            rect.offsetMax = rightBotton;
        }
    }
    public virtual void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectsOfType<Enemy>();
        spoinBrick = FindObjectOfType<SpoinBrick>();
    }
    //goi truoc khi canvas duoc acive
    public virtual void Setup()
    {

    }
    //goi sau khi canvas duoc active
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    //tat canvas sau bao nhieu giay
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time);
    }
    //tat truc tiep canvas
    public virtual void CloseDirectly()
    {
        gameObject.SetActive(false);
    }
    public virtual void CloseDirectlyCotinue()
    {
        gameObject.SetActive(false);
        player.joystick.inputCanvas.gameObject.SetActive(true);
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].Play = true;
        }
    }
    public virtual void DestroyOnClose()
    {
        if (isDestroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

/*    public void Quaylai()
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
    }*/






}
