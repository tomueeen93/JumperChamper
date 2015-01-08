using UnityEngine;
using System.Collections;

[System.Serializable]
public class FlickCallBackRule {
	public int deg;
	public string callbackName;
}

public class Flick : MonoBehaviour {
	public float validityTouchTime = 0.5f;
	public float validityTouchDistance = 20.0f;
	
	public float validityFlickTime = 0.5f;
	public float validityFlickMinDistance = 30.0f;
	public float validityFlickMaxDistance = 300.0f;
	public int validityFlickDegRange = 20;
	
	public bool enabledOnTouch = true;
	public FlickCallBackRule[] rules;
	
	private float touchTime = 0;
	private bool isTouch = false;
	
	private Vector3 touchPosition;
	
	void Update () {
		// マウスをクリックした時の処理
		if (Input.GetMouseButtonDown(0)) Down();
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
				// 
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