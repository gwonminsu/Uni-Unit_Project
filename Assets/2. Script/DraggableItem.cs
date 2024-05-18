using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewBehaviourScript : MonoBehaviour, IBeginDragHandler, IEndDragHandler,IDragHandler
{
    private Transform canvas; // UI가 소속되어 있는 최상단의 Canvas Transform
    private Transform previousParent; // 해당 오브젝트가 직전에 소속되어 있었던 부모 TransForm
    private RectTransform rect; // UI 위치 제어를 위한 RectTransform
    private CanvasGroup canvasGroup; // UI의 알파값과 상호작용 제어를 위한 CanvasGroup
    /*    public SkillItem skillItem; */

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    /*    public void SetSkillItem(SkillItem item)
        {
            skillItem = item;
            // 여기에서 스킬 아이템의 아이콘 등을 UI에 설정
        }*/

    // 현재 오브젝트를 드래그하기 시작할 때 1회 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        /*rect.SetParent(transform.root); // 드래그 중 최상위로 이동*/
        previousParent = transform.parent; // 드래그 직전에 소속되어 있던 부모 Transform 정보 저장

        // 현재 드래그 중인 UI가 화면의 최상단에 출력되도록 하기 위해
        transform.SetParent(canvas); // 부모 오브젝트를 Canvas로 설정
        transform.SetAsLastSibling(); // 가장 앞에 보이도록 마지막 자식으로 설정

        // 드래그 가능한 오브젝트가 하나가 아닌 자식들을 가지고 있을 수도 있기 때문에 CanvasGroup으로 통제
        // 드래그 중인 오브젝트가 살짝 투명하게 만들기 위해 알파값을 0.6으로 설정하고, 광선 충돌처리가 되지 않도록 한다
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    // 현재 오브젝트를 드래그 중일 때 매 프레임 호출
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position; // 현재 스크린상의 마우스 위치를 UI 위치로 설정 (UI가 마우스를 쫒아다니는 상태)
    }

    // 현재 오브젝트의 드래그를 종료할 때 1회 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        /*        if (!RectTransformUtility.RectangleContainsScreenPoint(HandPanel.instance.GetRectTransform(), Input.mousePosition))
                {
                    ShopManager.instance.ResetItemPosition(this.gameObject); // 원래 위치로 돌아감
                }
                else
                {
                    // HandPanel에 아이템 추가 로직
                }*/

        // 드래그를 시작하면 부모가 canvas로 설정되기 때문에 드래그를 종료할 때 부모가 canvas이면 아이템 슬롯이 아닌 엉뚱한 곳에
        // 드롭을 했다는 뜻이기 때문에 드래그 직전에 소속되어 있던 아이템 슬롯으로 아이템 이동
        if (transform.parent == canvas)
        {
            // 마지막에 소속되어 있었던 previousParent의 자식으로 설정하고, 해당 위치로 설정
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
        }

        // 알파값을 1로 설정하고, 광선 충돌처리가 되도록 한다
        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
