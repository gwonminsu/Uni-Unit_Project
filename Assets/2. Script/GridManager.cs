using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridTilePrefab; // 그리드 타일 프리팹
    public int gridSizeX = 8; // 그리드의 가로 크기
    public int gridSizeZ = 8; // 그리드의 세로 크기
    public float tileSize = 1.0f; // 그리드 타일의 크기

    private GameObject[,] gridTiles; // 그리드 타일 배열

    void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        gridTiles = new GameObject[gridSizeX, gridSizeZ];

        // 그리드 생성
        for (float x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (float z = -gridSizeZ / 2; z < gridSizeZ / 2; z++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize + tileSize / 2, 2, z * tileSize + tileSize / 2); // 그리드 타일의 중심으로 위치 조정
                GameObject gridTile = Instantiate(gridTilePrefab, tilePosition, Quaternion.identity);
                gridTile.transform.localScale = new Vector3(tileSize, 1.0f, tileSize);
                gridTile.transform.SetParent(transform); // 그리드 매니저의 자식으로 설정
                gridTiles[(int)(x + gridSizeX / 2), (int)(z + gridSizeZ / 2)] = gridTile;
            }
        }
    }

    // 유닛의 이동 가능한 위치를 반환하는 메서드
    public Vector3 GetNearestGridPoint(Vector3 position)
    {
        float halfTileSize = tileSize / 2.0f; // 타일 크기의 절반
        int xIndex = Mathf.RoundToInt(position.x / tileSize); // X 인덱스 계산
        int zIndex = Mathf.RoundToInt(position.z / tileSize); // Z 인덱스 계산

        // 가장 가까운 그리드 타일의 중심 위치 계산
        float nearestX = (float)xIndex * tileSize + halfTileSize;
        float nearestZ = (float)zIndex * tileSize + halfTileSize;

        return new Vector3(nearestX, position.y, nearestZ - 0.2f);
    }
}
