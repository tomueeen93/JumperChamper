using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class FlickCallBackRule {
	public int deg;
	public string callbackName;
}

public class Flick : MonoBehaviour {
	public float validityTouchTime = 0.5f;
	public float validityTouchDistance = 20.0f;

	public float validitySlideTime = 0.5f;
	public float validitySlideDistance = 30.0f;

	public float validityFlickTime = 0.5f;
	public float validityFlickMinDistance = 30.0f;
	public float validityFlickMaxDistance = 300.0f;
	public int validityFlickDegRange = 20;
	
	public bool enabledOnTouch = true;
	public FlickCallBackRule[] rules;
	
	private float touchTime = 0;
	private float touchingTime = 0;

	private bool isTouch = false;
	private bool isSlide = false;
	private bool isFlick = false;
	
	private Vector3 touchPosition;
	private Vector3 touchingPosition;

	private GameObject TextObject;
	//private Text msgText = TextObject.gameObject.GetComponent<Text>();

	void Update () {
		// マウスをクリックした瞬間の処理
		if (Input.GetMouseButtonDown(0)) Down();
		// マウスをクリックしている時の処理
		if(Input.GetMouseButton(0)) Touching();
		// マウスを離した時の処理
		if (Input.GetMouseButtonUp(0)) Up();
	}

	// マウスクリックした時の処理
	void Down() {
		// タッチした時のポジションを保存
		touchPosition = Input.mousePosition;
		// タッチした時間の保存
		touchTime = Time.time;
		isTouch = true;
	}
	// マウスクリックをしている間の処理
	void Touching(){
		// タッチしている座標を保存
		touchingPosition = Input.mousePosition;
		// タッチしているところの距離を保存
		float distance = Vector3.Distance(touchPosition, touchingPosition);
		// 角度を計算
		int deg = getDeg (touchPosition, touchingPosition);
		// タッチしてからの時間を取得
		float touchingTime = Time.time - touchTime;

		// デバッグ用の処理
		TextObject = GameObject.Find("DebugLog");
		Text str = TextObject.GetComponent<Text>();
		str.text = "distance : "+distance+"\ndeg : "+deg+"\ntime : "+touchingTime;
	}

	// マウスを離した時の処理
	void Up() {
		// タッチしているかをチェック
		if (!isTouch) return;
		// タッチし終わった時のマウスポジションを保存
		Vector3 touchEndPosition = Input.mousePosition;

		// ここまでの経過時間を保存
		float deltaTime = Time.time - touchTime;
		// フリック距離を保存
		float distance = Vector3.Distance(touchPosition, touchEndPosition);
		// フリックした方向（度数法）を保存
		int deg = getDeg(touchPosition, touchEndPosition);

		// タッチが有効　かつ タッチ時間と距離の整合性チェック
		if (enabledOnTouch && ValidateTouch(deltaTime, distance)) {
			SendMessage("OnTouch");
		}
		// 同じくフリックの整合性のチェック
		if (ValidateFlick(deltaTime, distance)) {
			// フリックルールの設定
			foreach (FlickCallBackRule rule in rules) {

				if (ValidateFlickDeg(rule.deg, deg)) {
					SendMessage(rule.callbackName);
					break;
				}
			}
		}
		// すべての整合性がとれたらタッチ判定を消す
		isTouch = false;
	}
	
	bool ValidateTouch(float deltaTime, float distance) {
		if (validityTouchTime < deltaTime) return false;
		if (validityTouchDistance < distance) return false;
		return true;
	}
	
	bool ValidateFlick(float deltaTime, float distance) {
		if (validityFlickTime < deltaTime) return false;
		
		return (validityFlickMinDistance < distance && distance < validityFlickMaxDistance);
	}
	
	bool ValidateFlickDeg(int ruleDeg, int deg) {
		int min = ruleDeg - validityFlickDegRange;
		int max = ruleDeg + validityFlickDegRange;
		
		if (min < deg && deg < max) return true;
		
		// 0度付近を考慮し360度分足してから再チェック
		min += 360;
		max += 360;
		
		return min < deg && deg < max;
	}
	
	int getDeg(Vector3 a, Vector3 b) {
		return Mathf.RoundToInt(180 + (Mathf.Atan2(a.x - b.x, a.y - b.y) * Mathf.Rad2Deg));
	}
}