using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasGamePlay : UICanvas
{

    public override void Setup()
    {
        base.Setup();
        
    }


    public void SettingsButton()
    {
        UIManager.Instance.OpenUI<CanvasSettings>().SetState(this);
        player.joystick.inputCanvas.gameObject.SetActive(false);
        Time.timeScale = 0f;
/*        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].Play = false;
        }*/
    }
}
