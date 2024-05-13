using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤 인스턴스 선언

    public int gold = 10; // 플레이어의 시작 골드
    public GameObject unitPrefab; // 구매할 유닛의 프리팹
    public GameObject uiPrefab; // 유닛 ui 프리팹
    public TextMeshProUGUI goldText; // 골드를 표시할 TextMeshProUGUI

    private GridManager gridManager;

    public static int unitIdCounter = 1; // 유닛 ID 카운터
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 게임매니저 인스턴스가 씬 로드 시에도 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스가 생성될 경우 파괴
        }
    }

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

            // 그리드 인덱스를 서버 전송용 전통적인 2차원 배열 방식으로 변환
            int xIndex_server = Mathf.FloorToInt(spawnPosition.x + 3.5f);
            int zIndex_server = 7 - Mathf.FloorToInt(spawnPosition.z + 3.5f);

            if (spawnPosition != Vector3.zero) // 유효한 위치인 경우
            {
                // Y 좌표와 회전값을 조정
                spawnPosition.y = 2.56f;
                Quaternion spawnRotation = Quaternion.Euler(85, 0, 0);

                GameObject newUnit = Instantiate(unitPrefab, spawnPosition, spawnRotation);
                GameObject unitUI = Instantiate(uiPrefab, newUnit.transform);

                Debug.Log("유닛이 (" + xIndex_server + ", " + zIndex_server + ") 좌표에 소환됨");

                Unit unit = newUnit.GetComponent<Unit>();
                if (unit != null) // 유닛 정보 관리하는 컴포넌트의 좌표값 전송
                {
                    unit.InitializeUnit(xIndex_server, zIndex_server);
                }

                unitUI.transform.localPosition = new Vector3(0, 0, 0);
                newUnit.name = "Unit_" + unitIdCounter++; // 유닛에 고유 이름 부여
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

    // 특성 구매를 위한 함수
    public void PurchaseSkill(SkillItem skill, Unit unit)
    {
        FindObjectOfType<SkillManager>().BuySkill(skill, unit);
    }
}
