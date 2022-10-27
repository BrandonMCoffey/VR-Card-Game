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
    private int _totalCards;

    private void Start()
    {
        _totalCards = _cards.Count;
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
        if (_cards.Count == 0)
        {
            _deckVisual.gameObject.SetActive(false);
            return;
        }
        _currentCard = Instantiate(_cardPrefab, _cardParent);
        _currentCard.SetDeck(this);
        _currentCard.SetCardSprite(_cards[0]);
        _cards.RemoveAt(0);
        _deckVisual.localScale = new Vector3(1, (float)_cards.Count / _totalCards, 1);
    }
}
