using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FieldScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _card_prefab;
    [SerializeField]
    private GameRules _game_rules;
    [SerializeField]
    private CardBundles _card_bundles;
    [SerializeField]
    private RectTransform _grid_panel;
    [SerializeField]
    private TextMeshProUGUI _win_cond_text;
    [SerializeField]
    private DoTweenAnimations _caption_panel_fader;

    private List<CardData> _card_data_shuffle;
    private List<CardScript> _added_cards;
    private List<CardData> _win_cond_cards;
    private CardScript _win_card;
    private CardBundleData _card_bundle;
    private int _difficulty_index;
    


    [SerializeField]
    public UnityEvent OnEndGameHandlers;

    private void Awake()
    {
        if (_card_bundles == null)
            Debug.LogError("No card bundle!");
        if (_card_prefab == null)
            Debug.LogError("No card prefab!");
        if (_game_rules == null)
            Debug.LogError("No game rule!");
        if (_win_cond_text == null)
            Debug.LogError("No win condition object!");
        if (_caption_panel_fader == null)
            Debug.LogError("No caption panel fader script");

        _card_data_shuffle = new List<CardData>();
        _added_cards = new List<CardScript>();
        _win_cond_cards = new List<CardData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameRestart();
    }

    public void GameRestart()
    {
        _difficulty_index = 0;
        _added_cards.Clear();
        _win_cond_cards.Clear();
        _card_bundle = _card_bundles.Bundles.GetRandomElement();
        StartLevel(_difficulty_index);
    }

    private void StartCaptionFade()
    {
        _caption_panel_fader.SetStartValue(0f);
        _caption_panel_fader.FadeIn();
    }

    private void StartLevel(int in_difficulty_index)
    {
        if (_card_bundles == null)
            return;
        if (_game_rules == null)
            return;
        if (in_difficulty_index < 0 || in_difficulty_index >= _game_rules.Rules.Length)
        {
            Debug.LogError("Wrong difficulty");
            return;
        }

        if (_win_cond_cards.Count >= _card_bundle.CardData.Length)
        {
            Debug.LogError("More win condition cards than actual cards");
            return;
        }

        ClearLevel();

        StartCaptionFade();
        CreateCards(in_difficulty_index);

        if (_added_cards.Count == 0)
        {
            Debug.LogError("Nothing was added");
            return;
        }

        SelectWinCard();
    }

    private void SelectWinCard()
    {
        int rnd_ind = UnityEngine.Random.Range(0, _added_cards.Count);
        int loop_prt = _added_cards.Count;
        do
        {
            rnd_ind++;
            if (rnd_ind >= _added_cards.Count)
                rnd_ind = 0;
            _win_card = _added_cards[rnd_ind];
            loop_prt--;

        } while (_win_cond_cards.Contains(_win_card.CardData) && loop_prt > 0);

        if (loop_prt == 0)
            Debug.LogError("All cards were iterated but no was chosen");

        _win_cond_cards.Add(_win_card.CardData);

        if (_win_cond_text != null)
            _win_cond_text.text = _win_card.CardData.Identifier;
    }

    private void CreateCards(int in_difficulty_index)
    {
        _card_data_shuffle.AddRange(_card_bundle.CardData);
        CGameParams rule = _game_rules.Rules[in_difficulty_index];
        int max_card_count = Mathf.Min(rule.CardNumbers, _card_data_shuffle.Count);

        if (_card_data_shuffle.Count < rule.CardNumbers)
            Debug.LogWarning($"Rule require more cards than available: {_card_data_shuffle.Count} < {rule.CardNumbers}");

        _added_cards.Clear();
        for (int i = 0; i < max_card_count; i++)
        {
            CardScript card = CreateCard(_card_data_shuffle);
            if (card == null)
            {
                Debug.LogError("Returned card data is null");
            }
            else
            {
                _added_cards.Add(card);
            }
        }
        _card_data_shuffle.Clear();
    }

    private CardScript CreateCard(List<CardData> in_card_bundle_data)
    {
        if (_card_prefab == null)
            return null;
        if (in_card_bundle_data == null)
        {
            Debug.LogError("Bundle is NULL");
            return null;
        }

        CardData card_data = in_card_bundle_data.GetRandomElement(true);

        GameObject card = Instantiate(_card_prefab, Vector3.zero, Quaternion.identity, _grid_panel);
        card.transform.localPosition = new Vector3(card.transform.localPosition.x, card.transform.localPosition.y, 0);
        card.transform.localScale = Vector3.one;

        CardScript cs = card.GetComponent<CardScript>();
        if(cs == null)
        {
            Debug.LogError("No card component");
            return null;
        }
        cs.Init(card_data);
        cs.OnCardClickHandlers.AddListener(OnCardClick);

        return cs;
    }

    public void OnCardClick(CardScript in_card)
    {
        if (in_card != _win_card)
            in_card.OnWrongCard();
        else
        {
            in_card.OnWinCard();
            StartCoroutine(WaitCardAnimationEnd());
        }
    }

    private IEnumerator WaitCardAnimationEnd()
    {
        bool anim_active = false;
        do
        {
            yield return new WaitForSeconds(0.5f);
            anim_active = _added_cards.Find(card => card.IsAnimationActive());
        } while (anim_active);

        NextLevel();
    }

    private void ClearLevel()
    {
        for(int i = _grid_panel.childCount - 1; i >= 0; i--)
        {
            Destroy(_grid_panel.GetChild(i).gameObject);
        }
           
    }

    private void NextLevel()
    {
        
        _difficulty_index++;
        if(_difficulty_index >= _game_rules.Rules.Length)
        {

            if (OnEndGameHandlers == null)
            {
                Debug.LogError("OnEndGameHandlers is NULL");
            }
            else
            {
                OnEndGameHandlers.Invoke();
            }
        }
        else
        {
            StartLevel(_difficulty_index);
        }
        
    }

}
