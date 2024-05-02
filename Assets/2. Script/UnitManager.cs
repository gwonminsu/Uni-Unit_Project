using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPOOutline;
using DG.Tweening;

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

    public static UnitManager instance;

    private void Awake()
    {
        instance = this;
    }

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
                Unit unit = selectedUnit.GetComponent<Unit>();
                if (unit != null) // ���� ���� �����ϴ� ������Ʈ�� ��ǥ�� ����
                {
                    unit.xIndex = xIndex_server;
                    unit.zIndex = zIndex_server;
                }

                gridManager.SetOccupied(nearestGridPoint, true); // �� ��ġ�� ���� ���� ����
            }
            else
            {
                selectedUnit.transform.position = initialUnitPosition; // ��ȿ���� ���� ��ġ�� ���� ��ġ��
            }
        }
        

        unitAnimator.SetBool("isRunning", false);
        selectedUnitOutlinable.OutlineParameters.Color = Color.green;
        selectedUnit = null;
        gridManager.ResetIndicators(); // ��� �ε������� �ʱ�ȭ
    }

    public void MoveUnit(Unit unit, int xIndex, int zIndex)
    {
        Vector3 targetPosition = unit.transform.position; // ������ ���� ��ġ
        Vector3 initailTargetPosition = unit.transform.position; // ������ �ʱ� ��ġ
        targetPosition.x = (xIndex - 3.5f); // unit.cs�� ����� xIndex�� ���� ��ġ�� ����
        targetPosition.z = (7 - zIndex - 3.5f); // ��������

        // �׸��� �ٱ��̸� ���� �Ǹ�
        if (IsOutsideGrid(targetPosition))
        {
            SellUnit(unit);
        }
        else
        {
            if (gridManager.IsValidGridPosition(targetPosition))
            {
                // �̵� �����ϸ� ���� ���� ��ġ ������Ʈ, DOTween�� ����Ͽ� �� ��ġ�� �ε巴�� �̵�
                unit.transform.DOMove(targetPosition, 0.5f) // �̵� �ð� 0.5��
                    .SetEase(Ease.OutBack) // Bounce ȿ�� ����
                    .OnStart(() => {
                        gridManager.SetOccupied(unit.transform.position, false); // ���� ��ġ ���� ����
                    })
                    .OnComplete(() => {
                        gridManager.SetOccupied(targetPosition, true); // �� ��ġ ����
                        unit.xIndex = xIndex;
                        unit.zIndex = zIndex;
                    });
            }
            else
            {
                // �̹� ������ ��ġ�� �α� ����ϰ� ���� ��ġ��
                Debug.Log("�̹� ������ ��ġ�Դϴ�.");
                unit.transform.position = initailTargetPosition;
                unit.xIndex = unit.lastXIndex; // ���� xIndex�� ����
                unit.zIndex = unit.lastZIndex; // ���� zIndex�� ����
            }
        }
    }


    private bool IsOutsideGrid(Vector3 position)
    {
        // �׸��� �ٱ����� Ȯ��
        return !gridManager.IsValidGridPosition(position) && !gridManager.IsInGridBounds(position);
    }

    // �巡�׷� ������ ���� �Ǹ�
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

    // Ư�� ���� �Ǹ�
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