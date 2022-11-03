using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoCardHandController : MonoBehaviour
{
    [SerializeField] private float _handOffset = 0.25f;
    [SerializeField] private float _cardOffset = 10f;

    [SerializeField, ReadOnly] private List<UnoCardController> _controllers = new List<UnoCardController>();

    public UnoCardController FirstCard => _controllers.Count > 0 ? _controllers[0] : null;

    public void AddCardToHand(UnoCardController card)
    {
        // Add to Hand
        _controllers.Add(card);

        //_controllers[_controllers.Count - 1].SetCardSprite(card.CardSprite);
        //_controllers[_controllers.Count - 1].SetGrabbable(false);

        //Destroy(card.gameObject);
    }

    public void ReleaseCardFromHand(UnoCardController card)
    {
        if (_controllers.Contains(card))
        {
            _controllers.Remove(card);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _controllers.Count; i++)
        {
            var t = _controllers[i].transform;
            var pos = transform.position;
            t.position = new Vector3(pos.x, pos.y + _handOffset, pos.z);
            t.RotateAround(pos, Vector3.forward, (i * _cardOffset) - (_cardOffset * (_controllers.Count - 1f / 2f)));
            t.rotation = Quaternion.identity;
        }
    }
}
