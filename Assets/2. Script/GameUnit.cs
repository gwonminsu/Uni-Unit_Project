using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유닛의 모든 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/CreateUnit", order = 1)]
public class GameUnit : ScriptableObject
{
    /// 게임 내에서 생성할 유닛 프리팹
    public GameObject prefab;

    /// 유닛이 공격할 때 생성할 발사체 프리팹
    public GameObject attackProjectile;

    // UI 프레임에 표시 될 유닛 이름
    public string uiname;

    // 유닛의 레벨
    public int lv = 1;

    // 유닛의 판매 금액
    public int price = 1;

    // 유닛의 체력
    public float hp = 10;

    // 유닛의 공격 성공 시 피격 데미지
    public float damage = 5;

    // 유닛이 공격을 받을 때 데미지를 줄여줄 수 있는 값
    public float defence = 0;

    // 유닛이 공격을 받을 때 공격을 회피 할 수 있는 확률
    public float avoid = 0;

    // 유닛이 공격할 수 있는 사거리
    public float attackRange = 1;

    // 유닛의 현재 상태 (ex - 기절, 중독 등등)
    public string current_state = "idle";
}
