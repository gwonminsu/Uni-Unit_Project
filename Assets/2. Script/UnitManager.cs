using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using DG.Tweening;

public class UnitManager : MonoBehaviour
{
    public GameObject sellPrefab; // 유닛이 판매될 때 발생하는 효과
    private AudioSource coinBlast; // 판매 효과음
    private GameObject selectedUnit; // 현재 선택된 유닛
    private Vector3 initialUnitPosition; // 유닛이 선택되었을 때의 시작 위치
    private GridManager gridManager; // 그리드 관리를 위한 참조
    private GameManager gameManager; // 게임 관리를 위한 참조
    private Animator unitAnimator; // 유닛의 애니메이터 컴포넌트
    private Outlinable selectedUnitOutlinable; // 유닛의 외곽선 처리를 위한 컴포넌트

    public static UnitManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        coinBlast = GetComponent<AudioSource>();
        // 게임 시작 시 그리드 매니저를 찾아 참조 저장
        gridManager = FindObjectOfType<GridManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // 매 프레임마다 마우스 입력을 체크
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 마우스 클릭 위치에서 레이캐스트 발사
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitObject = hit.collider.gameObject;

                // 레이캐스트가 유닛에 맞았을 경우, 그 유닛을 선택
                if (hitObject.CompareTag("Unit"))
                {
                    SelectUnit(hitObject);
                }
            }
        }

        // 선택된 유닛이 있고 마우스 버튼이 눌려있으면, 드래그 처리
        if (selectedUnit != null && Input.GetMouseButton(0))
        {
            DragUnit();
        }

        // 마우스 버튼을 놓으면, 유닛을 해당 위치에 배치
        if (selectedUnit != null && Input.GetMouseButtonUp(0))
        {
            PlaceUnit();
        }
    }

    private void SelectUnit(GameObject unit)
    {
        selectedUnit = unit;
        initialUnitPosition = unit.transform.position;
        unitAnimator = unit.GetComponentInChildren<Animator>();
        unitAnimator.SetBool("isRunning", true);
        selectedUnitOutlinable = selectedUnit.GetComponent<Outlinable>();
        selectedUnitOutlinable.OutlineParameters.Color = Color.white;
    }

    private void DragUnit()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = selectedUnit.transform.position.y;
            selectedUnit.transform.position = newPosition;

            // 그리드 매니저를 통해 현재 위치가 유효한지 확인하고, 인디케이터를 업데이트
            bool isValidPosition = gridManager.IsValidGridPosition(newPosition);
            gridManager.UpdateIndicator(newPosition, isValidPosition);
        }
        if (IsOutsideGrid(selectedUnit.transform.position))
        {
            selectedUnitOutlinable.OutlineParameters.Color = Color.yellow;
        }
        else
        {
            selectedUnitOutlinable.OutlineParameters.Color = Color.white;
        }
    }

    private void PlaceUnit()
    {
        if (IsOutsideGrid(selectedUnit.transform.position))
        {
            SellUnit();
        }
        else
        {
            Vector3 nearestGridPoint = gridManager.GetNearestGridPoint(selectedUnit.transform.position);
            if (gridManager.IsValidGridPosition(nearestGridPoint))
            {
                gridManager.SetOccupied(initialUnitPosition, false); // 이전 위치의 점유 상태 해제

                selectedUnit.transform.position = nearestGridPoint; // 새 위치로 이동

                // 그리드 인덱스를 서버 전송용 전통적인 2차원 배열 방식으로 변환
                int xIndex_server = Mathf.FloorToInt(nearestGridPoint.x + 3.5f);
                int zIndex_server = 7 - Mathf.FloorToInt(nearestGridPoint.z + 3.5f);

                Debug.Log("유닛[" + selectedUnit.name + "](이)가 " + "xIndex: " + xIndex_server + ", zIndex: " + zIndex_server + "로 이동되었음");
                Unit unit = selectedUnit.GetComponent<Unit>();
                if (unit != null) // 유닛 정보 관리하는 컴포넌트의 좌표값 전송
                {
                    unit.xIndex = xIndex_server;
                    unit.zIndex = zIndex_server;
                }

                gridManager.SetOccupied(nearestGridPoint, true); // 새 위치의 점유 상태 설정
            }
            else
            {
                selectedUnit.transform.position = initialUnitPosition; // 유효하지 않은 위치면 원래 위치로
            }
        }
        

        unitAnimator.SetBool("isRunning", false);
        selectedUnitOutlinable.OutlineParameters.Color = Color.green;
        selectedUnit = null;
        gridManager.ResetIndicators(); // 모든 인디케이터 초기화
    }

    public void MoveUnit(Unit unit, int xIndex, int zIndex)
    {
        Vector3 targetPosition = unit.transform.position; // 변경할 유닛 위치
        Vector3 initailTargetPosition = unit.transform.position; // 유닛의 초기 위치
        targetPosition.x = (xIndex - 3.5f); // unit.cs의 변경된 xIndex의 실제 위치로 세팅
        targetPosition.z = (7 - zIndex - 3.5f); // 마찬가지

        // 그리드 바깥이면 유닛 판매
        if (IsOutsideGrid(targetPosition))
        {
            SellUnit(unit);
        }
        else
        {
            if (gridManager.IsValidGridPosition(targetPosition))
            {
                // 이동 가능하면 실제 유닛 위치 업데이트, DOTween을 사용하여 새 위치로 부드럽게 이동
                unit.transform.DOMove(targetPosition, 0.5f) // 이동 시간 0.5초
                    .SetEase(Ease.OutBack) // Bounce 효과 적용
                    .OnStart(() => {
                        gridManager.SetOccupied(unit.transform.position, false); // 현재 위치 점유 해제
                    })
                    .OnComplete(() => {
                        gridManager.SetOccupied(targetPosition, true); // 새 위치 점유
                        unit.xIndex = xIndex;
                        unit.zIndex = zIndex;
                    });
            }
            else
            {
                // 이미 점유된 위치면 로그 출력하고 원래 위치로
                Debug.Log("이미 점유된 위치입니다.");
                unit.transform.position = initailTargetPosition;
                unit.xIndex = unit.lastXIndex; // 이전 xIndex로 복원
                unit.zIndex = unit.lastZIndex; // 이전 zIndex로 복원
            }
        }
    }


    private bool IsOutsideGrid(Vector3 position)
    {
        // 그리드 바깥인지 확인
        return !gridManager.IsValidGridPosition(position) && !gridManager.IsInGridBounds(position);
    }

    // 드래그로 선택한 유닛 판매
    private void SellUnit()
    {
        gameManager.UpdateGold(1); // 골드 증가
        Instantiate(sellPrefab, selectedUnit.transform.position, Quaternion.identity); // FX 효과 생성
        Destroy(selectedUnit); // 유닛 제거

        // 효과음 재생
        if (coinBlast != null)
        {
            coinBlast.Play();
        }
    }

    // 특정 유닛 판매
    private void SellUnit(Unit unit)
    {
        gameManager.UpdateGold(unit.price);
        Instantiate(sellPrefab, unit.transform.position, Quaternion.identity);
        Destroy(unit.gameObject);
        if (coinBlast != null)
        {
            coinBlast.Play();
        }
    }
}