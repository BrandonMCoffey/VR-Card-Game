using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoDeckController : MonoBehaviour
{
    [SerializeField] private Transform _cardParent;
    [SerializeField] private UnoCardController _cardPrefab;
    [SerializeField] private Transform _deckVisual;
    [SerializeField] private List<Sprite> _cards;

    private UnoCardController _currentCard;
    private const int TotalCards = 60;

    private void Start()
    {
        ShuffleDeck();
        GetNewCard();
    }

    private void ShuffleDeck()
    {
        var count = _cards.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            (_cards[i], _cards[r]) = (_cards[r], _cards[i]);
        }
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
        _cards.RemoveAt(_cards.Count - 1);
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
        _currentCard.SetDeck(this);
        _currentCard.SetCardSprite(card.CardSprite);
        Destroy(card.gameObject);
    }

    private void UpdateDeckSize()
    {
        var y = Mathf.Clamp01((_cards.Count + 1) / Mathf.Max(1f, TotalCards));
        _deckVisual.localScale = new Vector3(1, y, 1);
    }
}
