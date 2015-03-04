#define DEBUG
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoardController : MonoBehaviour {
	// 滑走スピード
	public float move_power = 100;
	// ジャンプ力
	public float jump_power = 0.5f;
	// 回転力
	public float rotate_power = 25;
	// SpeedUpAreaでかかる重力
	public float down_power = 50;
	// 進行方向の判定
	public bool go_left = false;
	// 滑走中の判定
	public bool sliding = false;
	// 着地後かどうかの判定
	public bool landed = false;
	// ジャンプ後かどうかの判定
	public bool jumped = true;
	// 体の捻りの判定
	public bool left_twist = false;
	public bool right_twist = false;
	// 逆ひねりをいれるタイミング
	public float twist_time = 0;
	// 回転ジャンプをするかどうか
	public bool rotate_jump = false;
	// どちら側を前に滑っているか
	public bool left_front = false;
	// 着地した際のBadDireciton判定
	public bool bad_direction = false;
	// クラッシュしたかどうか
	public bool crashed = false;
	// ジャンプ中の回転数
	Vector3 delta_rotation;
	// 前のフレームのオイラー角
	Vector3 pre_rotation;
    // 回転数
    public int count_rotate = 0;

    // スコア
    public float score;
    // 基礎点
    public float base_score = 100;
    // スコア減少率
    public float decreasing_rate = 0.5f;

    // イベントの設定
	private bool touch;
	private bool slide;
	private bool flick;

	// タッチしていた時間
	float touchingTime;
	// タッチしている角度
	int deg;
	// フリックの距離
	float distance;
	// デバッグ用のオブジェクト
	public bool enable_debug = false;
	private GameObject TextObject;
	Text str;
	string status_str;
	string velocity_str;
	string distance_str;
	string rotate_str;
	string direction_str;
	string delta_rotation_str;
    string count_rotate_str;
    string score_str;

	// アニメーション用にオブジェクト取得
	Animator animator;
	public GameObject boarderModelObject;

	//サウンド用のオブジェクト取得
	public AudioSource[] audiosources;

	private void Start () {
		Debug.Log (boarderModelObject);
		Debug.Log (animator);

		// 音声の取得
		audiosources = GetComponents<AudioSource>();
		// ジャンプアニメーションの再生
		animator.SetTrigger("toJumping");
		// デバッグ用の処理
		TextObject = GameObject.Find("DebugLog");
		str = TextObject.GetComponent<Text>();
	}
	private void Update () {
		// 手前に進んでいく処理
		MoveForward ();
		// 空中の位置制御
		JumpControl ();
		// 速度制限をかける
		LimitSpeed ();
		// キー入力の判断
		JudgeKeyInput ();
		// タッチ操作がされていた時の処理
		JudgeTouch ();
		// スライド操作がされていた時の処理
		JudgeSlide ();
		// フリック操作がされた時の処理
		JudgeFlick ();
		// デバッグメッセージの表示
		if (enable_debug)debugMessage ();
	}
	private void MoveForward(){
		// 手前に進んでいく処理
		if(landed&&!crashed){
			transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - 0.3f);
		}else if(jumped&&!crashed){
			transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - 0.05f);
		}
	}
	private void JumpControl (){
		// 空中の位置制御
		if(jumped){
			// 回転数の計算
			CalculateDeltaRotation();
            // １回転したかどうかの判定
            JudgeOneRotate();
			// 壁に近づき過ぎた時の処理
			if(transform.position.x > 25)
				transform.position = new Vector3(29,transform.position.y,transform.position.z);
			else if(transform.position.x < -25)
				transform.position = new Vector3(-29,transform.position.y,transform.position.z);
		}
	}
	private void CalculateDeltaRotation() {
		// 前のフレームとの角度差を計算
		float dx = Mathf.Abs (transform.rotation.x - pre_rotation.x);
		float dy = Mathf.Abs (transform.rotation.y - pre_rotation.y);
		float dz = Mathf.Abs (transform.rotation.z - pre_rotation.z);
		// 差分を計算
		delta_rotation = new Vector3 (delta_rotation.x + dx, delta_rotation.y + dy, delta_rotation.z + dz);
		// 今の角度を保存
		pre_rotation = new Vector3(transform.rotation.x,transform.rotation.y,transform.rotation.z);
	}
    private void JudgeOneRotate() {
        // dr.yが1.7ごとに回転数を増やす
        if (count_rotate < Mathf.FloorToInt(delta_rotation.y / 1.7f)){
            count_rotate++;   
        }
    }
	private void LimitSpeed (){
		// 速度制限をかける default 50
		float maxspeed = 50;
		if (this.rigidbody.velocity.x < -maxspeed) {
			rigidbody.velocity = new Vector3(-maxspeed,rigidbody.velocity.y,0);
		} else if (this.rigidbody.velocity.x > maxspeed) {
			rigidbody.velocity = new Vector3(maxspeed,rigidbody.velocity.y,0);
		}
	}
	private void JudgeKeyInput (){
		// 左キー入力
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			// ジャンプ中なら左回転
			if(jumped){rigidbody.AddTorque(-transform.up * rotate_power, ForceMode.Impulse);}
		}
		// 右キー入力
		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			// ジャンプ中なら右回転
			if(jumped){rigidbody.AddTorque(transform.up * rotate_power, ForceMode.Impulse);}
		}
		// 上キー入力
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			if(jumped){rigidbody.AddTorque(transform.forward * (rotate_power/5), ForceMode.Impulse);}
		}
		// 下キー入力
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			if(jumped){rigidbody.AddTorque(-transform.forward * (rotate_power/5), ForceMode.Impulse);}
		}
	}
	private void JudgeTouch(){
		//　タッチ操作がされていた時の処理
		if(touch){
			Debug.Log("Touch");
			// タッチの状態を戻す
			setTouch(false);
		}
	}
	private void JudgeSlide(){
		// スライド操作がされていた時の処理
		if(slide){
			// Debug.Log("Slide");
			
			// ツイスト可能かどうかのチェック
			if(distance > 20 && sliding){
				// 左右ツイストの判定
				if(deg > 60 && deg < 120 && !left_twist)right_twist=true;
				else if(deg > 240 && deg < 300 && !right_twist)left_twist=true;
				else Debug.Log("Twist ERROR");
			}
			//　スライドの状態を終了する
			setSlide(false);
		}
	}
	private void JudgeFlick(){
		// フリック操作がされた時の処理
		if(flick){
			// Debug.Log("Flick");
			// TODO フリックした時の音を再生
			//audiosources[0].Play();
			// 回転をさせる
			Rotate (deg);
			// フリック状態の終了
			setFlick(false);
		}
	}
	private void Rotate(int deg){
		// オブジェクトの回転

		Vector3 v = new Vector3( Mathf.Cos(deg), Mathf.Sin(deg), 0f);
		// 右回転
        if (deg > 60 && deg < 120) rigidbody.AddTorque(transform.up * rotate_power, ForceMode.Impulse);
		// 左回転
        else if (deg > 240 && deg < 300) rigidbody.AddTorque(-transform.up * rotate_power, ForceMode.Impulse);
		else Debug.Log("Rotate ERROR");
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
	// トリガー中の処理
	private void OnTriggerStay(Collider other){
		// JumpArea中の処理
		if(other.name == "JumpArea"){
			if(sliding){
				if(left_twist){

				}
			}
		}
	}
	// トリガーを抜けた時の判定
	private void OnTriggerExit(Collider other)
	{
		// SpeedUpAreaを抜けたときの処理
		if(other.name == "SpeedUpArea"){
			// ジャンプ状態に入った時の処理
			if(sliding)	IntoJumping();
		}
	}
	private void IntoJumping(){
		// 角度のリセット
		delta_rotation = new Vector3(0,0,0);
		pre_rotation = new Vector3 (transform.rotation.x,transform.rotation.y,transform.rotation.z);
        // 回転数のリセット
        count_rotate = 0;
		// 滑走終了 ジャンプ開始
		sliding= false;
		jumped=true;
		// 滑走方向状態の解除
		bad_direction = false;
		// 着地のリセット
		landed = false;
		// 滑走サウンドの停止
		audiosources[1].Stop();
		// TODO ジャンプサウンドの再生
		
		// ジャンプアニメーションの開始
		animator.SetTrigger("toJumping");
		// スノーパーティクルを消す
		transform.FindChild("SnowParticle").gameObject.SetActiveRecursively(false);
	}
	// コリジョンの衝突判定
	private void OnCollisionEnter(Collision other)
	{
		// Groundタグとの衝突時の処理
		if(other.gameObject.tag == "Ground"){
			// 着地した時の処理
			if(!landed)IntoLanded();
		}
	}
	// 着地した時の処理
	private void IntoLanded(){
        // 着地時のボード角度から向きを判定
		float board_direction_Y = this.transform.eulerAngles.y;
		if(!checkLeftFront(board_direction_Y,30))Debug.Log ("Landed Check ERROR");
        // スコア加算
        AddScore();
		// ツイスト状態を解除
		left_twist = false;
		right_twist = false;
		// 着地状態に以降
		landed = true;
		// 滑走アニメーションを開始
		if(bad_direction)
			animator.SetTrigger("toIdle");
		else if(go_left){
			if(left_front)animator.SetTrigger("toRightSlide");
			else animator.SetTrigger("toLeftSlide");
		} else {
			if(left_front)animator.SetTrigger("toLeftSlide");
			else animator.SetTrigger("toRightSlide");
		}
		// 滑走サウンドの再生
		audiosources[1].Play();
		// 着地音の再生
		audiosources[2].Play ();
		// スノーパーティクルを出す
		transform.FindChild("SnowParticle").gameObject.SetActiveRecursively(true);
	}
    private void AddScore() { 
        if(bad_direction)score += (count_rotate * base_score) * decreasing_rate;
        else score += count_rotate * base_score;
    }

	// デバッグメッセージの表示
	public void debugMessage(){
		if (sliding)
			status_str = "sliding";
		else if (jumped)
			status_str = "jumping";
		// 速度の取得
		velocity_str = this.rigidbody.velocity.ToString();
		// 回転の取得
		rotate_str = this.transform.localRotation.y.ToString();
		// 回転角の取得
		delta_rotation_str = delta_rotation.ToString();
        // 回転数の取得
        count_rotate_str = count_rotate.ToString();
        // スコアの取得
        score_str = score.ToString();
        // デバッグ情報の書き込み
		string debug_message = "";
		debug_message = "status : " + status_str + "\nvelocity : " + velocity_str + "\nrotate : " + rotate_str
            + "\n" + direction_str + "\n" + delta_rotation_str + "\n" + count_rotate_str
            + "\n" + score_str;
		str.text = debug_message;
	}
	// どちらが前なのかチェック
	public bool checkLeftFront(float degY , int maxDeg){
        bad_direction = false;
		if(360-maxDeg < degY || degY < maxDeg){
			if(go_left){
				direction_str = "Good Direction : right front\n" + degY;
				left_front = false;
			}else{
				direction_str = "Good Direction : left front\n" + degY;
				left_front= false;
			}
		}else if (180-maxDeg < degY && degY < 180+maxDeg) {
			if(go_left){
				direction_str = "Good Direction : left front\n" + degY;
				left_front = true;
			}else{
				direction_str = "Good Direction : right front\n" + degY;
				left_front = true;
			}
		}else{
			direction_str = "Bad Direction\n" + degY;
			bad_direction = true;
		}
		return false;
	}

	// 外部からの呼び出し用
	public void setTime(float time){touchingTime = time;}
	public void setDeg(int deg){this.deg = deg;}
	public void setDistance (float distance){this.distance = distance;}
	public void setTouch (bool is_touch){touch = is_touch;}
	public void setSlide(bool is_slide){slide = is_slide;}
	public void setFlick(bool is_flick){flick = is_flick;}
	public void setAnimator(Animator animator){this.animator = animator;}
}
