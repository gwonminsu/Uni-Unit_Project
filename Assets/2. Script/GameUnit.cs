using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��� ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/CreateUnit", order = 1)]
public class GameUnit : ScriptableObject
{
    /// ���� ������ ������ ���� ������
    public GameObject prefab;

    /// ������ ������ �� ������ �߻�ü ������
    public GameObject attackProjectile;

    // UI �����ӿ� ǥ�� �� ���� �̸�
    public string uiname;

    // ������ ����
    public int lv = 1;

    // ������ �Ǹ� �ݾ�
    public int price = 1;

    // ������ ü��
    public float hp = 10;

    // ������ ���� ���� �� �ǰ� ������
    public float damage = 5;

    // ������ ������ ���� �� �������� �ٿ��� �� �ִ� ��
    public float defence = 0;

    // ������ ������ ���� �� ������ ȸ�� �� �� �ִ� Ȯ��
    public float avoid = 0;

    // ������ ������ �� �ִ� ��Ÿ�
    public float attackRange = 1;

    // ������ ���� ���� (ex - ����, �ߵ� ���)
    public string current_state = "idle";
}
