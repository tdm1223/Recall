using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public float movePower = 1.0f;
	public float jumpPower = 1.0f;
	Rigidbody2D rb2d;
	Vector3 movement;
	bool isJumping=false;
	// Use this for initialization
	void Start () {
		rb2d=gameObject.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Jump"))
		{
			isJumping=true;
		}

		Move();
	}

	void Move()
	{
		Vector3 velocity = Vector2.zero;

		if(Input.GetAxis("Horizontal")<0)
		{
			velocity=Vector2.left;
		}
		else if(Input.GetAxis("Horizontal")>0)
		{
			velocity=Vector2.right;
		}
		else
		{
			velocity=Vector2.zero;
		}
		transform.position+=velocity*movePower*Time.deltaTime;
	}

	// void Jump()
	// {
	// 	if(!isJumping)
	// 	{
	// 		return;
	// 	}

	// 	rb2d.velocity=Vector2.zero;

	// 	Vector2 jumpPower
	// }
}
