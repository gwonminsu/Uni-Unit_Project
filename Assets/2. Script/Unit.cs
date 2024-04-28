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
    public int xIndex; // ������ ���� x��ǥ
    public int zIndex; // ������ ���� z��ǥ

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI healthText;

    private int lastHealth;
    private int lastAttack;

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
