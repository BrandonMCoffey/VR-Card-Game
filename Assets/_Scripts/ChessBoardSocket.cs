using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardSocket : MonoBehaviour
{
	[SerializeField, ReadOnly] private ChessPieceGrabbable _piece;
	
	public bool CanPlacePiece => _piece == null;
	
	public void SetPiece(ChessPieceGrabbable piece)
	{
		_piece = piece;
	}
	
	private void OnValidate()
	{
		_piece = GetComponentInChildren<ChessPieceGrabbable>();
		if (_piece) _piece.SetSocket(this);
	}
}
