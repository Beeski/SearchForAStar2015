using UnityEngine;
using System.Collections;

public class BlockFactory : MonoBehaviour 
{
    [SerializeField] private Block BlockPrefab;

    private Block[] mPool;

	private static BlockFactory mInstance;

    private const int MAX_OBSTACLES = 40;

	void Awake()
	{
		if( mInstance == null )
		{
			mInstance = this;
            mPool = new Block[MAX_OBSTACLES];

            for (int count = 0; count < MAX_OBSTACLES; count++)
            {
                mPool[count] = Instantiate(mInstance.BlockPrefab) as Block;
                mPool[count].gameObject.SetActive(false);
				mPool[count].gameObject.name = "Block_" + ( count + 1 );
                mPool[count].transform.parent = transform;
            }
		}
		else
		{
			Debug.LogError( "Only one BlockFactory allowed - destorying duplicate" );
			Destroy( this.gameObject );
		}
	}

    public static Block Create()
	{
        Block newBlock = null;
        if (mInstance != null && mInstance.BlockPrefab != null)
        {
            for( int count = 0; count < MAX_OBSTACLES; count++ )
            {
                if( !mInstance.mPool[count].gameObject.activeInHierarchy )
                {
                    newBlock = mInstance.mPool[count];
					break;
                }
            }
        }
        else
        {
            Debug.LogError("Unable to create Block");
        }
        return newBlock;
    }
}
