using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {
	float move_power;
	float jump_power;
	float rotate_power;
	float down_power;
	public bool jump_enable;
	bool rotate_enable;
	bool go_left;
	bool sliding;
	bool landed;

	// Use this for initialization
	private void Start () {
		move_power = 100;
		jump_power = 0.5f;
		rotate_power = 250;
		down_power = 100;
		jump_enable = true;
		rotate_enable = false;
		go_left = false;
		sliding = false;
		landed = false;

	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow) && rotate_enable) {
			Debug.Log("Rotate Left");
			rigidbody.AddRelativeTorque(Vector3.down * rotate_power, ForceMode.Impulse);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow) && rotate_enable) {
			Debug.Log("Rotate Right");
			rigidbody.AddRelativeTorque(Vector3.up * rotate_power, ForceMode.Impulse);
		}
		
		if (Input.GetKeyDown(KeyCode.UpArrow) && jump_enable) {
			rigidbody.AddRelativeForce(Vector3.up * jump_power, ForceMode.Impulse);
			Debug.Log("Jump");
		}
		/*    
    	if (Input.GetKeyDown(KeyCode.DownArrow)) {
        	rigidbody.AddRelativeTorque(Vector3.up * rotate_power, ForceMode.Impulse);
    	}
		*/
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if(sliding){

			}else{
				rigidbody.AddForce(Vector3.down * move_power, ForceMode.Impulse);
			}

		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(!sliding){
			sliding=true;
			Debug.Log ("sliding : " + sliding);
			if(other.name == "SpeedUpArea"){
				Debug.Log ("SUA in");
				rotate_enable = false;
				jump_enable = false;
				go_left = !(go_left);
				Debug.Log ("go_left : "+go_left);
			}
		}

	}
	private void OnTriggerExit(Collider other)
	{
		if(sliding){
			sliding= false;
			Debug.Log ("sliding : "+sliding);
			if(other.name == "SpeedUpArea"){
				Debug.Log ("SUA out");
				rotate_enable = true;
				jump_enable = true;
				landed = false;
			}
		}
	}

	private void OnCollisionEnter(Collision other)
	{

		if(!landed && other.gameObject.tag == "Ground"){
			float board_direction_X = this.transform.eulerAngles.x;
			if(330 < board_direction_X || board_direction_X < 30){
				if(go_left)Debug.Log ("Good Direction : right front " + board_direction_X);
				else Debug.Log ("Good Direction : left front " + board_direction_X);
			}else if (150 < board_direction_X && board_direction_X < 210) {
				if(go_left)Debug.Log ("Good Direction : left front " + board_direction_X);
				else Debug.Log ("Good Direction : right front " + board_direction_X);
			}else{
				Debug.Log("Bad Direction " + board_direction_X);
			}
			landed = true;
		}
	}

}
