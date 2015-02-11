using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour 
{
	private bool [] mRows;

	void Update()
	{
		Vector3 position = transform.position;
		position.z -= Gameplay.GameDeltaTime * Gameplay.BlockForce;
		transform.position = position;
	}

	public void SetLogic( bool[] rows )
	{
		mRows = rows;
	}

	public bool CollideWithPosition( int Position )
	{
		return mRows[Position];
	}
}
