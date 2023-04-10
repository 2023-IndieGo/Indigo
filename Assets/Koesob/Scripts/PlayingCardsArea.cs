using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayingCardsArea : BaseArea
{
    protected override void OnContain()
    {
        Debug.LogWarning("PlayingCardArea Cotain");
        if (!ViewerManager.Instance.PointerObject.TryGetComponent<CardViewer>(out var card)) return;

        card.SetOrigin(rect.localPosition);
    }
}
