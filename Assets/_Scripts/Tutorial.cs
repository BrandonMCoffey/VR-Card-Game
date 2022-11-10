using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
	[SerializeField] private UnoDeckController _deckToGrabFrom;
	[SerializeField] private UnoCardHandController _hand;
	[SerializeField] private UnoDeckController _deckToPlaceIn;
	[SerializeField] private UnoAutoCardFiller _autoFiller;
	
	[SerializeField] private GameObject _welcome;
	[SerializeField] private GameObject _uno;
	[SerializeField] private GameObject drawCard;
	[SerializeField] private GameObject _placeInHand;
	[SerializeField] private GameObject _continueDrawing;
	[SerializeField] private GameObject _beginPlay;
	[SerializeField] private GameObject _end;
	
	private bool _grabbedFromDeck;
	private bool _placedInHand;
	private bool _placedInDeck;
	
	private void Start()
	{
		_welcome.SetActive(true);
		_uno.SetActive(false);
		drawCard.SetActive(false);
		_placeInHand.SetActive(false);
		_continueDrawing.SetActive(false);
		_beginPlay.SetActive(false);
		
		StartCoroutine(TutorialRoutine());
	}
	
	private IEnumerator TutorialRoutine()
	{
		yield return new WaitForSeconds(10);
		
		_welcome.SetActive(false);
		_uno.SetActive(true);
		
		yield return new WaitForSeconds(4);
		
		_uno.SetActive(false);
		drawCard.SetActive(true);
		
		_deckToGrabFrom.OnGrabCard += SetGrabbedTrue;
		yield return new WaitUntil(() => _grabbedFromDeck);
		_deckToGrabFrom.OnGrabCard -= SetGrabbedTrue;
		
		drawCard.SetActive(false);
		_placeInHand.SetActive(true);
		
		_hand.OnPlaceCard += SetPlacedHandTrue;
		yield return new WaitUntil(() => _placedInHand);
		_hand.OnPlaceCard -= SetPlacedHandTrue;
		
		_placeInHand.SetActive(false);
		_continueDrawing.SetActive(true);
		
		yield return new WaitForSeconds(6);
		
		_continueDrawing.SetActive(false);
		_beginPlay.SetActive(true);
		
		_autoFiller.FillIfEmpty();
		
		_deckToPlaceIn.OnCardPlaced += SetPlacedDeckTrue;
		yield return new WaitUntil(() => _placedInDeck);
		_deckToPlaceIn.OnCardPlaced -= SetPlacedDeckTrue;
		
		_beginPlay.SetActive(false);
		_end.SetActive(true);
	}
	
	private void SetGrabbedTrue() => _grabbedFromDeck = true;
	private void SetPlacedHandTrue() => _placedInHand = true;
	private void SetPlacedDeckTrue() => _placedInDeck = true;
}
