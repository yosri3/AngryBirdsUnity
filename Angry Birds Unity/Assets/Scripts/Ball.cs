using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public Rigidbody2D rigidBody;
	public Rigidbody2D anchor;
	public float maxPull;
	public float whenToReset;

	private bool shot;
	private bool canReset;
	private Vector2 initialPosition;
	private Quaternion initialRotation;
	private SpringJoint2D spring;
	private Vector2 previousVelocity;
	private bool released;

	void Awake(){
		canReset = true;
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		spring = GetComponent<SpringJoint2D> ();
		rigidBody.isKinematic = true;
		released = true;
		shot = false;
	}
	void Update(){
		
		if (!released) {
			pullBird();
		} else {
			releaseBird ();

			if(shot){
				Debug.Log (rigidBody.velocity.magnitude);
				if(rigidBody.velocity.magnitude < whenToReset){
					hit ();
				}
			}
			previousVelocity = rigidBody.velocity;
		}
	}

	public void pullBird(){
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//restrict maximum pull but move freely within the maximum pull
		if (Vector2.Distance (mousePosition, anchor.position) > maxPull) {
			rigidBody.position = anchor.position + (mousePosition - anchor.position).normalized * maxPull;
		} else {
			rigidBody.position = mousePosition;
		}
	}

	public void releaseBird(){
		//if bird starts to slow down(due to spring pulling back) disable spring
		if(previousVelocity.magnitude > rigidBody.velocity.magnitude){
			//Component.Destroy (spring);
			shot = true;
			spring.enabled = false;
			//this.enabled = false;
		}
	}

	public void hit(){
		if(canReset){
			shot = false;
			rigidBody.isKinematic = true;
			rigidBody.velocity = Vector2.zero;
			rigidBody.angularVelocity = 0f;
			canReset = false;
			StartCoroutine (startReset());
		}
	}

	public void OnMouseDown () {
		released = false;
		rigidBody.isKinematic = true;
	}
	public void OnMouseUp () {
		released = true;
		rigidBody.isKinematic = false;
		canReset = true;
	}
	IEnumerator startReset(){
		yield return new WaitForSeconds (3);
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		spring.enabled = true;
	}

}
