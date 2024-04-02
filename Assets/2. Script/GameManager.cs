using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gold = 10; // 플레이어의 시작 골드
    public GameObject unitPrefab; // 구매할 유닛의 프리팹
    public TextMeshProUGUI goldText; // 골드를 표시할 TextMeshProUGUI

    private GridManager gridManager;

    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        UpdateGoldUI();
    }

    // 골드를 추가하거나 차감하는 함수
    public void UpdateGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    // 골드 UI 업데이트
    void UpdateGoldUI()
    {
        goldText.text = "Gold: " + gold;
    }

    // 유닛 구매 함수
    public void BuyUnit()
    {
        if (gold >= 2) // 유닛 구매 비용 체크
        {
            Vector3 spawnPosition = GetRandomValidPosition();
            if (spawnPosition != Vector3.zero) // 유효한 위치인 경우
            {
                // Y 좌표와 회전값을 조정
                spawnPosition.y = 2.56f;
                Quaternion spawnRotation = Quaternion.Euler(85, 0, 0);

                Instantiate(unitPrefab, spawnPosition, spawnRotation);
                UpdateGold(-2); // 골드 차감

                // 그리드 위치 점유
                gridManager.SetOccupied(spawnPosition, true);
            }
            else
            {
                // 유효한 위치가 없는 경우 로그 메시지 출력
                Debug.Log("구매 실패: 유효한 위치가 존재하지 않음!");
            }
        }
        else
        {
            // 골드가 충분하지 않은 경우의 로직
            Debug.Log("골드가 부족하다!");
        }
    }

    // 유효한 랜덤 위치 반환 함수
    private Vector3 GetRandomValidPosition()
    {
        // GridManager를 통해 전체 그리드에서 유효한 위치 찾기
        Vector3[] validPositions = gridManager.GetValidPositions();
        if (validPositions.Length > 0)
        {
            int randomIndex = Random.Range(0, validPositions.Length);
            return validPositions[randomIndex];
        }
        return Vector3.zero; // 유효한 위치가 없는 경우
    }
}
