using UnityEngine;
using System.Collections.Generic;

public class HandPanel : MonoBehaviour
{
    public static HandPanel instance;
    private List<SkillItem> storedSkills = new List<SkillItem>();

    private void Awake()
    {
        instance = this;
    }

    public RectTransform GetRectTransform()
    {
        return GetComponent<RectTransform>();
    }

    public void AddSkill(SkillItem skill)
    {
        if (!storedSkills.Contains(skill))
        {
            storedSkills.Add(skill);
            // UI 업데이트 로직 추가
        }
    }
}
