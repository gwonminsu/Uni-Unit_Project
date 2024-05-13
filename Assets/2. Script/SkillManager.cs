using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public List<SkillItem> availableSkills = new List<SkillItem>(); // ���� ������ ��ų ���
    private Dictionary<Unit, List<SkillItem>> unitSkills = new Dictionary<Unit, List<SkillItem>>(); // ���ֺ� ��ų ���

    // ��ų ����
    public void BuySkill(SkillItem skill, Unit unit)
    {
        if (GameManager.instance.gold >= skill.cost)
        {
            GameManager.instance.UpdateGold(-skill.cost); // ��� ����
            if (!unitSkills.ContainsKey(unit))
            {
                unitSkills[unit] = new List<SkillItem>();
            }
            unitSkills[unit].Add(skill);
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
        }
    }

    // ��ų Ȱ��ȭ
    public void ActivateSkill(Unit unit, SkillItem skill)
    {
        if (unitSkills.ContainsKey(unit) && unitSkills[unit].Contains(skill))
        {
            skill.Activate(unit);
        }
    }
}