using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;

public class UnitManager : MonoBehaviour
{
    public GameObject sellPrefab; // ������ �Ǹŵ� �� �߻��ϴ� ȿ��
    private AudioSource coinBlast; // �Ǹ� ȿ����
    private GameObject selectedUnit; // ���� ���õ� ����
    private Vector3 initialUnitPosition; // ������ ���õǾ��� ���� ���� ��ġ
    private GridManager gridManager; // �׸��� ������ ���� ����
    private GameManager gameManager; // ���� ������ ���� ����
    private Animator unitAnimator; // ������ �ִϸ����� ������Ʈ
    private Outlinable selectedUnitOutlinable; // ������ �ܰ��� ó���� ���� ������Ʈ

    void Start()
    {
        coinBlast = GetComponent<AudioSource>();
        // ���� ���� �� �׸��� �Ŵ����� ã�� ���� ����
        gridManager = FindObjectOfType<GridManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // �� �����Ӹ��� ���콺 �Է��� üũ
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // ���콺 Ŭ�� ��ġ���� ����ĳ��Ʈ �߻�
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject hitObject = hit.collider.gameObject;

                // ����ĳ��Ʈ�� ���ֿ� �¾��� ���, �� ������ ����
                if (hitObject.CompareTag("Unit"))
                {
                    SelectUnit(hitObject);
                }
            }
        }

        // ���õ� ������ �ְ� ���콺 ��ư�� ����������, �巡�� ó��
        if (selectedUnit != null && Input.GetMouseButton(0))
        {
            DragUnit();
        }

        // ���콺 ��ư�� ������, ������ �ش� ��ġ�� ��ġ
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

            // �׸��� �Ŵ����� ���� ���� ��ġ�� ��ȿ���� Ȯ���ϰ�, �ε������͸� ������Ʈ
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
                gridManager.SetOccupied(initialUnitPosition, false); // ���� ��ġ�� ���� ���� ����

                selectedUnit.transform.position = nearestGridPoint; // �� ��ġ�� �̵�

                // �׸��� �ε����� ���� ���ۿ� �������� 2���� �迭 ������� ��ȯ
                int xIndex_server = Mathf.FloorToInt(nearestGridPoint.x + 3.5f);
                int zIndex_server = 7 - Mathf.FloorToInt(nearestGridPoint.z + 3.5f);

                Debug.Log("����[" + selectedUnit.name + "](��)�� " + "xIndex: " + xIndex_server + ", zIndex: " + zIndex_server + "�� �̵��Ǿ���");

                gridManager.SetOccupied(nearestGridPoint, true); // �� ��ġ�� ���� ���� ����
            }
            else
            {
                selectedUnit.transform.position = initialUnitPosition; // ��ȿ���� ���� ��ġ�� ���� ��ġ��
                                                                       // ���⿡ �˸� ǥ�� ���� �߰� ����
            }
        }
        

        unitAnimator.SetBool("isRunning", false);
        selectedUnitOutlinable.OutlineParameters.Color = Color.green;
        selectedUnit = null;
        gridManager.ResetIndicators(); // ��� �ε������� �ʱ�ȭ
    }

    private bool IsOutsideGrid(Vector3 position)
    {
        // �׸��� �ٱ����� Ȯ��
        return !gridManager.IsValidGridPosition(position) && !gridManager.IsInGridBounds(position);
    }

    private void SellUnit()
    {
        gameManager.UpdateGold(1); // ��� ����
        Instantiate(sellPrefab, selectedUnit.transform.position, Quaternion.identity); // FX ȿ�� ����
        Destroy(selectedUnit); // ���� ����

        // ȿ���� ���
        if (coinBlast != null)
        {
            coinBlast.Play();
        }
    }
}