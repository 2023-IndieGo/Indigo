using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseArea : MonoBehaviour
{
    protected RectTransform rect;
    private void Awake()
    {
        if (!TryGetComponent(out rect))
        {
            rect = gameObject.AddComponent<RectTransform>();
        }
    }
    private void Start()
    {
        ViewerManager.Instance.CheckDrop -= CheckDrop;
        ViewerManager.Instance.CheckDrop += CheckDrop;
    }

    private void CheckDrop(Vector2 _pointerPosition)
    {
        if (IsPointerInRectangle(_pointerPosition))
        {
            OnContain();
        }

    }

    private bool IsPointerInRectangle(Vector2 _pointerPosition)
    {
        if (transform.position.x - rect.rect.width / 2 > _pointerPosition.x) return false;
        if (transform.position.x + rect.rect.width / 2 < _pointerPosition.x) return false;
        if (transform.position.y - rect.rect.height / 2 > _pointerPosition.y) return false;
        if (transform.position.y + rect.rect.height / 2 < _pointerPosition.y) return false;

        return true;
    }

    protected abstract void OnContain();
}
