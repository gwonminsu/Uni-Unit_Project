using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName = "BasicUnit"; // UI 프레임에 표시 될 유닛 이름 (추후 특수 특성 아이템에 의해 이름이 변경될 예정)
    [SerializeField] private int _health = 1; // 체력
    [SerializeField] private int _attack = 1; // 공격력
    public int price = 1; // 유닛 판매 금액
    public float defence = 0; // 유닛이 공격을 받을 때 데미지를 줄여줄 수 있는 값
    public float avoid = 0; // 유닛이 공격을 받을 때 공격을 회피 할 수 있는 확률
    public string current_state = "idle"; // 유닛의 현재 상태 (ex - 기절, 중독 등등)
    [SerializeField] private int _xIndex; // 유닛의 현재 x좌표
    [SerializeField] private int _zIndex; // 유닛의 현재 z좌표

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;

    private int lastHealth;
    private int lastAttack;
    private int _lastXIndex;
    private int _lastZIndex;

    public GameObject movementItem;
    public GameObject attackItem;
    public GameObject specialItem;

    public int Health
    {
        get => _health;
        set => _health = value;
    }

    public int Attack
    {
        get => _attack;
        set => _attack = value;
    }

    public int xIndex
    {
        get => _xIndex;
        set
        {
            lastXIndex = _xIndex; // 변경 전 값 저장
            _xIndex = value;
        }
    }

    public int zIndex
    {
        get => _zIndex;
        set
        {
            lastZIndex = _zIndex; // 변경 전 값 저장
            _zIndex = value;
        }
    }

    public int lastXIndex
    {
        get { return _lastXIndex; }
        set { _lastXIndex = value; }
    }

    public int lastZIndex
    {
        get { return _lastZIndex; }
        set { _lastZIndex = value; }
    }

    void Start()
    {
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name == "UnitPower")
            {
                attackText = text;
            }
            else if (text.gameObject.name == "UnitHP")
            {
                healthText = text;
            }
        }
        UpdateStatsDisplay(); // 초기 상태를 UI에 반영
    }

    void Update()
    {
        if (lastHealth != _health || lastAttack != _attack)
        {
            UpdateStatsDisplay();
            lastHealth = _health;
            lastAttack = _attack;
        }

        if (lastXIndex != _xIndex || lastZIndex != _zIndex)
        {
            UnitManager.instance.MoveUnit(this, _xIndex, _zIndex);
            lastXIndex = _xIndex;
            lastZIndex = _zIndex;
        }
    }

    public void InitializeUnit(int x, int z)
    {
        _xIndex = x;
        _zIndex = z;
        lastXIndex = x;
        lastZIndex = z;
        UpdateStatsDisplay();
    }

    void UpdateStatsDisplay()
    {
        if (attackText != null) attackText.text = _attack.ToString();
        if (healthText != null) healthText.text = _health.ToString();
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

    public void UseItem()
    {
        // 아이템 사용 로직 구현
    }
}
