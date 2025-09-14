using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public WaveSet wave;
    public EnemySpawner enemySpawner;
    public static int currentWave = 0;

    void Start() => StartCoroutine(WaveSet());

    IEnumerator WaveSet()
    {
        for (int i = 0; i < wave.waves.Count; i++)
        {
            yield return StartCoroutine(OnWave(wave.waves[i]));
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator OnWave(WaveData waveData)
    {
        Debug.Log($"{waveData} ½ÃÀÛ");
        return null;    
    }







}
