using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class UnitManager : MonoBehaviour
{
    private GameObject selectedUnit; // 현재 선택된 유닛
    private Vector3 initialUnitPosition; // 유닛의 초기 위치
    private GridManager gridManager; // 그리드 매니저
    private Animator unitAnimator; // 유닛 애니메이터 참조
    private Outlinable selectedUnitOutlinable; // 선택된 유닛의 Outlinable 컴포넌트

    void Start()
    {
        // 그리드 매니저 찾기
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        // 마우스 왼쪽 버튼을 누르면
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 2f); // 레이캐스트 시각화

            // 마우스 클릭 위치에서 레이를 쏴서 충돌된 오브젝트를 확인
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitObject = hit.collider.gameObject;

                // 충돌된 오브젝트가 유닛이면 선택
                if (hitObject.CompareTag("Unit"))
                {
                    SelectUnit(hitObject);
                    initialUnitPosition = hitObject.transform.position; // 유닛의 초기 위치 저장
                }
            }
        }

        // 선택된 유닛이 있고, 드래그 중이면
        if (selectedUnit != null && Input.GetMouseButton(0))
        {
            // 마우스 커서 위치로 유닛 이동
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 newPosition = hit.point;
                newPosition.y = selectedUnit.transform.position.y; // 유닛의 원래 Y 위치 유지
                selectedUnit.transform.position = newPosition;
            }
        }

        // 드래그가 끝나면
        if (selectedUnit != null && Input.GetMouseButtonUp(0))
        {
            Vector3 nearestGridPoint = gridManager.GetNearestGridPoint(selectedUnit.transform.position);
            // 유닛이 그리드 위에 있지 않으면 초기 위치로 되돌림
            if (!IsValidGridPosition(nearestGridPoint))
            {
                selectedUnit.transform.position = initialUnitPosition;
                Debug.Log("유닛이 가장 가까운 그리드 포인트로 이동함: " + nearestGridPoint);
            }
            else
            {
                selectedUnit.transform.position = nearestGridPoint;
            }
            Debug.Log("유닛이 이동함: " + selectedUnit.transform.position);
            unitAnimator.SetBool("isRunning", false); // 드래그 끝날 때 Idle 애니메이션으로 전환

            // 아웃라인을 원래 색상으로 변경
            if (selectedUnitOutlinable != null)
            {
                selectedUnitOutlinable.OutlineParameters.Color = Color.green;
            }

            selectedUnit = null; // 선택된 유닛 해제
        }
    }

    // 유닛 선택
    private void SelectUnit(GameObject unit)
    {
        selectedUnit = unit;
        Debug.Log("선택된 유닛: " + selectedUnit.name);

        unitAnimator = selectedUnit.GetComponentInChildren<Animator>();
        unitAnimator.SetBool("isRunning", true); // 드래그 시작 시 Run 애니메이션으로 전환

        selectedUnitOutlinable = selectedUnit.GetComponent<Outlinable>(); // Outlinable 컴포넌트 가져오기
        if (selectedUnitOutlinable != null)
        {
            selectedUnitOutlinable.OutlineParameters.Color = Color.white; // 아웃라인 색상을 하얀색으로 변경
        }
    }

    // 그리드 위치가 유효한지 확인
    private bool IsValidGridPosition(Vector3 position)
    {
        // 그리드의 범위를 넘어가는지 확인
        return position.x >= -gridManager.gridSizeX / 2 * gridManager.tileSize &&
               position.x < gridManager.gridSizeX / 2 * gridManager.tileSize &&
               position.z >= -gridManager.gridSizeZ / 2 * gridManager.tileSize &&
               position.z < gridManager.gridSizeZ / 2 * gridManager.tileSize;
    }
}

