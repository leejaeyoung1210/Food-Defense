using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TowerSynergy : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI a;
    [SerializeField] TextMeshProUGUI b;
    [SerializeField] TextMeshProUGUI c;
    [SerializeField] TextMeshProUGUI d;
    [SerializeField] TextMeshProUGUI e;
    [SerializeField] TextMeshProUGUI f;
    [SerializeField] TextMeshProUGUI g;
    [SerializeField] TextMeshProUGUI h;
    [SerializeField] TextMeshProUGUI i;
    [SerializeField] TextMeshProUGUI j;
    [SerializeField] TextMeshProUGUI k;

    [SerializeField] TextMeshProUGUI totalAttackPower;
    [SerializeField] TextMeshProUGUI totalAttackSpeed;
    [SerializeField] TextMeshProUGUI totalEnemySpeed;

    [SerializeField] TowerSpot spots;

    private int synergyId;

    private HashSet<int> activeSynergy = new HashSet<int>(); // 활성화된 시너지들 HashSet 중복허용x 리스트 
    private HashSet<int> currentSynergy = new();   // 이번 프레임 시너지 결과
    private HashSet<int> previousSynergy = new(); //이전 프레임 시너지 결과
    private void Update()
    {

        activeSynergy.Clear();
        currentSynergy.Clear();

        SpotCheck();
        CheckActiveSynergy(activeSynergy);

        foreach (var id in previousSynergy) // 활성화 끄기 
        {
            if (!currentSynergy.Contains(id))
            {
                DisableSynergy(id);
            }
        }

        foreach (var id in currentSynergy)
        {
            if (!previousSynergy.Contains(id))
            {
                SetSynergy(id);
            }
        }
        ApplyBuffTowers(currentSynergy);
        ApplyDeBuffEnemy(currentSynergy);
        previousSynergy = new HashSet<int>(currentSynergy);
    }

    private void SpotCheck() // 스팟에 있는 타워들 체크
    {
        foreach (var tower in spots.spotPoints)
        {
            if (tower.isSpawning)
            {
                int comboType = tower.tower.GetComponent<Tower>().data.Combo_type; // 타워의 시너지 아이디 가져오기
                activeSynergy.Add(comboType); // 활성화된 시너지에 추가   
            }
        }
    }

    private void CheckActiveSynergy(HashSet<int> tower) // 활성화된 시너지 체크    
    {
        var synergyTable = DataTableManager.SynergyTableData;

        foreach (var synergyId in synergyTable.GetAll())
        {
            if (tower.Contains(synergyId.Combo_typeA) && tower.Contains(synergyId.Combo_typeB))
            {
                currentSynergy.Add(synergyId.Id);
            }
        }
    }

    private TextMeshProUGUI GetTextById(int id)
    {
        return id switch
        {
            52401 => a,
            52402 => b,
            51403 => c,
            51004 => d,
            52405 => e,
            52406 => f,
            54007 => g,
            52408 => h,
            52109 => i,
            521010 => j,
            514311 => k,
            _ => null
        };
    }

    private void SetSynergy(int id)
    {
        var synergy = DataTableManager.SynergyTableData.Get(id);
        if (synergy == null) return;

        TextMeshProUGUI target = GetTextById(id);
        if (target == null) return;

        SetTextColor(target);
    }
    private void DisableSynergy(int id)
    {
        var synergy = DataTableManager.SynergyTableData.Get(id);
        if (synergy == null) return;

        TextMeshProUGUI target = GetTextById(id);
        if (target == null) return;

        ResetColor(target);

        foreach (var spot in spots.spotPoints)
        {
            if (!spot.isSpawning) continue;

            var tower = spot.tower.GetComponent<Tower>();
            int type = tower.data.Combo_type;

            if (type == synergy.Combo_typeA || type == synergy.Combo_typeB)
            {
                tower.ResetBuff(); // 버프 제거 메서드
            }
        }
        if (synergy.ValueType1 == 4 || synergy.ValueType2 == 4)
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemyObj in enemies)
            {
                var enemy = enemyObj.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ResetSlow(); // 디버프 제거 메서드
                }
            }
        }
    }

    private void ApplyBuffTowers(HashSet<int> synergyIds)
    {
        foreach (var spot in spots.spotPoints)
        {
            if (!spot.isSpawning) continue;

            var tower = spot.tower.GetComponent<Tower>();
            tower.ResetBuff();

            float towerPower = 0f;
            float towerSpeed = 0f;
            int type = tower.data.Combo_type;

            foreach (int id in synergyIds)
            {
                var synergy = DataTableManager.SynergyTableData.Get(id);
                if (synergy == null) continue;

                if (type == synergy.Combo_typeA || type == synergy.Combo_typeB)
                {
                    if (synergy.ValueType1 == 1) towerPower += synergy.Value1;
                    if (synergy.ValueType2 == 1) towerPower += synergy.Value2;
                    if (synergy.ValueType1 == 2) towerSpeed += synergy.Value1;
                    if (synergy.ValueType2 == 2) towerSpeed += synergy.Value2;
                }
            }

            // 누적 제한 적용
            towerPower = Mathf.Min(towerPower, 25f) / 100f;
            towerSpeed = Mathf.Min(towerSpeed, 25f) / 100f;

            totalAttackPower.text = $"+{towerPower * 100}%";
            totalAttackSpeed.text = $"+{towerSpeed * 100}%";

            tower.ApplyBuff(towerPower, towerSpeed);
        }

    }

    private void ApplyDeBuffEnemy(HashSet<int> synergyIds)
    {
        float slow = 0f;

        foreach (int id in synergyIds)
        {
            var synergy = DataTableManager.SynergyTableData.Get(id);
            if (synergy == null) continue;

            if (synergy.ValueType1 == 4) slow += synergy.Value1;
            if (synergy.ValueType2 == 4) slow += synergy.Value2;
        }

        if (slow == 0f)
        {
            // 시너지 없을 때 텍스트 초기화
            totalEnemySpeed.text = "-0%";

            // 이미 적용된 슬로우도 초기화
            var enemyObj = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemys in enemyObj)
            {
                var enemy = enemys.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ResetSlow();
                }
            }
            return;
        }

        slow = Mathf.Min(slow, 25f) / 100f;

        var enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemyObj in enemyObjects)
        {
            var enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.ApplySlow(slow);
            }
        }

        totalEnemySpeed.text = $"-{slow * 100}%";
    }

    private void SetTextColor(TextMeshProUGUI text)
    {
        if (text.color != Color.yellow)
        {
            text.color = Color.yellow;
        }
    }
    private void ResetColor(TextMeshProUGUI text)
    {
        if (text.color != Color.white)
        {
            text.color = Color.white;
        }
    }
}
