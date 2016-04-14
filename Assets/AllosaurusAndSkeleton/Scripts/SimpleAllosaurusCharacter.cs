using UnityEngine;
using System.Collections;

public class SimpleAllosaurusCharacter : MonoBehaviour {
	public Animator allosaurusAnimator;
	public bool isGrounded=false;
	public bool jumpUp=false;
	public float jumpSpeed=5f;
	public float groundCheckOffset=0.3f;
	public float groundCheckDistance=.8f;
	public float runCycleLegOffset=.2f;

	void Start () {
		allosaurusAnimator = GetComponent<Animator> ();
	}
	
	void Update () {
		GroundedCheck ();			
	}
	
	public void Jump(){
		if (isGrounded & allosaurusAnimator.GetFloat ("Forward")>.1f) {
			jumpUp=true;
			isGrounded = false;
			allosaurusAnimator.applyRootMotion = false;
			allosaurusAnimator.SetBool ("Landing",false);

			float runCycle =
				Mathf.Repeat(
					allosaurusAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
			if (runCycle < .5){ 
				allosaurusAnimator.SetTrigger ("JumpLeftFoot");
			}else{
				allosaurusAnimator.SetTrigger ("JumpRightFoot");
			}

			GetComponent<Rigidbody> ().AddForce ((transform.up - allosaurusAnimator.GetFloat ("Forward") * transform.right) * jumpSpeed, ForceMode.Impulse);

		}
	}

	public void Attack(){
		allosaurusAnimator.SetTrigger ("Attack");
	}

	public void TailAttack(){
		allosaurusAnimator.SetTrigger ("TailAttack");
	}
	

	void GroundedCheck(){
		if (jumpUp) {
			if(allosaurusAnimator.GetCurrentAnimatorClipInfo (0) [0].clip.name == "Fly"){
				jumpUp=false;
			}
		}

		if (!jumpUp & Physics.Raycast (transform.position + Vector3.up * groundCheckOffset, Vector3.down, groundCheckDistance)) {
			if (!isGrounded) {
				allosaurusAnimator.SetBool ("Landing", true);
				allosaurusAnimator.applyRootMotion = true;
				isGrounded = true;
			}
		}
	}

	public void Move(float v,float h){
		allosaurusAnimator.SetFloat ("Forward", v);
		allosaurusAnimator.SetFloat ("Turn", h);
	}

}
