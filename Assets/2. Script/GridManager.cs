using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject gridTilePrefab; // 그리드 타일을 생성할 프리팹
    public int gridSizeX = 8; // 그리드의 가로 크기
    public int gridSizeZ = 8; // 그리드의 세로 크기
    public float tileSize = 1.0f; // 타일 하나의 크기

    private GameObject[,] gridTiles; // 그리드 타일을 저장할 배열
    private GameObject[,] indicatorTiles; // 인디케이터 타일을 저장할 배열
    private bool[,] isOccupied; // 해당 타일이 점유되었는지 여부를 저장할 배열

    public GameObject indicatorPrefab; // 인디케이터 프리팹
    public Material indicatorValidMaterial; // 유효한 배치 위치를 표시할 때 사용할 머티리얼
    public Material indicatorInvalidMaterial; // 유효하지 않은 배치 위치를 표시할 때 사용할 머티리얼

    void Start()
    {
        CreateGrid();
        CreateIndicators();
    }

    private void CreateGrid()
    {
        // 그리드와 타일 배열 초기화
        gridTiles = new GameObject[gridSizeX, gridSizeZ];
        indicatorTiles = new GameObject[gridSizeX, gridSizeZ];
        isOccupied = new bool[gridSizeX, gridSizeZ];

        // 그리드 타일 생성
        for (float x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (float z = -gridSizeZ / 2; z < gridSizeZ / 2; z++)
            {
                // 타일의 중심 위치 계산
                Vector3 tilePosition = new Vector3(x * tileSize + tileSize / 2, 2, z * tileSize + tileSize / 2);
                // 타일 인스턴스 생성
                GameObject gridTile = Instantiate(gridTilePrefab, tilePosition, Quaternion.identity);
                gridTile.transform.localScale = new Vector3(tileSize, 1.0f, tileSize);
                gridTile.transform.SetParent(transform); // 현재 객체의 자식으로 설정
                gridTiles[(int)(x + gridSizeX / 2), (int)(z + gridSizeZ / 2)] = gridTile;
            }
        }
    }

    private void CreateIndicators()
    {
        // 인디케이터 타일 생성
        for (float x = -gridSizeX / 2; x < gridSizeX / 2; x++)
        {
            for (float z = -gridSizeZ / 2; z < gridSizeZ / 2; z++)
            {
                // 인디케이터 타일 위치 계산 (약간 높게 설정)
                Vector3 tilePosition = new Vector3(x * tileSize + tileSize / 2, 2.5f, z * tileSize + tileSize / 2);
                // 인디케이터 타일 인스턴스 생성
                GameObject indicatorTile = Instantiate(indicatorPrefab, tilePosition, Quaternion.identity, transform);
                indicatorTile.SetActive(false); // 초기에는 비활성화
                indicatorTiles[(int)(x + gridSizeX / 2), (int)(z + gridSizeZ / 2)] = indicatorTile;
            }
        }
    }

    public void UpdateIndicator(Vector3 position, bool valid)
    {
        // 위치에 가장 가까운 그리드 인덱스 계산
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);

        // 인덱스가 유효한 범위 내에 있는지 확인
        if (xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ)
        {
            ResetIndicators();
            GameObject indicatorTile = indicatorTiles[xIndex, zIndex];
            indicatorTile.SetActive(true);
            // 유효한 위치라면 초록색, 아니면 빨간색으로 표시
            indicatorTile.GetComponent<Renderer>().material = valid ? indicatorValidMaterial : indicatorInvalidMaterial;
        }
        else
        {
            // 인덱스가 범위 밖에 있으면 인디케이터를 비활성화
            ResetIndicators();
        }
    }

    // 모든 인디케이터 비활성화
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

    // 유효한 그리드 위치인지 판별
    public bool IsValidGridPosition(Vector3 position)
    {
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);

        // 그리드 범위 안에 있는지 확인
        bool isInGridRange = xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ;
        if (!isInGridRange)
        {
            return false;
        }

        // 아래쪽 그리드 영역에만 유닛을 놓을 수 있게 변경
        bool isLowerHalf = zIndex < gridSizeZ / 2;

        // 해당 위치가 점유되지 않았는지 확인
        bool isNotOccupied = !isOccupied[xIndex, zIndex];

        return isLowerHalf && isNotOccupied;
    }

    // 그리드 범위 내에 있는지 확인
    public bool IsInGridBounds(Vector3 position)
    {
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);

        // 위치가 그리드 경계 내에 있는지 확인
        return xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ;
    }


    // 그리드 위치의 점유 상태 설정
    public void SetOccupied(Vector3 position, bool occupied)
    {
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);


        if (xIndex >= 0 && xIndex < gridSizeX && zIndex >= 0 && zIndex < gridSizeZ)
        {
            isOccupied[xIndex, zIndex] = occupied;
        }
    }

    // 가장 가까운 그리드 포인트 반환
    public Vector3 GetNearestGridPoint(Vector3 position)
    {
        // 중심 위치 계산
        int xIndex = Mathf.FloorToInt((position.x - tileSize / 2 + gridSizeX * tileSize / 2) / tileSize);
        int zIndex = Mathf.FloorToInt((position.z - tileSize / 2 + gridSizeZ * tileSize / 2) / tileSize);
        Vector3 nearestPoint = new Vector3(xIndex * tileSize + tileSize / 2 - gridSizeX * tileSize / 2, position.y, zIndex * tileSize + tileSize / 2 - gridSizeZ * tileSize / 2);

        // Debug.Log("xIndex: " + xIndex + ", zIndex: " + zIndex);

        return nearestPoint;
    }

    // 유효한 위치 반환 함수
    public Vector3[] GetValidPositions()
    {
        List<Vector3> validPositions = new List<Vector3>();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 position = new Vector3(x * tileSize + tileSize / 2 - gridSizeX * tileSize / 2, 0, z * tileSize + tileSize / 2 - gridSizeZ * tileSize / 2);
                if (IsValidGridPosition(position))
                {
                    validPositions.Add(position);
                }
            }
        }
        return validPositions.ToArray();
    }
}