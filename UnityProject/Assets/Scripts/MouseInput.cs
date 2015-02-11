using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour 
{
	public enum Direction { Up, Down, Left, Right };
	
	public delegate void OnTapCallback( Vector3 position );
	public delegate void OnSwipeCallback( Direction direction );
	
	public static event OnTapCallback OnTap;
	public static event OnSwipeCallback OnSwipe;

	private enum MouseButtons
	{
		Left,
		Right,
		Middle,

		NumMouseButtons
	};

	private const int kNumMouseButtons = (int)MouseButtons.NumMouseButtons;

	private Vector3 mStartingPosition;
	private float mStartingTime;
	private bool [] mMouseButton;
	private bool [] mMouseButtonLast;
	private bool mPotentialTap;
	private bool mPotentialSwipe;

	private const float TapMoveThreshold = 50.0f;
	private const float TapDuration = 0.5f;
	private const float SwipeDuration = 1.0f;

	void Start () 
	{
		mMouseButton = new bool[kNumMouseButtons];
		mMouseButtonLast = new bool[kNumMouseButtons];
		for( int count = 0; count < kNumMouseButtons; count++ )
		{
			mMouseButton[count] = false;
			mMouseButtonLast[count] = false;
		}

		Input.simulateMouseWithTouches = true;
	}
	
	void Update () 
	{
		// Cache the last frame mouse status and read in the current mouse status 
		for( int count = 0; count < kNumMouseButtons; count++ )
		{
			mMouseButtonLast[count] = mMouseButton[count];
			mMouseButton[count] = Input.GetMouseButton( count );
		}

		bool tap = false;
		bool swipe = false;

		Direction direction = Direction.Left;

		// Detect different input types: Tap and Swipe
		if( MouseButtonJustPressed( MouseButtons.Left ) )
		{
			mStartingPosition = Input.mousePosition;
			mStartingTime = Time.time;
			mPotentialTap = true;
			mPotentialSwipe = false;
		}
		else if( MouseButtonHeld( MouseButtons.Left ) )
		{
			float duration = Time.time - mStartingTime;
			mPotentialTap = mStartingPosition == Input.mousePosition && duration <= TapDuration;
			mPotentialSwipe = mStartingPosition != Input.mousePosition && duration <= SwipeDuration;
		}
		else if( MouseButtonJustReleased( MouseButtons.Left ) )
		{
			if( mPotentialTap )
			{
				tap = true;
			}
			else if( mPotentialSwipe )
			{
				swipe = true;
				float duration = Time.time - mStartingTime;
				Vector3 difference = mStartingPosition - Input.mousePosition;
				if( Mathf.Abs( difference.x ) > Mathf.Abs( difference.y ) )
				{
					// Horizontal Movement
					if( difference.x < 0 )
					{
						// Right
						direction = Direction.Right;
					}
					else
					{
						// Left
						direction = Direction.Left;
					}
				}
				else
				{
					// Vertical Movement
					if( difference.y < 0 )
					{
						// Up
						direction = Direction.Up;
					}
					else
					{
						// Down
						direction = Direction.Down;
					}
				}
			}
		}
		else
		{
			mStartingPosition = Vector3.zero;
			mStartingTime = 0.0f;
			mPotentialTap = false;
			mPotentialSwipe = false;
		}
			
		if( tap || swipe )
		{
			if( tap && OnTap != null )
			{
				OnTap( Input.mousePosition );
			}
			else if( swipe && OnSwipe != null )
			{
				OnSwipe( direction );
			}
		}
	}

	private bool MouseButtonJustPressed( MouseButtons button )
	{
		return mMouseButton[(int)button] && !mMouseButtonLast[(int)button];
	}

	private bool MouseButtonJustReleased( MouseButtons button )
	{
		return !mMouseButton[(int)button] && mMouseButtonLast[(int)button];
	}

	private bool MouseButtonHeld( MouseButtons button )
	{
		return mMouseButton[(int)button] && mMouseButtonLast[(int)button];
	}

	/*void OnGUI()
	{
		string content = "+x+x+x+x+x+x+x+x+x+x+x+ Mouse Input +x+x+x+x+x+x+x+x+x+x+x+";
		content += "\n";

		content += "Starting Position: " + mStartingPosition.ToString();
		content += "\n";

		content += "Starting Time: " + mStartingTime.ToString();
		content += "\n";

		if( mPotentialTap )
		{
			content += "TAPPING?";
			content += "\n";
		}

		if( mPotentialSwipe )
		{
			content += "SWIPING?";
			content += "\n";
		}

		content += "\n+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+x+\n";

		GUI.enabled = false;
		
		GUI.TextArea( new Rect( 10, 10, Screen.width - 20, Screen.height - 20 ), content );
	}*/
}
