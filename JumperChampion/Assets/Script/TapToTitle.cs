using UnityEngine;
using System.Collections;

public class TapToTitle : MonoBehaviour {
	void Update () {
        if (Input.GetMouseButton(0))
        {
            Application.LoadLevel("title");
        }
	}
}
