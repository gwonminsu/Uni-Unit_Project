using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillItem", menuName = "SkillItem")]
public class SkillItem : ScriptableObject
{
    public string skillName;
    public int cost;
    public Sprite icon;

    // ��ų�� ȿ���� �ߵ��ϴ� �Լ�
    public virtual void Activate(Unit unit)
    {
        // ��ų�� ȿ�� ����
    }
}

