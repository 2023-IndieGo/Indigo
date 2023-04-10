using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class CardViewer : BaseViewer, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private enum State
    {
        None,
        PlayingCards,
        Node,
        Deck,
        Grave
    }

    private State state = State.None;
    private RectTransform rect;
    private Vector3 origin = new Vector3(0f, 0f, 0f);
    private Card card = null;

    public Vector3 Origin { get => origin; private set => origin = value; }
    public Card Card { get => card; set => card = value; }

    public CardViewer(BaseModel model) : base(model)
    {
    }

    public override void Initialize()
    {
        rect = GetComponent<RectTransform>();
    }


    public override void Refresh()
    {

    }

    public void SetOrigin(Vector3 _origin)
    {
        origin = _origin;
    }

    private void FollowToPointer()
    {
        Vector3 mousePosition = Input.mousePosition;
        rect.position = mousePosition;
    }

    private IEnumerator MoveToOrigin()
    {
        Vector3 direction = origin - rect.localPosition;

        while(direction.magnitude > 1f)
        {
            direction = direction.normalized;
            direction *= 0.8f;

            rect.position += direction;

            direction = origin - rect.localPosition;

            yield return null;
        }
    }

    #region Pointer에 반응하는 메서드

    public void OnBeginDrag(PointerEventData eventData)
    {
        ViewerManager.Instance.PointerObject = this.gameObject;
    }
    public void OnDrag(PointerEventData eventData)
    {
        FollowToPointer();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ViewerManager.Instance.CheckDrop(eventData.position);
        Debug.LogWarning(eventData.position);
        ViewerManager.Instance.PointerObject = null;
        StartCoroutine(MoveToOrigin());
    }


    #endregion
}
