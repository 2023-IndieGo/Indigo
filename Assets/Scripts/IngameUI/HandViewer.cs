using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class HandViewer : MonoBehaviour, IBeginDragHandler, IDragHandler, IDropHandler, ISubmitHandler, IPointerEnterHandler
{
    public List<GameObject> cardList;
    [OnValueChanged("OnCardPostionAndRotateSet")]
    public float spacing = 50f;
    [OnValueChanged("OnCardPostionAndRotateSet")]
    public float rotationScale = 0.1f;
    [OnValueChanged("OnCardPostionAndRotateSet")]
    public float maxRotation = 20f;
    [OnValueChanged("OnCardPostionAndRotateSet")]
    public float arcPosY = 0.7f;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("DragStart");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("EnterMouse");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        Debug.Log("Summit");
    }

    void OnCardPostionAndRotateSet()
    {
        var Rect = GetComponent<RectTransform>();
        cardList = new List<GameObject>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            cardList.Add(this.transform.GetChild(i).gameObject);
        }
        float centerIndex = (cardList.Count - 1) / 2f;
        for (int i = 0; i < cardList.Count; i++)
        {
            RectTransform rt = cardList[i].GetComponent<RectTransform>();
            float x = (i - centerIndex) * spacing;
            rt.anchoredPosition = new Vector2(Rect.sizeDelta.x/2 + x , 0f);

            Image image = cardList[i].GetComponent<Image>();
            float angle = i - centerIndex;
            angle *= rotationScale;
            angle = Mathf.Clamp(angle, -maxRotation, maxRotation);
            image.rectTransform.localRotation = Quaternion.Euler(0f, 0f, angle);
            rt.anchoredPosition += new Vector2(0, Mathf.Abs(x-i)*arcPosY*-1);
        }
    }

    public (Vector2 pos, Vector3 eulerAngle) GetCardOriginPosAndEuler(int index)
    {

        return (new Vector2(0, 0), new Vector3(0, 0, 0));
    }
}





