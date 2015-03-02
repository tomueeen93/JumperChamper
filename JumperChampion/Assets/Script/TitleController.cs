using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {
	void Update () {
		if(Input.GetMouseButton(0)){
			Application.LoadLevel("main");
		}
	}
}
