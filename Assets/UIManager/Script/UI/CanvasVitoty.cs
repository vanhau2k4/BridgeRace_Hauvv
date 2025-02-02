using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasVitoty : UICanvas
{
    public TMP_Text ranKing;
    public TMP_Text score;
    public void MainMenuButton()
    {
        Close(0);
        UIManager.Instance.OpenUI<CanvasMainMenu>();
    }
    public void SetRanking(int rank)
    {
        ranKing.text = $"Hạng {rank}";
    }
    public void SetScore(int sco)
    {
        score.text = $"Điểm {sco}";
    }
}
