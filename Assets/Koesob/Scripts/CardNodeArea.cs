using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CardNodeArea : BaseArea
{
    public UnityAction OnCompleted;
    public NodeManager.Node node;

    protected override void OnContain()
    {
        if (!ViewerManager.Instance.PointerObject.TryGetComponent<CardViewer>(out var card)) return;

        node.Card = card.Card;
        card.SetOrigin(rect.localPosition);

        OnCompleted();
        OnCompleted = null;
    }
}
