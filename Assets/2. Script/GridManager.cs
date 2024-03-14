using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridTilePrefab; // �׸��� Ÿ�� ������
    public int gridSizeX = 8; // �׸����� ���� ũ��
    public int gridSizeZ = 8; // �׸����� ���� ũ��
    public float tileSize = 1.0f; // �׸��� Ÿ���� ũ��

    private GameObject[,] gridTiles; // �׸��� Ÿ�� �迭

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        gridTiles = new GameObject[gridSizeX, gridSizeZ];

        // �׸��� ����
        for (float x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (float z = -gridSizeZ / 2; z < gridSizeZ / 2; z++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize + tileSize / 2, 2, z * tileSize + tileSize / 2); // �׸��� Ÿ���� �߽����� ��ġ ����
                GameObject gridTile = Instantiate(gridTilePrefab, tilePosition, Quaternion.identity);
                gridTile.transform.localScale = new Vector3(tileSize, 1.0f, tileSize);
                gridTile.transform.SetParent(transform); // �׸��� �Ŵ����� �ڽ����� ����
                gridTiles[(int)(x + gridSizeX / 2), (int)(z + gridSizeZ / 2)] = gridTile;
            }
        }
    }

    // ������ �̵� ������ ��ġ�� ��ȯ�ϴ� �޼���
    public Vector3 GetNearestGridPoint(Vector3 position)
    {
        float halfTileSize = tileSize / 2.0f; // Ÿ�� ũ���� ����
        int xIndex = Mathf.RoundToInt(position.x / tileSize); // X �ε��� ���
        int zIndex = Mathf.RoundToInt(position.z / tileSize); // Z �ε��� ���

        // ���� ����� �׸��� Ÿ���� �߽� ��ġ ���
        float nearestX = (float)xIndex * tileSize + halfTileSize;
        float nearestZ = (float)zIndex * tileSize + halfTileSize;

        return new Vector3(nearestX, position.y, nearestZ - 0.2f);
    }
}
