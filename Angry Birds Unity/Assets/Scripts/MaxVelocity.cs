using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxVelocity : MonoBehaviour {
	public bool shatters;
	public GameObject shatterObject;
	public int breakPoint;


	void OnCollisionEnter2D (Collision2D collision){
		
		if(collision.relativeVelocity.magnitude > breakPoint){
			if(shatters){
				Vector2 shatterObjectPosition = new Vector2(this.gameObject.transform.position.x,this.gameObject.transform.position.y);

				GameObject.Instantiate (shatterObject, shatterObjectPosition, this.gameObject.transform.rotation );
			}
			Destroy(gameObject);
		}
	}
}
