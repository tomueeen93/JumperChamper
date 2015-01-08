using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {

	// Playerプレハブ
	public GameObject player;
	
	// タイトル
	private GameObject title;
	
	void Start ()
	{
		// Titleゲームオブジェクトを検索し取得する
		title = GameObject.Find ("Title");
	}
	
	void Update ()
	{
		
		for (int i = 0; i < Input.touchCount; i++) {
			
			// タッチ情報を取得する
			Touch touch = Input.GetTouch (i);
			
			// ゲーム中ではなく、タッチ直後であればtrueを返す。
			if (IsPlaying () == false && touch.phase == TouchPhase.Began) {
				GameStart ();
			}
		}
		
		// ゲーム中ではなく、マウスクリックされたらtrueを返す。
		if (IsPlaying () == false && Input.GetMouseButtonDown (0)) {
			GameStart ();
		}
	}
	void OnGUI ()
	{
		// ゲーム中ではなく、タッチまたはマウスクリック直後であればtrueを返す。
		if (IsPlaying () == false && Event.current.type == EventType.MouseDown) {
			GameStart ();
		}
	}

	void GameStart ()
	{
		// ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
		title.SetActive (false);
		Instantiate (player, player.transform.position, player.transform.rotation);
	}
	
	public void GameOver ()
	{
		//FindObjectOfType<Score> ().Save ();
		// ゲームオーバー時に、タイトルを表示する
		title.SetActive (true);
	}
	
	public bool IsPlaying ()
	{
		// ゲーム中かどうかはタイトルの表示/非表示で判断する
		return title.activeSelf == false;
	}
}
