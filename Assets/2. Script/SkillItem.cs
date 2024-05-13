using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillItem", menuName = "SkillItem")]
public class SkillItem : ScriptableObject
{
    public string skillName;
    public int level;
    public int cost;
    public int sellPrice;
    public Sprite icon;

    // 스킬의 효과를 발동하는 함수
    public virtual void Activate(Unit unit)
    {
        switch (skillName)
        {
            case "한칸 이동":
                unit.ZIndex -= 1;  // 유닛이 한 칸 앞으로 이동하는 메서드 호출
                break;
            case "전방 공격":
                PerformAttackAt(unit.ZIndex - 1);  // 앞 칸에 공격 이펙트 발생 및 피해 적용 메서드 호출
                break;
            case "매턴 회복(소)":
                unit.Health += 2;  // 매턴 유닛의 체력을 2 회복하는 메서드 호출
                break;
                // 추가적인 스킬 로직 구현 가능
        }


    }
    private void PerformAttackAt(int zIndex)
    {
        // 해당 zIndex에서 공격 로직을 구현, 예: 지정된 인덱스에 있는 유닛을 찾아 공격
    }

}


