using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gameplay : MonoBehaviour 
{
    [SerializeField] public TextMesh TapToStart;
    [SerializeField] public TextMesh GameOver;
	[SerializeField] public TextMesh Score;
	[SerializeField] public TextMesh Multiplier;
	[SerializeField] public TextMesh Lives;
    [SerializeField] public float DistancePastPlayerForDestruction = 10.0f;
	[SerializeField] public float DistanceToPlayerForLogic = 0.2f;

	private enum State { Init, TapToStart, Game, GameOver };

	private List<Block> mBlocks;
	private DifficultyCurve mDifficulty;
	private Player mPlayer;
	private State mCurrentState;

	public static float GameDeltaTime { get; private set; }
	public static float BlockForce { get { return DifficultyCurve.BlockForce; } }
	public static bool Paused { get; private set; }

	void Awake()
	{
		MouseInput.OnTap += HandleOnTap;
		mBlocks = new List<Block>();
		mDifficulty = GetComponentInChildren<DifficultyCurve>();
		mPlayer = GetComponentInChildren<Player>();
        ChangeState( State.TapToStart );
		Paused = true;
	}

	void Update()
	{
		GameDeltaTime = Paused ? 0.0f : Time.deltaTime;
		mDifficulty.ProcessNextBlock( GameDeltaTime, mPlayer.transform.position );

		if( mDifficulty.NextBlock != null )
		{
			Block ob = mDifficulty.NextBlock;
			mDifficulty.ClearNextBlock();
            ob.gameObject.SetActive(true);
            mBlocks.Add( ob );
		}

		List<Block> old = new List<Block>();

		for( int count = 0; count < mBlocks.Count; count++ )
		{
			Block o = mBlocks[count];
			if( o.transform.position.z < -DistancePastPlayerForDestruction )
			{
				o.Deactivate();
				old.Add( o );
			}
		}

		for( int count = 0; count < old.Count; count++ )
		{
            mBlocks.Remove(old[count]);
		}

		UpdateGUI();

		if( mPlayer.Lives == 0 )
		{
			ChangeState( State.GameOver );
		}
	}

	void OnTriggerEnter( Collider collision )
	{
		Block ob = collision.gameObject.GetComponent<Block>();
		if( ob != null )
		{
			mPlayer.AddScore( ob.Score * mDifficulty.Multiplier ); 
		}
	}

    private void UpdateGUI()
	{
		switch( mCurrentState )
		{
		case State.TapToStart:
			break;
		case State.Game:		
			Score.text = string.Format( "{0:000000}", mPlayer.Score );
			Multiplier.text = string.Format( "{0:0.0}", mDifficulty.Multiplier );
			Lives.text = mPlayer.GetLivesString();
			break;
		case State.GameOver:
			break;
		}
	}

	private void ChangeState( State state )
    {
        if (mCurrentState != state)
        {
            mCurrentState = state;
            switch (state)
            {
                case State.TapToStart:
                    TapToStart.gameObject.SetActive(true);
                    Score.gameObject.SetActive(false);
                    GameOver.gameObject.SetActive(false);
					Multiplier.gameObject.SetActive(false);
					Lives.gameObject.SetActive(false);
					Paused = true;
                    break;
                case State.Game:
                    TapToStart.gameObject.SetActive(false);
                    Score.gameObject.SetActive(true);
                    GameOver.gameObject.SetActive(false);
					Multiplier.gameObject.SetActive(true);
					Lives.gameObject.SetActive(true);
					Paused = false;
					break;
                case State.GameOver:
                    TapToStart.gameObject.SetActive(false);
                    Score.gameObject.SetActive(true);
                    GameOver.gameObject.SetActive(true);
					Multiplier.gameObject.SetActive(false);
					Lives.gameObject.SetActive(false);
					Paused = true;
					break;
            }
        }
    }

	private void Reset()
	{
		for( int count = 0; count < mBlocks.Count; count++ )
		{
			mBlocks[count].Deactivate();
		}

		mBlocks.Clear();

		mPlayer.Reset();
		mDifficulty.Reset();
	}

	private void HandleOnTap( Vector3 position )
	{
		switch( mCurrentState )
		{
		case State.TapToStart:
			Paused = false;
			ChangeState( State.Game );
			break;
		case State.GameOver:
			Reset();
            ChangeState( State.TapToStart );
			break;
		}
	}
}
