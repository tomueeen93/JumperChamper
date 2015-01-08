using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {
	// 滑走スピード
	public float move_power;
	// ジャンプ力
	public float jump_power;
	// 回転力
	public float rotate_power;
	// SpeedUpAreaでかかる重力
	public float down_power;
	// 進行方向の判定
	public bool go_left;
	// 滑走中の判定
	public bool sliding;
	// 着地後かどうかの判定
	public bool landed;
	// ジャンプ後かどうかの判定
	public bool jumped;
	// 体の捻りの判定
	public bool left_twist;
	public bool right_twist;

	// Use this for initialization
	private void Start () {
		move_power = 100;
		jump_power = 0.5f;
		rotate_power = 250;
		down_power = 100;
		go_left = false;
		sliding = false;
		landed = false;
		jumped = true;
	}
	
	// Update is called once per frame
	private void Update () {
		// 左キー入力
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			// ジャンプ中なら左回転
			if(jumped){
				rigidbody.AddRelativeTorque(Vector3.down * rotate_power, ForceMode.Impulse);
			}
		}
		// 右キー入力
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			// ジャンプ中なら右回転
			if(jumped){
				rigidbody.AddRelativeTorque(Vector3.up * rotate_power, ForceMode.Impulse);
			}
		}
		// 上キー入力
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			rigidbody.AddRelativeForce(Vector3.up * jump_power, ForceMode.Impulse);
			Debug.Log("Jump");
		}
		// 下キー入力
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if(sliding){

			}else{
				rigidbody.AddForce(Vector3.down * move_power, ForceMode.Impulse);
			}

		}
	}

	// トリガーの衝突判定
	private void OnTriggerEnter(Collider other)
	{
		// SpeedUpAreaへ侵入時の処理
		if(other.name == "SpeedUpArea"){
			// ジャンプ中だったら
			if(jumped){
				// ジャンプ終了 滑走開始
				jumped = false;
				sliding = true;
				// 進行方向の反転
				go_left = !(go_left);
				Debug.Log ("go_left : "+go_left);
			}
		}
	}
	// Triggerを抜けた時の判定
	private void OnTriggerExit(Collider other)
	{
		// SpeedUpAreaを抜けたときの処理
		if(other.name == "SpeedUpArea"){
			// 滑走中だった場合
			if(sliding){
				Debug.Log ("sliding : " + sliding);
				// 滑走終了 ジャンプ開始
				sliding= false;
				jumped=true;
				// 着地のリセット
				landed = false;
			}
		}
	}

	// コリジョンの衝突判定
	private void OnCollisionEnter(Collision other)
	{
		// Groundタグとの衝突時の処理
		if(other.gameObject.tag == "Ground"){
			// 着地していなかったら
			if(!landed){
				// 着地時のボード角度から向きを判定
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
				// 着地状態に以降
				landed = true;
			}
		}
	}

}
