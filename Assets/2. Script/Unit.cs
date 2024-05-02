using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName = "BasicUnit"; // UI �����ӿ� ǥ�� �� ���� �̸� (���� Ư�� Ư�� �����ۿ� ���� �̸��� ����� ����)
    [SerializeField] private int _health = 1; // ü��
    [SerializeField] private int _attack = 1; // ���ݷ�
    public int price = 1; // ���� �Ǹ� �ݾ�
    public float defence = 0; // ������ ������ ���� �� �������� �ٿ��� �� �ִ� ��
    public float avoid = 0; // ������ ������ ���� �� ������ ȸ�� �� �� �ִ� Ȯ��
    public string current_state = "idle"; // ������ ���� ���� (ex - ����, �ߵ� ���)
    [SerializeField] private int _xIndex; // ������ ���� x��ǥ
    [SerializeField] private int _zIndex; // ������ ���� z��ǥ

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
            lastXIndex = _xIndex; // ���� �� �� ����
            _xIndex = value;
        }
    }

    public int zIndex
    {
        get => _zIndex;
        set
        {
            lastZIndex = _zIndex; // ���� �� �� ����
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
        UpdateStatsDisplay(); // �ʱ� ���¸� UI�� �ݿ�
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

    // ������ �ൿ�� �����ϴ� �޼����
    public void Move()
    {
        // �̵� ���� ����
    }

    public void PerformAttack()
    {
        // ���� ���� ����
    }

    public void UseItem()
    {
        // ������ ��� ���� ����
    }
}
