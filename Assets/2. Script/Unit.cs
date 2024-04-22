using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName; // UI �����ӿ� ǥ�� �� ���� �̸� (���� Ư�� Ư�� �����ۿ� ���� �̸��� ����� ����)
    public int health; // ü��
    public int attack; // ���ݷ�
    public int price; // ���� �Ǹ� �ݾ�
    public float defence; // ������ ������ ���� �� �������� �ٿ��� �� �ִ� ��
    public float avoid; // ������ ������ ���� �� ������ ȸ�� �� �� �ִ� Ȯ��
    public string current_state ; // ������ ���� ���� (ex - ����, �ߵ� ���)

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
        // �� �ؽ�Ʈ ������Ʈ ã��
        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            if (text.gameObject.name == "UnitPower")  // �̸��� ���� Ư��
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

    // ������ �ൿ�� �����ϴ� �޼����
    public void Move()
    {
        // �̵� ���� ����
    }

    public void PerformAttack()
    {
        // ���� ���� ����
    }

    // ������ ��� �޼���
    public void UseItem()
    {
        // ������ ��� ���� ����
    }
}
