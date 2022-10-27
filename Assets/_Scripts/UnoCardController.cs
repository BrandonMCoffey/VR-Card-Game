using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCardController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private UnoDeckController _deck;

    private bool _inDeck;
    private bool _grabbed;

    private void Update()
    {
        if (_inDeck || _grabbed) _rb.Sleep();
        if (_inDeck)
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }

    public void SetCardSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void SetDeck(UnoDeckController deck)
    {
        _inDeck = true;
        _deck = deck;
    }

    public void Pickup()
    {
        if (_deck) ReleaseFromDeck();
        _grabbed = true;
    }

    public void Release()
    {
        // Test for Nearby Deck
        _grabbed = false;
    }

    private void ReleaseFromDeck()
    {
        _deck.GetNewCard();
        _deck = null;
        transform.SetParent(null);
        _inDeck = false;
    }
}
