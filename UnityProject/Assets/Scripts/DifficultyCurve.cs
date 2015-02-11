using UnityEngine;
using System;
using System.Collections;

public class DifficultyCurve : MonoBehaviour 
{
	[Serializable]
	public class DifficultyLevel
	{
		public string Name;
		public float TimeBetweenObstacles;
		public float BlockForce;
		public float Multiplier;
	}

	[SerializeField] private DifficultyLevel [] mLevels;
	[SerializeField] private float BlockForceRamp = 0.1f;
	[SerializeField] private float HardTime = 100.0f;

	private float mTimeToNextObsticale;
	private float mTimePlaying;
	private int mNextDifficulty;

	public Block NextBlock { get; private set; }
	public float Multiplier { get; private set; }
	public static float BlockForce { get; private set; }

	void Awake()
	{
		Reset();
	}

	public void ProcessNextBlock( float dt, Vector3 target )
	{
		mTimePlaying += dt;

		if( NextBlock == null )
		{
			mTimeToNextObsticale -= dt;
			if( mTimeToNextObsticale < 0.0f )
			{
				mTimeToNextObsticale = mLevels[mNextDifficulty].TimeBetweenObstacles;

				Block o = BlockFactory.Create();
				Vector3 pos = o.transform.position;
				pos.x = target.x;
				o.transform.position = pos;
				NextBlock = o;

				float diff = mTimePlaying / HardTime;
				mNextDifficulty = (int)( diff * (float)mLevels.Length );
				mNextDifficulty = Mathf.Clamp( mNextDifficulty, 0, mLevels.Length - 1 );
				BlockForce = mLevels[mNextDifficulty].BlockForce;
				Multiplier = mLevels[mNextDifficulty].Multiplier;
			}
		}
	}

	public void Reset()
	{
		BlockForce = mLevels[0].BlockForce;
		mTimeToNextObsticale = 0.0f;
		NextBlock = null;
		mTimePlaying = 0.0f;
		Multiplier = 1.0f;
		mNextDifficulty = 0;
	}

	public void ClearNextBlock()
	{
		NextBlock = null;
	}
}
