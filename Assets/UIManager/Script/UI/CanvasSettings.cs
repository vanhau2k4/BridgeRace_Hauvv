using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSettings : UICanvas
{
    [SerializeField] GameObject[] button;

    public void SetState(UICanvas _uICanvas)
    {
        for (int i = 0; i < button.Length; i++)
        {
            button[i].gameObject.SetActive(false);
        }
        if (_uICanvas is CanvasMainMenu)
        {
            button[2].gameObject.SetActive(true);
        }
        else if (_uICanvas is CanvasGamePlay)
        {
            button[0].gameObject.SetActive(true);
            button[1].gameObject.SetActive(true);
            
        }
    }
    public void MainMenuButton()
    {
        UIManager.Instance.CloseALL();
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
}

