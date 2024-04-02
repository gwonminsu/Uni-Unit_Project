using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gold = 10; // �÷��̾��� ���� ���
    public GameObject unitPrefab; // ������ ������ ������
    public TextMeshProUGUI goldText; // ��带 ǥ���� TextMeshProUGUI

    private GridManager gridManager;

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
            if (spawnPosition != Vector3.zero) // ��ȿ�� ��ġ�� ���
            {
                // Y ��ǥ�� ȸ������ ����
                spawnPosition.y = 2.56f;
                Quaternion spawnRotation = Quaternion.Euler(85, 0, 0);

                Instantiate(unitPrefab, spawnPosition, spawnRotation);
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
}
