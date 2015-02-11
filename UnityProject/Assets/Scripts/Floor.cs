using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
		Block ob = collision.gameObject.GetComponent<Block>();
		if( ob != null )
		{
			ob.InContactWithFloor = true;
		}
    }
}
