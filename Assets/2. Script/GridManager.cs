using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridTilePrefab; // �׸��� Ÿ���� ������ ������
    public int gridSizeX = 8; // �׸����� ���� ũ��
    public int gridSizeZ = 8; // �׸����� ���� ũ��
    public float tileSize = 1.0f; // Ÿ�� �ϳ��� ũ��

    private GameObject[,] gridTiles; // �׸��� Ÿ���� ������ �迭
    private GameObject[,] indicatorTiles; // �ε������� Ÿ���� ������ �迭
    private bool[,] isOccupied; // �ش� Ÿ���� �����Ǿ����� ���θ� ������ �迭

    public GameObject indicatorPrefab; // �ε������� ������
    public Material indicatorValidMaterial; // ��ȿ�� ��ġ ��ġ�� ǥ���� �� ����� ��Ƽ����
    public Material indicatorInvalidMaterial; // ��ȿ���� ���� ��ġ ��ġ�� ǥ���� �� ����� ��Ƽ����

    void Start()
    {
        CreateGrid();
        CreateIndicators();
    }

    private void CreateGrid()
    {
        // �׸���� Ÿ�� �迭 �ʱ�ȭ
        gridTiles = new GameObject[gridSizeX, gridSizeZ];
        indicatorTiles = new GameObject[gridSizeX, gridSizeZ];
        isOccupied = new bool[gridSizeX, gridSizeZ];

        // �׸��� Ÿ�� ����
        for (float x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (float z = -gridSizeZ / 2; z < gridSizeZ / 2; z++)
            {
                // Ÿ���� �߽� ��ġ ���
                Vector3 tilePosition = new Vector3(x * tileSize + tileSize / 2, 2, z * tileSize + tileSize / 2);
                // Ÿ�� �ν��Ͻ� ����
                GameObject gridTile = Instantiate(gridTilePrefab, tilePosition, Quaternion.identity);
                gridTile.transform.localScale = new Vector3(tileSize, 1.0f, tileSize);
                gridTile.transform.SetParent(transform); // ���� ��ü�� �ڽ����� ����
                gridTiles[(int)(x + gridSizeX / 2), (int)(z + gridSizeZ / 2)] = gridTile;
            }
        }
    }

    private void CreateIndicators()
    {
        // �ε������� Ÿ�� ����
        for (float x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (float z = -gridSizeZ / 2; z < gridSizeZ / 2; z++)
            {
                // �ε������� Ÿ�� ��ġ ��� (�ణ ���� ����)
                Vector3 tilePosition = new Vector3(x * tileSize + tileSize / 2, 2.5f, z * tileSize + tileSize / 2);
                // �ε������� Ÿ�� �ν��Ͻ� ����
                GameObject indicatorTile = Instantiate(indicatorPrefab, tilePosition, Quaternion.identity, transform);
                indicatorTile.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
                indicatorTiles[(int)(x + gridSizeX / 2), (int)(z + gridSizeZ / 2)] = indicatorTile;
            }
        }
    }

    public void UpdateIndicator(Vector3 position, bool valid)
    {
        // ��ġ�� ���� ����� �׸��� �ε��� ���
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);

        // �ε����� ��ȿ�� ���� ���� �ִ��� Ȯ��
        if (xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ)
        {
            ResetIndicators();
            GameObject indicatorTile = indicatorTiles[xIndex, zIndex];
            indicatorTile.SetActive(true);
            // ��ȿ�� ��ġ��� �ʷϻ�, �ƴϸ� ���������� ǥ��
            indicatorTile.GetComponent<Renderer>().material = valid ? indicatorValidMaterial : indicatorInvalidMaterial;
        }
        else
        {
            // �ε����� ���� �ۿ� ������ �ε������͸� ��Ȱ��ȭ
            ResetIndicators();
        }
    }

    // ��� �ε������� ��Ȱ��ȭ
    public void ResetIndicators()
    {
        foreach (GameObject indicatorTile in indicatorTiles)
        {
            if (indicatorTile != null)
            {
                indicatorTile.SetActive(false);
            }
        }
    }

    // ��ȿ�� �׸��� ��ġ���� �Ǻ�
    public bool IsValidGridPosition(Vector3 position)
    {
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);

        // �׸��� ���� �ȿ� �ִ��� Ȯ��
        bool isInGridRange = xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ;
        if (!isInGridRange)
        {
            return false;
        }

        // �Ʒ��� �׸��� �������� ������ ���� �� �ְ� ����
        bool isLowerHalf = zIndex < gridSizeZ / 2;

        // �ش� ��ġ�� �������� �ʾҴ��� Ȯ��
        bool isNotOccupied = !isOccupied[xIndex, zIndex];

        return isLowerHalf && isNotOccupied;
    }


    // �׸��� ��ġ�� ���� ���� ����
    public void SetOccupied(Vector3 position, bool occupied)
    {
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);


        if (xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ)
        {
            isOccupied[xIndex, zIndex] = occupied;
        }
    }

    // ���� ����� �׸��� ����Ʈ ��ȯ
    public Vector3 GetNearestGridPoint(Vector3 position)
    {
        // �߽� ��ġ ���
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);
        Vector3 nearestPoint = new Vector3(xIndex * tileSize + tileSize / 2 - gridSizeX * tileSize / 2, position.y, zIndex * tileSize + tileSize / 2 - gridSizeZ * tileSize / 2);

        // Debug.Log("xIndex: " + xIndex + ", zIndex: " + zIndex);

        return nearestPoint;
    }
}