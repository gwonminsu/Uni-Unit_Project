using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private RaycastHit hit;
    private Ray ray;
    private Vector3 originPosition;
    private GameObject unitToMove;
    public float distance = 4;

    void OnMouseDrag()
    {
        if (unitToMove == null)
            return; // unitToMove가 null이면 드래그를 진행하지 않음

        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        unitToMove.transform.position = objPosition;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                originPosition = hit.collider.transform.position;
                unitToMove = hit.collider.gameObject; // 클릭한 유닛을 unitToMove 변수에 할당
                OnMouseDrag(); // 드래그 앤 드랍 시작
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            int layerMask = 1 << LayerMask.NameToLayer("Unit"); // Unit 레이어를 무시하도록 설정
            if (unitToMove != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, layerMask))
                {
                    if (hit.collider.CompareTag("Tile")) // 놓은 위치가 칸일 때만
                    {
                        unitToMove.transform.position = hit.collider.transform.position; // 유닛을 해당 칸으로 이동
                    }
                    else
                    {
                        unitToMove.transform.position = originPosition; // 유닛을 원래 있던 자리로 이동
                    }
                }
                else
                {
                    unitToMove.transform.position = originPosition; // 유닛을 원래 있던 자리로 이동
                }

                unitToMove = null; // 드래그가 종료되면 unitToMove 변수 초기화
            }
        }
    }
}
