using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefab = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform parent;

    private void Awake()
    {
        //load ui prefab tu resources
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < prefabs.Length; i++)
        {
            canvasPrefab.Add(prefabs[i].GetType(), prefabs[i]);
        }
    }
    //mo canvas
    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();

        return canvas;
    }
    //dong canvas sau time (s)
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (ISLoaded<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }
    // dong canvas truc tiem
    public void CloseDirectly<T>() where T : UICanvas
    {
        if (ISOpened<T>())
        {
            canvasActives[typeof(T)].CloseDirectly();
        }
    }
    //kiem tra canvas da duoc tao chua
    public bool ISLoaded<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null;
    }
    //kiem tra canvas duoc active hay chua
    public bool ISOpened<T>() where T : UICanvas
    {
        return ISLoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }
    //lay active canvas
    public T GetUI<T>() where T : UICanvas
    {
        if (!ISLoaded<T>())
        {
            T prefab = GetUiPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasActives[typeof(T)] = canvas;
        }
        return canvasActives[typeof(T)] as T;
    }
    //lay prefab
    private T GetUiPrefab<T>() where T : UICanvas
    {
        return canvasPrefab[typeof(T)] as T;
    }
    //dong tat ca
    public void CloseALL()
    {
        foreach (var canvas in canvasActives)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
