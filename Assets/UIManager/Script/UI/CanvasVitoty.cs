using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasVitoty : UICanvas
{
    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }

}
