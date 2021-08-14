using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class OnCardClickEvent : UnityEvent<CardScript>
{
}

public class CardScript : MonoBehaviour
{
    private CardData _card_data;
    public CardData CardData => _card_data;

    [SerializeField]
    public OnCardClickEvent OnCardClickHandlers;

    [SerializeField]
    private Image _image;

    [SerializeField]
    private DoTweenAnimations _card_animator;
    [SerializeField]
    private DoTweenAnimations _card_inside_animator;


    public void Init(CardData inData)
    {
        if (inData == null)
        {
            Debug.LogError("Init data for Card is invald!");
            return;
        }

        _card_data = inData;

        if(_image == null)
        {
            Debug.LogError("Image wasn't setted!");
            return;
        }

        if (_card_data.Sprite == null)
        {
            Debug.LogError($"Sprite for {_card_data.Identifier} not found!!");
            return;
        }

        if (_card_data.Identifier == null)
        {
            Debug.LogError("Win card doesn't have identifier");
            return;
        }

        _image.sprite = _card_data.Sprite;
        _image.transform.rotation = Quaternion.Euler(0, 0, _card_data.RotateDegree);

        if (OnCardClickHandlers == null)
            OnCardClickHandlers = new OnCardClickEvent();
    }


    // Start is called before the first frame update
    void Start()
    {
        if (_card_animator != null)
        {
            _card_animator.Bouncing();
        }
    }


    public void OnClick()
    {
        //Debug.Log($"ONCLICK {_card_data.Identifier}");
        //SendMessageUpwards("OnCardClick", _card_data, SendMessageOptions.RequireReceiver);
        if (OnCardClickHandlers == null)
            return;
        OnCardClickHandlers.Invoke(this);
    }

    public void OnWrongCard()
    {

        if (_card_inside_animator != null)
        {
            _card_inside_animator.EaseInBouncing();
        }
    }

    public void OnWinCard()
    {

        if (_card_inside_animator != null)
        {
            _card_inside_animator.Bouncing();
        }
    }

    public bool IsAnimationActive()
    {
        return _card_inside_animator != null && _card_inside_animator.IsAnimActive()
            || _card_animator != null && _card_animator.IsAnimActive();
    }
}
