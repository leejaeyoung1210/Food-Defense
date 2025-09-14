using UnityEngine;
using System.Collections;



public class WaveManager : MonoBehaviour
{
    public WaveSet wave;
    public EnemySpawner enemySpawner;
    public int currentWave { get; private set; } = 0;

    public static int enemytotalcount = 0;

    void Start() => StartCoroutine(WaveSet());

    IEnumerator WaveSet() // 웨이브 단계
    {
        for (int i = 0; i < wave.waves.Count; i++)
        {
            yield return StartCoroutine(OnWave(wave.waves[i]));
            currentWave++;
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator OnWave(WaveData wave) // 웨이브 플레이
    {
        float timer = wave.waveTime;
        foreach (var slot in wave.slots)
        {
            for (int i = 0; i < slot.count; i++)
            {
                var enemy = slot.enemy;
                if (enemy != null)
                {
                    enemySpawner.Spawn(enemy, enemySpawner.transform.position);
                    enemytotalcount++;
                }
                yield return new WaitForSeconds(1f);
            }
        }

        while (true)
        {
            timer -= Time.deltaTime;
            if (enemytotalcount <= 0)
            {
                Debug.Log($"{wave.waveNumber} 종료");
                yield break;
            }

            if(timer <= 0f)
            {
                Debug.Log($"{wave.waveNumber} 시간 종료");
                Time.timeScale = 0f;
            }
            yield return null;
        }
    }
}
