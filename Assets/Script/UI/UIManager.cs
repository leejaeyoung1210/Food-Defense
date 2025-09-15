using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI goldText;


    private void Update()
    {
        WaveLoad();
        GoldLoad();
    }   

    private void WaveLoad()
    {
        waveText.text = $"Wave : {Define.waveCount}";   
    }

    private void GoldLoad()
    {
        goldText.text = $"Gold : {Define.Gold}";
    }


}



