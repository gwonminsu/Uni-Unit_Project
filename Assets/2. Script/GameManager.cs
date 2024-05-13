using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱��� �ν��Ͻ� ����

    public int gold = 10; // �÷��̾��� ���� ���
    public GameObject unitPrefab; // ������ ������ ������
    public GameObject uiPrefab; // ���� ui ������
    public TextMeshProUGUI goldText; // ��带 ǥ���� TextMeshProUGUI

    private GridManager gridManager;

    public static int unitIdCounter = 1; // ���� ID ī����
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���ӸŴ��� �ν��Ͻ��� �� �ε� �ÿ��� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ� �ν��Ͻ��� ������ ��� �ı�
        }
    }

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        UpdateGoldUI();
    }

    // ��带 �߰��ϰų� �����ϴ� �Լ�
    public void UpdateGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    // ��� UI ������Ʈ
    void UpdateGoldUI()
    {
        goldText.text = "Gold: " + gold;
    }

    // ���� ���� �Լ�
    public void BuyUnit()
    {
        if (gold >= 2) // ���� ���� ��� üũ
        {
            Vector3 spawnPosition = GetRandomValidPosition();

            // �׸��� �ε����� ���� ���ۿ� �������� 2���� �迭 ������� ��ȯ
            int xIndex_server = Mathf.FloorToInt(spawnPosition.x + 3.5f);
            int zIndex_server = 7 - Mathf.FloorToInt(spawnPosition.z + 3.5f);

            if (spawnPosition != Vector3.zero) // ��ȿ�� ��ġ�� ���
            {
                // Y ��ǥ�� ȸ������ ����
                spawnPosition.y = 2.56f;
                Quaternion spawnRotation = Quaternion.Euler(85, 0, 0);

                GameObject newUnit = Instantiate(unitPrefab, spawnPosition, spawnRotation);
                GameObject unitUI = Instantiate(uiPrefab, newUnit.transform);

                Debug.Log("������ (" + xIndex_server + ", " + zIndex_server + ") ��ǥ�� ��ȯ��");

                Unit unit = newUnit.GetComponent<Unit>();
                if (unit != null) // ���� ���� �����ϴ� ������Ʈ�� ��ǥ�� ����
                {
                    unit.InitializeUnit(xIndex_server, zIndex_server);
                }

                unitUI.transform.localPosition = new Vector3(0, 0, 0);
                newUnit.name = "Unit_" + unitIdCounter++; // ���ֿ� ���� �̸� �ο�
                UpdateGold(-2); // ��� ����

                // �׸��� ��ġ ����
                gridManager.SetOccupied(spawnPosition, true);
            }
            else
            {
                // ��ȿ�� ��ġ�� ���� ��� �α� �޽��� ���
                Debug.Log("���� ����: ��ȿ�� ��ġ�� �������� ����!");
            }
        }
        else
        {
            // ��尡 ������� ���� ����� ����
            Debug.Log("��尡 �����ϴ�!");
        }
    }

    // ��ȿ�� ���� ��ġ ��ȯ �Լ�
    private Vector3 GetRandomValidPosition()
    {
        // GridManager�� ���� ��ü �׸��忡�� ��ȿ�� ��ġ ã��
        Vector3[] validPositions = gridManager.GetValidPositions();
        if (validPositions.Length > 0)
        {
            int randomIndex = Random.Range(0, validPositions.Length);
            return validPositions[randomIndex];
        }
        return Vector3.zero; // ��ȿ�� ��ġ�� ���� ���
    }

    // Ư�� ���Ÿ� ���� �Լ�
    public void PurchaseSkill(SkillItem skill, Unit unit)
    {
        FindObjectOfType<SkillManager>().BuySkill(skill, unit);
    }
}
