using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

		private Animator anim;
		private CharacterController controller;

		public float speed = 15.0f;
		public float turnSpeed = 400.0f;
		private Vector3 moveDirection = Vector3.zero;
		public float gravity = 20.0f;

		void Start () {
			Cursor.visible = false;
        	Cursor.lockState = CursorLockMode.Locked;
			controller = GetComponent <CharacterController>();
			anim = gameObject.GetComponentInChildren<Animator>();
		}

		void Update (){
			if(controller.isGrounded){
				if (Input.GetKey ("w")) {
					if (Input.GetKey (KeyCode.LeftShift)) {
						speed = 30f;
					}
					else{
						speed = 15f;
					}
					anim.SetInteger ("AnimationPar", 1);
				}
				else {
					anim.SetInteger ("AnimationPar", 0);
				}
				
			}

			if(controller.isGrounded){
				moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
				if (Input.GetKey ("space")) {
					moveDirection.y += gravity*1.2f;
					anim.SetInteger ("AnimationPar", 0);
					//Debug.Log("Jump");
				}
			}

			float turn = Input.GetAxis("Horizontal");
			transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
			if ((transform.position.z > 20 && transform.position.z< 3000) && (transform.position.x > 20 && transform.position.x< 980)){
				controller.Move(moveDirection * Time.deltaTime);
			}
			else{
				controller.Move(moveDirection * Time.deltaTime);
				var pos = transform.position;
				pos.x = Mathf.Clamp(transform.position.x, 20.0f, 980.0f);
				pos.z = Mathf.Clamp(transform.position.z, 20.0f, 980.0f);
				transform.position = pos;
			}

			moveDirection.y -= gravity * Time.deltaTime;
						
		}
}
