using UnityEngine;
using System.Collections;

public class ObstacleFactory : MonoBehaviour 
{
    [SerializeField] private float StartX;
    [SerializeField] private float StartY;
    [SerializeField] private float ScaleX = 1.0f;
    [SerializeField] private float ScaleY = 1.0f;
    [SerializeField] private float ScaleZ = 1.0f;

	private static ObstacleFactory mInstance; 

	void Awake()
	{
		if( mInstance == null )
		{
			mInstance = this;
		}
		else
		{
			Debug.LogError( "Only one ObstacleFactory allowed - destorying duplicate" );
			Destroy( this.gameObject );
		}
	}

    public static Obstacle Create(bool[] Positions)
	{
		float x = mInstance.StartX;
		float y = mInstance.StartY;
		float xs = mInstance.ScaleX;
		float ys = mInstance.ScaleY;
		float zs = mInstance.ScaleZ;

		GameObject newObstacle = new GameObject( "Obstacle" );

		for( int Position = 0; Position < 3; Position++ )
		{
            if (Positions[Position])
			{
				CreateCube( x, y, xs, ys, zs, newObstacle );
			}

			x += xs;
		}

		newObstacle.transform.position = new Vector3( 0.0f, 0.0f, mInstance.transform.position.z );

		Obstacle ob = newObstacle.AddComponent<Obstacle>();
        ob.SetLogic(Positions);
		return ob;
	}

	private static void CreateCube( float x, float y, float xs, float ys, float zs, GameObject parent )
	{
		GameObject cube = GameObject.CreatePrimitive( PrimitiveType.Cube );
		cube.transform.position = new Vector3( x, y, 0.0f );
		cube.transform.localScale = new Vector3( xs, ys, zs );
		cube.transform.parent = parent.transform;
	}
}
