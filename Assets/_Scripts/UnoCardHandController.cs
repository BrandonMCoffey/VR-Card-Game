using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCardHandController : MonoBehaviour
{
    [SerializeField] private List<Sprite> _cards;
    [SerializeField] private float _cardOffset = 10f;
    private List<UnoCardController> _controllers;
    
    public void AddCardToHand(UnoCardController card)
    {
        // Add to Hand
        _cards.Add(card.CardSprite);
        _controllers.Add(new UnoCardController());
        _controllers[_controllers.Count - 1].SetCardSprite(card.CardSprite);
        _controllers[_controllers.Count - 1].SetGrabbable(false);
        
        //Destroy(card.gameObject);
    }

    private void Update()
    {
        for(int i = 0; i < _controllers.Count; i++)
        {
            _controllers[i].transform.position.Set(transform.position.x, transform.position.y + 1f, transform.position.z);
            _controllers[i].transform.RotateAround(transform.position, Vector3.forward, (i * _cardOffset) - (_cardOffset * (_controllers.Count - 1f / 2f)));
        }
    }
}
