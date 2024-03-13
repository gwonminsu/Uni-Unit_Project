using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private GameObject selectedUnit; // 현재 선택된 유닛
    private GridManager gridManager; // 그리드 매니저

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

        // 드래그가 끝나면 가장 가까운 인덱싱된 타일로 유닛 이동
        if (selectedUnit != null && Input.GetMouseButtonUp(0))
        {
            Vector3 nearestGridPoint = gridManager.GetNearestGridPoint(selectedUnit.transform.position);
            selectedUnit.transform.position = nearestGridPoint;
            Debug.Log("유닛이 가장 가까운 그리드 포인트로 이동함: " + nearestGridPoint);
            selectedUnit = null; // 선택된 유닛 해제
        }
    }

    // 유닛 선택
    private void SelectUnit(GameObject unit)
    {
        selectedUnit = unit;
        Debug.Log("선택된 유닛: " + selectedUnit.name);
    }
}
