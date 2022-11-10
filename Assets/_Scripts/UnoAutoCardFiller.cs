using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnoAutoCardFiller : MonoBehaviour
{
	[SerializeField] private UnoDeckController _deckToGrabFrom;
	[SerializeField] private UnoDeckController _deckToPlaceIn;
	[SerializeField] private bool _fillAfterOtherPlaced;
	[SerializeField] private float _fillAfterOtherPlacedtime = 0.5f;
		
	private void OnEnable()
	{
		_deckToPlaceIn.OnCardPlaced += AutoFillAfterOtherPlaced;
	}
	
	private void OnDisable()
	{
		_deckToPlaceIn.OnCardPlaced -= AutoFillAfterOtherPlaced;
	}
	
	[Button]
	public void FillIfEmpty()
	{
		if (_deckToPlaceIn.CurrentCard == null) Fill();
	}
	
	public void AutoFillAfterOtherPlaced()
	{
		if (_fillAfterOtherPlaced)
		{
			StartCoroutine(AutoFillAfterOtherPlacedRoutine());
		}
	}
	
	public IEnumerator AutoFillAfterOtherPlacedRoutine()
	{
		yield return new WaitForSeconds(_fillAfterOtherPlacedtime);
		Fill();
	}
	
	[Button]
	public void Fill()
	{
		_deckToPlaceIn.SetTopCard(_deckToGrabFrom.CurrentCard, false);
		_deckToGrabFrom.GetNewCard();
	}
}
