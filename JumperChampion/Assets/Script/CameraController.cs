using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject player = null;
	public Vector3 offset;
	private int Y_axis_fixer = 0;
	private GameObject target = null;
	private GameObject boader = null;

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
		if(boader.GetComponent<BoardController>().sliding){
			this.transform.position = new Vector3 (
				this.player.transform.position.x + this.offset.x,
				this.player.transform.position.y + this.offset.y,
				this.player.transform.position.z + this.offset.z
				);
		}
		transform.LookAt (player.transform);
	}
}
