using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopSimulator : MonoBehaviour
{
    [SerializeField] private GameObject _ovrRig;
    [SerializeField] private UnoDeckController _deck1;
    [SerializeField] private UnoDeckController _deck2;
    [SerializeField] private UnoCardHandController _hand;
    [SerializeField] private Transform _heldCardParent;

    [Header("Controls")]
    [SerializeField, ReadOnly] private bool _cardInHand;
    [SerializeField, ReadOnly] private UnoCardController _currentCard;

    [Button(Spacing = 10, Mode = ButtonMode.InPlayMode)]
    private void GrabCardFromDeck1() => GrabCardFromDeck(_deck1);

    [Button(Mode = ButtonMode.InPlayMode)]
    private void PlaceCardInDeck1() => PlaceCardInDeck(_deck1);

    [Button(Spacing = 10, Mode = ButtonMode.InPlayMode)]
    private void GrabCardFromDeck2() => GrabCardFromDeck(_deck2);

    [Button(Mode = ButtonMode.InPlayMode)]
    private void PlaceCardInDeck2() => PlaceCardInDeck(_deck2);

    private void Start()
    {
        _ovrRig.SetActive(false);
    }

    private void Update()
    {
        if (_cardInHand)
        {
            var t = _currentCard.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }
    }

    private void GrabCardFromDeck(UnoDeckController deck)
    {
        if (_cardInHand)
        {
            Debug.Log("Already have card in Hand. Cannot Pickup new Card", gameObject);
            return;
        }
        _currentCard = deck.CurrentCard;
        _cardInHand = _currentCard != null;
        if (_cardInHand)
        {
            _currentCard.Pickup();
            _currentCard.transform.SetParent(_heldCardParent);
        }
    }

    private void PlaceCardInDeck(UnoDeckController deck)
    {
        if (!_cardInHand)
        {
            Debug.Log("No Card in Hand to Place in Deck", gameObject);
            return;
        }
        _currentCard.PlaceCardInDeck(deck);
        _currentCard = null;
        _cardInHand = false;
    }

    [Button(Spacing = 10, Mode = ButtonMode.InPlayMode)]
    private void GrabCardFromHand()
    {
        if (_cardInHand)
        {
            Debug.Log("Already have card in Hand. Cannot Pickup new Card", gameObject);
            return;
        }
        _currentCard = _hand.FirstCard;
        _cardInHand = _currentCard != null;
        if (_cardInHand)
        {
            _currentCard.Pickup();
            _currentCard.transform.SetParent(_heldCardParent);
        }
    }
    
    [Button(Mode = ButtonMode.InPlayMode)]
    private void PlaceCardInHand()
    {
        if (!_cardInHand)
        {
            Debug.Log("No Card in Hand to Place in Hand", gameObject);
            return;
        }
        _currentCard.PlaceCardInHand(_hand);
        _currentCard = null;
        _cardInHand = false;
    }
    
    [Button(Spacing = 10, Mode = ButtonMode.InPlayMode)]
    private void DropHeldCard()
    {
        if (!_cardInHand)
        {
            Debug.Log("No Card to Drop", gameObject);
            return;
        }
        _currentCard.Release();
        _currentCard = null;
        _cardInHand = false;
    }
}
