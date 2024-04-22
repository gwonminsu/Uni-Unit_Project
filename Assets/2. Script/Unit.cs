using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName; // UI 프레임에 표시 될 유닛 이름 (추후 특수 특성 아이템에 의해 이름이 변경될 예정)
    public int health; // 체력
    public int attack; // 공격력
    public int price; // 유닛 판매 금액
    public float defence; // 유닛이 공격을 받을 때 데미지를 줄여줄 수 있는 값
    public float avoid; // 유닛이 공격을 받을 때 공격을 회피 할 수 있는 확률
    public string current_state ; // 유닛의 현재 상태 (ex - 기절, 중독 등등)

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;

    public GameObject movementItem;
    public GameObject attackItem;
    public GameObject specialItem;

    void Start()
    {
        unitName = "BasicUnit";
        health = 1;
        attack = 1;
        price = 1;
        defence = 0;
        avoid = 0;
        current_state = "idle";
        // 각 텍스트 컴포넌트 찾기
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name == "UnitPower")  // 이름을 통해 특정
            {
                attackText = text;
            }
            else if (text.gameObject.name == "UnitHP")
            {
                healthText = text;
            }
        }
        UpdateStatsDisplay();
    }

    void UpdateStatsDisplay()
    {
        if (attackText != null)
            attackText.text = attack.ToString();
        if (healthText != null)
            healthText.text = health.ToString();
    }

    // 유닛의 행동을 정의하는 메서드들
    public void Move()
    {
        // 이동 로직 구현
    }

    public void PerformAttack()
    {
        // 공격 로직 구현
    }

    // 아이템 사용 메서드
    public void UseItem()
    {
        // 아이템 사용 로직 구현
    }
}
