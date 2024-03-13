using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private GameObject selectedUnit; // ���� ���õ� ����
    private GridManager gridManager; // �׸��� �Ŵ���

    void Start()
    {
        // �׸��� �Ŵ��� ã��
        gridManager = FindObjectOfType<GridManager>();
    }

    void Update()
    {
        // ���콺 ���� ��ư�� ������
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 2f); // ����ĳ��Ʈ �ð�ȭ

            // ���콺 Ŭ�� ��ġ���� ���̸� ���� �浹�� ������Ʈ�� Ȯ��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitObject = hit.collider.gameObject;

                // �浹�� ������Ʈ�� �����̸� ����
                if (hitObject.CompareTag("Unit"))
                {
                    SelectUnit(hitObject);
                }
            }
        }

        // ���õ� ������ �ְ�, �巡�� ���̸�
        if (selectedUnit != null && Input.GetMouseButton(0))
        {
            // ���콺 Ŀ�� ��ġ�� ���� �̵�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 newPosition = hit.point;
                newPosition.y = selectedUnit.transform.position.y; // ������ ���� Y ��ġ ����
                selectedUnit.transform.position = newPosition;
            }
        }

        // �巡�װ� ������ ���� ����� �ε��̵� Ÿ�Ϸ� ���� �̵�
        if (selectedUnit != null && Input.GetMouseButtonUp(0))
        {
            Vector3 nearestGridPoint = gridManager.GetNearestGridPoint(selectedUnit.transform.position);
            selectedUnit.transform.position = nearestGridPoint;
            Debug.Log("������ ���� ����� �׸��� ����Ʈ�� �̵���: " + nearestGridPoint);
            selectedUnit = null; // ���õ� ���� ����
        }
    }

    // ���� ����
    private void SelectUnit(GameObject unit)
    {
        selectedUnit = unit;
        Debug.Log("���õ� ����: " + selectedUnit.name);
    }
}
