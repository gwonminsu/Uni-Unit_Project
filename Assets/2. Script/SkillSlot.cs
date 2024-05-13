using UnityEngine;
using UnityEngine.EventSystems;

public class SkillSlot : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private RectTransform rectTransform;
    public SkillItem skillItem;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetSkillItem(SkillItem item)
    {
        skillItem = item;
        // 여기에서 스킬 아이템의 아이콘 등을 UI에 설정
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rectTransform.SetParent(transform.root); // 드래그 중 최상위로 이동
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(HandPanel.instance.GetRectTransform(), Input.mousePosition))
        {
            ShopManager.instance.ResetItemPosition(this.gameObject); // 원래 위치로 돌아감
        }
        else
        {
            // HandPanel에 아이템 추가 로직
        }
    }
}
