using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillItem", menuName = "SkillItem")]
public class SkillItem : ScriptableObject
{
    public string skillName;
    public int cost;
    public Sprite icon;

    // 스킬의 효과를 발동하는 함수
    public virtual void Activate(Unit unit)
    {
        // 스킬별 효과 구현
    }
}

