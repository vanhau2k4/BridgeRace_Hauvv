using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGame : MonoBehaviour
{
    void Start()
    {
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

}
