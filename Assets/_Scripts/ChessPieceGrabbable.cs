using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPieceGrabbable : MonoBehaviour
{
	[SerializeField] private Rigidbody _rb;
	[SerializeField] private ChessBoardSocket _socket;
	[SerializeField] private float _socketSearchRange = 0.1f;
	
	[SerializeField, ReadOnly] private bool _inSocket;
	[SerializeField, ReadOnly] private bool _grabbed;
	
	public void SetSocket(ChessBoardSocket socket) => _socket = socket;
	
	private void Start()
	{
		_inSocket = _socket != null;
	}
	
	private void Update()
	{
		if (_inSocket || _grabbed)
		{
			_rb.Sleep();
			if (_inSocket)
			{
				var t = _rb.transform;
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
			}
		}
	}
    
	[Button]
	public void Pickup()
	{
		Debug.Log("Pickup");
		if (_socket) ReleaseFromSocket();
		_grabbed = true;
	}

	[Button]
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
		var colliders = Physics.OverlapSphere(pos, _socketSearchRange);
		if (colliders.Length == 0) return false;
		ChessBoardSocket closestSocket = null;
		float dist = _socketSearchRange * 2;
		foreach (Collider col in colliders)
		{
			var colDist = Vector3.Distance(pos, col.transform.position);
			if (colDist < dist)
			{
				var socket = col.GetComponent<ChessBoardSocket>();
				if (socket && socket.CanPlacePiece) {
					closestSocket = socket;
					dist = colDist;
				}
			}
		}
		if (closestSocket)
		{
			Debug.Log("Closest Socket Found");
			PlaceInSocket(closestSocket);
			return true;
		}
		return false;
	}

	private void PlaceInSocket(ChessBoardSocket socket)
	{
		socket.SetPiece(this);
		transform.SetParent(socket.transform);
		transform.localPosition = Vector3.zero;
		_socket = socket;
		_inSocket = true;
	}

	private void ReleaseFromSocket()
	{
		_socket = null;
		transform.SetParent(null);
		_inSocket = false;
	}
}
