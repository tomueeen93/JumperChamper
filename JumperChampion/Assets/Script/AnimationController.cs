using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {
	Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		Debug.Log (animator);
		GameObject go = GameObject.Find("Board");
		go.SendMessage("setAnimator",animator);
		Debug.Log ("set animator");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			//AnimateJumping();
		}
		if (Input.GetKeyDown(KeyCode.B)) {
			//AnimateLeftSlide();
		}
	}

	public void AnimateJumping(){
		Debug.Log ("animate jumping");
		animator.SetTrigger("toJumping");
	}
	public void AnimateLeftSlide(){
		animator.SetTrigger("toLeftSlide");
	}
	public void AnimateRightSlide(){
		animator.SetTrigger("toRightSlide");
	}
}
