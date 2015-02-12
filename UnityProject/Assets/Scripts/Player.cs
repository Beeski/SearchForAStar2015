using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	[SerializeField] private Color LostLife = Color.red;
    [SerializeField] private float JumpWidth = 2.5f;
	[SerializeField] private float LostLifeTime = 0.25f;

	private MeshRenderer mMeshRenderer;
	private Vector3 mStartingPosition;
	private Color mStartingColour;
	private float mCentreX;
	private float mTargetX;
	private float mStartX;
	private float mMoveTime;
	private float mTimeSinceLifeLost;
	private bool mJumping;

	private const float MoveDuration = 0.5f;

	public float Score { get; private set; }
	public int Lives { get; private set; }

	private const int MAX_LIVES = 3;

	void Awake()
	{
		mMeshRenderer = GetComponentInChildren<MeshRenderer>();
		if( mMeshRenderer != null )
		{
			mStartingColour = mMeshRenderer.material.GetColor( "_Color" );
		}

		mStartingPosition = transform.position;
	}

	void Start() 
	{
		MouseInput.OnSwipe += HandleOnSwipe;
		mCentreX = mStartingPosition.x;
		mTimeSinceLifeLost = LostLifeTime + 1.0f;
		Score = 0.0f;
		Lives = MAX_LIVES;
		mJumping = false;
	}

	void Update() 
	{
        if (mJumping)
		{
			mMoveTime += Gameplay.GameDeltaTime;
			float t = mMoveTime / MoveDuration;
			float x = Mathf.SmoothStep( mStartX, mTargetX, t );
            x = Mathf.Clamp( x, -JumpWidth, JumpWidth );

            mJumping = (mMoveTime < MoveDuration);

            Vector3 position = transform.position;
			position.x = x;
            transform.position = position;
        }

		if( mMeshRenderer != null && mTimeSinceLifeLost <= LostLifeTime )
		{
			mTimeSinceLifeLost += Time.deltaTime;
			float t = mTimeSinceLifeLost / LostLifeTime;
			Color c = Color.Lerp( LostLife, mStartingColour, t );
			mMeshRenderer.material.SetColor( "_Color", c );
		}
	}

	void OnTriggerEnter( Collider collision )
	{
		Block ob = collision.gameObject.GetComponent<Block>();
		if( ob != null )
		{
			ob.DisableScore = true;
			mTimeSinceLifeLost = 0.0f;
			Lives--;
		}
	}

	public void Reset()
	{
		Vector3 position = mStartingPosition;
		transform.position = position;
		mStartX = mTargetX = mCentreX;
		mTimeSinceLifeLost = LostLifeTime + 1.0f;
		Score = 0.0f;
		Lives = MAX_LIVES;
		mJumping = false;
	}

	public void AddScore( float points )
	{
		Score += points;
	}

	public string GetLivesString ()
	{
		string lives = "";
		for( int count = 0; count < MAX_LIVES; count++ )
		{
			lives += Lives > count ? "*" : "X";
		}
		return lives;
	}

	private void HandleOnSwipe( MouseInput.Direction direction )
	{
		switch( direction )
		{
		case MouseInput.Direction.Left:
			JumpLeft();
			break;
		case MouseInput.Direction.Right:
			JumpRight();
			break;
		}
	}

	private void JumpLeft()
	{
		if( !mJumping )
		{
			mStartX = transform.position.x;

			if( mStartX > mCentreX )
			{
				mTargetX = mCentreX;
			}
			else
			{
				mTargetX = mCentreX - JumpWidth;
			}

            mMoveTime = 0.0f;
            mJumping = true;
        }
	}

	private void JumpRight()
	{
		if( !mJumping )
		{
			mStartX = transform.position.x;

			if( mStartX < mCentreX )
			{
				mTargetX = mCentreX;
			}
			else
			{
				mTargetX = mCentreX + JumpWidth;
			}

			mMoveTime = 0.0f;
            mJumping = true;
		}
	}
}
