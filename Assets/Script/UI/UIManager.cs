using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI spawnCostText;

    public WaveManager waveTimer;



    private void Update()
    {
        WaveLoad();
        GoldLoad();
        TimerLoad();
        SpawnCostLoad();
    }

    private void WaveLoad()
    {
        waveText.text = $"Wave {Define.waveCount}";
    }

    private void GoldLoad()
    {
        goldText.text = $"{Define.gold}";
    }
    private void TimerLoad()
    {
        float time = Mathf.Max(0f, waveTimer.timer);
        int minute = (int)(time / 60f);
        int second = (int)(time % 60f);
        //timerText.text = $"{minute:D2}:{second:D2}"; //
        timerText.text = $"{time:F0}";
    }

    private void SpawnCostLoad()
    {
        spawnCostText.text = $"{Define.spawnCost}";
    }


}



