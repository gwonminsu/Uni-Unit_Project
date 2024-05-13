using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillItem> availableSkills = new List<SkillItem>(); // 구매 가능한 스킬 목록
    private Dictionary<Unit, List<SkillItem>> unitSkills = new Dictionary<Unit, List<SkillItem>>(); // 유닛별 스킬 목록

    // 스킬 구매
    public void BuySkill(SkillItem skill, Unit unit)
    {
        if (GameManager.instance.gold >= skill.cost)
        {
            GameManager.instance.UpdateGold(-skill.cost); // 골드 차감
            if (!unitSkills.ContainsKey(unit))
            {
                unitSkills[unit] = new List<SkillItem>();
            }
            unitSkills[unit].Add(skill);
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
        }
    }

    // 스킬 활성화
    public void ActivateSkill(Unit unit, SkillItem skill)
    {
        if (unitSkills.ContainsKey(unit) && unitSkills[unit].Contains(skill))
        {
            skill.Activate(unit);
        }
    }
}