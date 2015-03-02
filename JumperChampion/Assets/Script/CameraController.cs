using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject player = null;
	public Vector3 offset;
	private int Y_axis_fixer = 0;
	private GameObject target = null;
	private GameObject boader = null;
	private float dtime = 0.0f;
	private float atime = 0.001f;

	// Use this for initialization
	void Start () {
		this.player = GameObject.FindGameObjectWithTag ("Player");
		this.boader = GameObject.FindGameObjectWithTag ("Board");
		this.target = player;
		this.offset = this.transform.position - this.player.transform.position;

		Debug.Log(boader.GetComponent<BoardController>().jumped);
	}
	
	// Update is called once per frame
	void Update () {
		// 滑走中はプレイヤーを追尾
		if (boader.GetComponent<BoardController> ().sliding) {
			dtime = 0;
			atime = 0.001f;
			this.transform.position = new Vector3 (
				//this.player.transform.position.x + this.offset.x,
				0,
				this.player.transform.position.y + this.offset.y,
				this.player.transform.position.z + this.offset.z
			);
				
		}
		else {// ジャンプ中だったときのカメラ
			if(dtime<0.4f){
				this.transform.position = new Vector3 (
					Mathf.Lerp(0, this.player.transform.position.x, dtime),
					this.player.transform.position.y + this.offset.y,
					Mathf.Lerp(this.player.transform.position.z + this.offset.z, this.player.transform.position.z, dtime)
					);
				atime = atime * 1.15f;
				dtime = dtime + atime;
			}
		}
		transform.LookAt (player.transform);
	}
}
