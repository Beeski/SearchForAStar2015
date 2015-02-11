using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour 
{
	[SerializeField] private float ScoreForAvoid = 2.0f;

    private Quaternion mStartingRotation;
	private Vector3 mStartingPosition;

	public float Score { get { return ScoreForAvoid; } set { ScoreForAvoid = value; } }
    public bool InContactWithFloor { get; set; }

    void Awake()
    {
        mStartingPosition = transform.position;
		mStartingRotation = transform.rotation;
		InContactWithFloor = false;
    }

	void Update()
	{
        if( InContactWithFloor )
        {
            rigidbody.AddForce(Vector3.back * Gameplay.BlockForce);
        }
	}

	public void Deactivate()
	{
		transform.position = mStartingPosition;
		transform.rotation = mStartingRotation;
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		gameObject.SetActive( false );
		InContactWithFloor = false;
	}
}
