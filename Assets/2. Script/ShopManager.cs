using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance; // 싱글톤 인스턴스 선언

    public GameObject shopItemPrefab; // 상점 아이템 슬롯 프리팹
    public Transform shopPanel;       // 상점 패널
    public int numberOfSlots = 5;     // 상점 슬롯의 수
    public List<SkillItem> availableItems; // 판매할 스킬 아이템 목록

    private List<GameObject> slots = new List<GameObject>(); // 생성된 슬롯의 리스트

    void Start()
    {
        PopulateShop();
    }

    void PopulateShop()
    {
/*        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject slot = Instantiate(shopItemPrefab, shopPanel);
            SkillSlot skillSlot = slot.GetComponent<SkillSlot>();
            if (skillSlot != null)
            {
                SkillItem randomItem = GetRandomItem();
                skillSlot.SetSkillItem(randomItem); // 스킬 아이템 설정
                slots.Add(slot);
            }
        }*/
    }

    SkillItem GetRandomItem()
    {
        if (availableItems.Count == 0) return null;
        int index = Random.Range(0, availableItems.Count);
        return availableItems[index];
    }

    public void ResetItemPosition(GameObject item)
    {
        item.transform.SetParent(shopPanel);
        item.transform.localPosition = Vector3.zero; // 원래 위치로 리셋
    }
}