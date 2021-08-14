using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RestartScript : MonoBehaviour
{
    // Start is called before the first frame update
    //private Image _img_bkg;
    private CanvasGroup _cnv_grp;
    [SerializeField]
    private ProgressBarScript _prg_bar_scr;
    [SerializeField]
    private float _load_animation_steps;
    [SerializeField]
    private GameObject _restart_btn;
    [SerializeField]
    private DoTweenAnimations _endgame_panel_fader;
    [SerializeField]
    private DoTweenAnimations _progers_bar_fader;
    [SerializeField]
    public UnityEvent OnEndLoadingHandlers;

    void Start()
    {
        transform.SetAsFirstSibling();

        _cnv_grp = GetComponent<CanvasGroup>();
        if (_prg_bar_scr == null)
            Debug.LogError("No progress bar script");
        if(_load_animation_steps <= 0)
        {
            _load_animation_steps = 30;
        }
        if (_endgame_panel_fader == null)
            Debug.LogError("No endgame panel animation script");
        if (_progers_bar_fader == null)
            Debug.LogError("No progress bar animator");

    }

    public void OnRestartClick()
    {
       
        if (_prg_bar_scr != null)
        {   
            StartCoroutine(LoadingBarUpdate(_load_animation_steps));
        }
            
        _restart_btn.SetActive(false);

        if (_progers_bar_fader != null)
            _progers_bar_fader.FadeIn();
    }

    public void OnEndGame()
    {
        _restart_btn.SetActive(true);

        if(_progers_bar_fader != null)
            _progers_bar_fader.SetStartValue(0);

        transform.SetAsLastSibling();
        if (_endgame_panel_fader != null)
        {
            _endgame_panel_fader.SetStartValue(0f);
            _endgame_panel_fader.FadeIn();
        }
    }

    private IEnumerator LoadingBarUpdate(float in_steps)
    {
        _prg_bar_scr.SetProgressBar(0f);
        float time_step = 0.05f;

        for (int i = 0; i <= in_steps; i++)
        {
            yield return new WaitForSeconds(time_step);
            float percent = (float)i / in_steps;
            _prg_bar_scr.SetProgressBar(percent);
        }

        EndLoading();

    }

    private void EndLoading()
    {
        if(OnEndLoadingHandlers != null)
            OnEndLoadingHandlers.Invoke();
        transform.SetAsFirstSibling();
    }
}
