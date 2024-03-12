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
            return; // unitToMove�� null�̸� �巡�׸� �������� ����

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
                unitToMove = hit.collider.gameObject; // Ŭ���� ������ unitToMove ������ �Ҵ�
                OnMouseDrag(); // �巡�� �� ��� ����
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            int layerMask = 1 << LayerMask.NameToLayer("Unit"); // Unit ���̾ �����ϵ��� ����
            if (unitToMove != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, layerMask))
                {
                    if (hit.collider.CompareTag("Tile")) // ���� ��ġ�� ĭ�� ����
                    {
                        unitToMove.transform.position = hit.collider.transform.position; // ������ �ش� ĭ���� �̵�
                    }
                    else
                    {
                        unitToMove.transform.position = originPosition; // ������ ���� �ִ� �ڸ��� �̵�
                    }
                }
                else
                {
                    unitToMove.transform.position = originPosition; // ������ ���� �ִ� �ڸ��� �̵�
                }

                unitToMove = null; // �巡�װ� ����Ǹ� unitToMove ���� �ʱ�ȭ
            }
        }
    }
}
