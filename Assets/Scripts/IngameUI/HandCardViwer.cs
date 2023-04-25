using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class HandCardViwer : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, ISubmitHandler, IPointerEnterHandler, IPointerClickHandler
{
    private enum HandCardState
    {
        OnHand,
        Clicked,
        End,
    }

    HandCardState cur_State;
    RectTransform rect;
    [SerializeField] Vector3 origin;
    HandViewer handViewer;
    int lastHandIndex;

    private void Start()
    {
        cur_State = HandCardState.OnHand;
        rect = GetComponent<RectTransform>();
        handViewer = FindObjectOfType<HandViewer>();
        origin = rect.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("DragStart");
        cur_State = HandCardState.Clicked;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
        //레이캐스팅 후 필드가 아니면 핸드로 다시 이동
        cur_State = HandCardState.OnHand;
        //필드면 드랍 트라이
        Drop();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("EnterMouse");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("Summit");
    }

    void Update()
    {
        if (cur_State == HandCardState.Clicked)
        {
            //마우스 포인터를 따라다니도록 설정
            this.rect.position = Input.mousePosition;
        }
        else if (cur_State == HandCardState.OnHand)
        {
            if (rect.position == origin) return;
            Vector3 direction = origin - rect.localPosition;
            direction = direction.normalized;
            direction *= 0.8f;

            rect.position += direction;

            direction = origin - rect.localPosition;

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (cur_State)
        {
            case HandCardState.OnHand:
                cur_State = HandCardState.Clicked;

                break;
            case HandCardState.Clicked:
                //레이캐스팅 후 필드면 드랍
                //아니면 핸드로
                Drop();
                break;
            case HandCardState.End:

                break;

        }
    }

    public void Drop()
    {

    }
}
