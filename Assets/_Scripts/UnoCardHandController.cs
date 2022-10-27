using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCardHandController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _cards; 
    
    public void AddCardToHand(UnoCardController card)
    {
        // Add to Hand
        _cards.Add(card.CardSprite);
        
        Destroy(card.gameObject);
    }
}
