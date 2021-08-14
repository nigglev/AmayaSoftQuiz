using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class DoTweenAnimations : MonoBehaviour
{
    private Tween _fade_tween;
    private Sequence _sequence;

    [SerializeField]
    private CanvasGroup _cnv_grp;
    [SerializeField]
    public UnityEvent OnFadeEnd;
    [SerializeField]
    private float _fade_duration;

    [SerializeField]
    private int EBCount;
    [SerializeField]
    private float EBAmpl;
    [SerializeField]
    private float EBTime;

    private Transform _tr;

    private void Awake()
    {
        _tr = transform;
    }

    public bool IsAnimActive()
    {
        return _fade_tween != null && _fade_tween.IsActive()
            || _sequence != null && _sequence.IsActive();
    }

    private void OnAnimComplete()
    {
        _fade_tween = null;
        _sequence = null;

        if (OnFadeEnd != null)
            OnFadeEnd.Invoke();
    }


    public void FadeIn(float in_duration)
    {
        Fade(1f, in_duration);
    }

    public void FadeOut(float in_duration)
    {
        Fade(0f, in_duration);
    }

    public void FadeIn()
    {
        FadeIn(_fade_duration);
    }

    public void FadeOut()
    {
        FadeOut(_fade_duration);
    }

    private void Fade(float in_end_value, float in_duration)
    {
        if (IsAnimActive())
        {
            return;
        }

        if (_cnv_grp == null)
        {
            Debug.LogError("No canvas group");
            return;
        }

        _fade_tween = _cnv_grp.DOFade(in_end_value, in_duration);
        _fade_tween.OnComplete(OnAnimComplete);
    }

    public void SetStartValue(float in_value)
    {
        if (_cnv_grp == null)
        {
            Debug.LogError("No canvas group");
            return;
        }
        _cnv_grp.alpha = in_value;
    }

    public void Bouncing()
    {
        if (IsAnimActive())
        {
            return;
        }

        float time1 = 1f;
        float time2 = 0.3f;

        _sequence = DOTween.Sequence();

        _tr.localScale = Vector3.zero;
        Vector3 step1 = new Vector3(1.05f, 1.05f, 1);
        Vector3 step2 = new Vector3(0.95f, 0.95f, 1);

        _sequence.Append(_tr.DOScale(step1, time1));
        _sequence.Append(_tr.DOScale(step2, time2));
        _sequence.Append(_tr.DOScale(Vector3.one, time2));
        _sequence.PlayForward();
        _sequence.onComplete += OnAnimComplete;
    }

    public void EaseInBouncing()
    {
        if (IsAnimActive())
        {
            return;
        }

        _sequence = DOTween.Sequence();

        float ypos = _tr.position.y;

        for (int i = 0; i < EBCount; i++)
        {
            _sequence.Append(_tr.DOMoveY(ypos + EBAmpl, EBTime));
            _sequence.Append(_tr.DOMoveY(ypos - EBAmpl, EBTime));
        }
        _sequence.Append(_tr.DOMoveY(ypos, EBTime));

        _sequence.PlayForward();
        _sequence.onComplete += OnAnimComplete;
    }
}
