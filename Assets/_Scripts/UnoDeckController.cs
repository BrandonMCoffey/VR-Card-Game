using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoDeckController : MonoBehaviour
{
    [SerializeField] private Transform _cardParent;
    [SerializeField] private UnoCardController _cardPrefab;
    [SerializeField] private Transform _deckVisual;
    [SerializeField] private List<Sprite> _cards;
    [SerializeField] private bool discardPile = false;

    struct unoData{
        public int value;
        public int color;
    }

    private List<unoData> _data = new List<unoData>();

    [Header("Debug")]
    [SerializeField, ReadOnly] private UnoCardController _currentCard;

    public UnoCardController CurrentCard => _currentCard;

    private const int TotalCards = 60;

    private void Start()
    {
        // Why?
        Random.InitState(System.DateTime.Now.Millisecond);
        GiveCardsValuesAndColors();
        GetNewCard();
        ShuffleDeck();
        Debug.Log("End Start");
    }

    private void GiveCardsValuesAndColors()
    {
        for(var i = 0; i < _cards.Count; i++)
        {
            unoData ud = new unoData();
            
            if(i < 10)
            {
                ud.value = i;
                ud.color = 1;
            }
            else if(i < 20)
            {
                ud.value = i%10;
                ud.color = 2;
            }
            else if (i < 30)
            {
                ud.value = i % 10;
                ud.color = 3;
            }
            else if (i < 40)
            {
                ud.value = i % 10;
                ud.color = 4;
            }
            else if(i < 43)
            {
                ud.value = i - 40 + 10;
                ud.color = 1;
            }
            else if (i < 46)
            {
                ud.value = i - 43 + 10;
                ud.color = 2;
            }
            else if (i < 49)
            {
                ud.value = i - 46 + 10;
                ud.color = 3;
            }
            else if (i < 52)
            {
                ud.value = i - 49 + 10;
                ud.color = 4;
            }
            else
            {
                ud.value = 0;
                ud.color = 5;
            }
            _data.Add(ud);
        }
    }

    [Button(Spacing = 10, Mode = ButtonMode.InPlayMode)]
    private void ShuffleDeck()
    {
        if (_cards.Count == 0) return;
        var count = _cards.Count;
        for (var i = 0; i < count - 1; ++i)
        {
            var r = Random.Range(i, count);
            (_cards[i], _cards[r]) = (_cards[r], _cards[i]);
            (_data[i], _data[r]) = (_data[r], _data[i]);
        }
        
        // Randomize Top Card
        var rand = Random.Range(0, _cards.Count);
        var s = _currentCard.CardSprite;
        _currentCard.SetCardSprite(_cards[rand]);
        _cards[rand] = s;
        _currentCard.SetColor(_data[rand].color);
        _currentCard.SetValue(_data[rand].value);
    }

    public void GetNewCard()
    {
        UpdateDeckSize();
        if (_cards.Count == 0)
        {
            _currentCard = null;
            return;
        }
        _currentCard = Instantiate(_cardPrefab, _cardParent);
        _currentCard.SetDeck(this);
        _currentCard.SetCardSprite(_cards[^1]);
        _currentCard.SetColor(_data[^1].color);
        _currentCard.SetValue(_data[^1].value);
        _cards.RemoveAt(_cards.Count - 1);
        _data.RemoveAt(_data.Count - 1);
    }

    public void SetTopCard(UnoCardController card)
    {
        if (_currentCard)
        {
            _cards.Add(_currentCard.CardSprite);
            UpdateDeckSize();
        }
        else
        {
            _currentCard = Instantiate(_cardPrefab, _cardParent);
        }
        unoData ud = new unoData();
        ud.color = card.GetColor();
        ud.value = card.GetValue();
        _data.Add(ud);
        _currentCard.SetDeck(this);
        _currentCard.SetCardSprite(card.CardSprite);
        Destroy(card.gameObject);
    }

    private void UpdateDeckSize()
    {
        var y = Mathf.Clamp01((_cards.Count + 1) / Mathf.Max(1f, TotalCards));
        _deckVisual.localScale = new Vector3(1, y, 1);
    }

    public bool IsDiscard()
    {
        return discardPile;
    }

    public bool IsEmpty()
    {
        return _cards.Count == 0;
    }
}
