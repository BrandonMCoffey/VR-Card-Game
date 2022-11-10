using UnityEngine;

public class UnoCardController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _deckSearchRange = 0.5f;
    [SerializeField] private UnoDeckController _deck;
    [SerializeField] private UnoCardHandController _hand;

    [Header("Debug")]
    [SerializeField, ReadOnly] private bool _inDeck;
    [SerializeField, ReadOnly] private bool _inHand;
    [SerializeField, ReadOnly] private bool _grabbed;

    public Sprite CardSprite => _renderer.sprite;
    private int cardValue;
    private int color;

    private bool grabbable = true;
    
    private void Update()
    {
        if (_inDeck || _inHand || _grabbed)
        {
            _rb.Sleep();
            if (_inDeck || _inHand)
            {
                var t = _rb.transform;
                t.localPosition = Vector3.zero;
                t.localRotation = Quaternion.identity;
            }
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

    public void SetValue(int v)
    {
        cardValue = v;
    }

    public void SetColor(int c)
    {
        color = c;
    }

    public int GetValue()
    {
        return cardValue;
    }

    public int GetColor()
    {
        return color;
    }

    public void Pickup()
    {
        Debug.Log("Pickup");
        if (_deck) ReleaseFromDeck();
        if (_hand) ReleaseFromHand();
        _grabbed = true;
    }

    public void Release()
    {
        Debug.Log("Release");
        _grabbed = false;
        _rb.WakeUp();
        AddToNearbySockets();
    }

    private bool AddToNearbySockets()
    {
        var pos = _rb.transform.position;
        var colliders = Physics.OverlapSphere(pos, _deckSearchRange);
        if (colliders.Length == 0) return false;
        UnoDeckController closestDeck = null;
        UnoCardHandController closestHand = null;
        float dist = _deckSearchRange * 2;
        foreach (Collider col in colliders)
        {
            var root = col.transform.root;
            if (Vector3.Distance(pos, root.position) < dist)
            {
                var deck = root.GetComponent<UnoDeckController>();
                if (deck)
                {
                    if (deck.IsDiscard() & !deck.IsEmpty())
                    {
                        if(deck.CurrentCard.GetColor() == 5 || color == 5 || deck.CurrentCard.GetColor() == color || deck.CurrentCard.GetValue() == cardValue)
                        {
                            closestDeck = deck;
                            closestHand = null;
                        }
                    }
                    else
                    {
                        closestDeck = deck;
                        closestHand = null;
                    }
                }
                var hand = root.GetComponent<UnoCardHandController>();
                if (hand)
                {
                    closestHand = hand;
                    closestDeck = null;
                }
            }
        }
        if (closestDeck)
        {
            Debug.Log("Closest Deck Found");
            PlaceCardInDeck(closestDeck);
            return true;
        }
        if (closestHand)
        {
            Debug.Log("Closest Hand Found");
            PlaceCardInHand(closestHand);
            return true;
        }
        return false;
    }

    public void PlaceCardInDeck(UnoDeckController deck)
    {
        deck.SetTopCard(this);
        _deck = deck;
        _inDeck = true;
    }

    private void ReleaseFromDeck()
    {
        _deck.GetNewCard();
        _deck = null;
        transform.SetParent(null);
        _inDeck = false;
    }

    public void PlaceCardInHand(UnoCardHandController hand)
    {
        hand.AddCardToHand(this);
        _hand = hand;
        _inHand = true;
    }

    private void ReleaseFromHand()
    {
        _hand.ReleaseCardFromHand(this);
        _hand = null;
        transform.SetParent(null);
        _inHand = false;
    }

    public void SetGrabbable(bool g)
    {
        // TODO: Set Grabbable on child components
        grabbable = g;
    }

    public bool GetGrabble()
    {
        return grabbable;
    }
}
