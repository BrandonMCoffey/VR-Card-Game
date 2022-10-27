using UnityEngine;

public class UnoCardController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _deckSearchRange = 0.5f;
    [SerializeField] private UnoDeckController _deck;

    public Sprite CardSprite => _renderer.sprite;

    private bool _inDeck;
    private bool _grabbed;
    
    private void Update()
    {
        if (_inDeck || _grabbed)
        {
            _rb.Sleep();
            if (_inDeck)
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

    public void Pickup()
    {
        Debug.Log("Pickup");
        if (_deck) ReleaseFromDeck();
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
                    closestDeck = deck;
                    closestHand = null;
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
            closestDeck.SetTopCard(this);
            return true;
        }
        if (closestHand)
        {
            Debug.Log("Closest Hand Found");
            closestHand.AddCardToHand(this);
            return true;
        }
        return false;
    }

    private void ReleaseFromDeck()
    {
        _deck.GetNewCard();
        _deck = null;
        transform.SetParent(null);
        _inDeck = false;
    }
}
