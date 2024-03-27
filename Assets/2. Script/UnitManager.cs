using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class UnitManager : MonoBehaviour
{
    private GameObject selectedUnit; // 현재 선택된 유닛
    private Vector3 initialUnitPosition; // 유닛이 선택되었을 때의 시작 위치
    private GridManager gridManager; // 그리드 관리를 위한 참조
    private Animator unitAnimator; // 유닛의 애니메이터 컴포넌트
    private Outlinable selectedUnitOutlinable; // 유닛의 외곽선 처리를 위한 컴포넌트

    void Start()
    {
        // 게임 시작 시 그리드 매니저를 찾아 참조 저장
        gridManager = FindObjectOfType<GridManager>();
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
    }

    private void PlaceUnit()
    {
        Vector3 nearestGridPoint = gridManager.GetNearestGridPoint(selectedUnit.transform.position);
        if (gridManager.IsValidGridPosition(nearestGridPoint))
        {
            gridManager.SetOccupied(initialUnitPosition, false); // 이전 위치의 점유 상태 해제

            selectedUnit.transform.position = nearestGridPoint; // 새 위치로 이동

            gridManager.SetOccupied(nearestGridPoint, true); // 새 위치의 점유 상태 설정
        }
        else
        {
            selectedUnit.transform.position = initialUnitPosition; // 유효하지 않은 위치면 원래 위치로
            // 여기에 알림 표시 로직 추가 가능
        }

        unitAnimator.SetBool("isRunning", false);
        selectedUnitOutlinable.OutlineParameters.Color = Color.green;
        selectedUnit = null;
        gridManager.ResetIndicators(); // 모든 인디케이터 초기화
    }
}
