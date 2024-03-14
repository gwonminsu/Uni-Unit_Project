using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private GameObject selectedUnit; // ���� ���õ� ����
    private Vector3 initialUnitPosition; // ������ �ʱ� ��ġ
    private GridManager gridManager; // �׸��� �Ŵ���
    private Animator unitAnimator; // ���� �ִϸ����� ����

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
                    initialUnitPosition = hitObject.transform.position; // ������ �ʱ� ��ġ ����
                }

                // ������ ���õǸ� �ִϸ����� ������Ʈ�� ã�Ƽ� ����
                if (selectedUnit != null)
                {
                    unitAnimator = selectedUnit.transform.Find("UnitRoot").GetComponent<Animator>();
                    unitAnimator.SetBool("isRunning", true); // �巡�� ���� �� Run �ִϸ��̼����� ��ȯ
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
            // ������ �׸��� ���� ���� ������ �ʱ� ��ġ�� �ǵ���
            if (!IsValidGridPosition(nearestGridPoint))
            {
                selectedUnit.transform.position = initialUnitPosition;
                Debug.Log("������ ���� ����� �׸��� ����Ʈ�� �̵���: " + nearestGridPoint);
            }
            else
            {
                selectedUnit.transform.position = nearestGridPoint;
            }
            Debug.Log("������ �̵���: " + selectedUnit.transform.position);
            unitAnimator.SetBool("isRunning", false); // �巡�� ���� �� Idle �ִϸ��̼����� ��ȯ
            selectedUnit = null; // ���õ� ���� ����
        }
    }

    // ���� ����
    private void SelectUnit(GameObject unit)
    {
        selectedUnit = unit;
        Debug.Log("���õ� ����: " + selectedUnit.name);
    }

    // �׸��� ��ġ�� ��ȿ���� Ȯ��
    private bool IsValidGridPosition(Vector3 position)
    {
        // �׸����� ������ �Ѿ���� Ȯ��
        return position.x >= -gridManager.gridSizeX / 2 * gridManager.tileSize &&
               position.x < gridManager.gridSizeX / 2 * gridManager.tileSize &&
               position.z >= -gridManager.gridSizeZ / 2 * gridManager.tileSize &&
               position.z < gridManager.gridSizeZ / 2 * gridManager.tileSize;
    }
}
