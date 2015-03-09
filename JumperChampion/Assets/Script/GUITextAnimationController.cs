using UnityEngine;
using System.Collections;

public class GUITextAnimationController : MonoBehaviour {
    public float d_point = 0.0f;
    public float speed = 1f;
    public float accel = 0.3f;
    public Vector3 dv = new Vector3(0,250,0);
    void Start () {
        StartCoroutine(Routine());
	}

    IEnumerator Routine() {
        dv.x = Random.Range(-75, 75);
        dv.y = Random.Range(dv.y -75, dv.y + 75);
        while(true){
            if (transform.localPosition.y < dv.y - 10)
            {
                // 目標点を超えていない
                // １フレームで移動する量を計算
                d_point = speed * accel;
                // 移動させる
                transform.localPosition = transform.localPosition + new Vector3(Mathf.Lerp(0, dv.x, d_point), Mathf.Lerp(0, dv.y, d_point), 0);
                yield return new WaitForSeconds(0.001f);
            }
            else
            {
                // 目標点を超えている
                yield return new WaitForSeconds(0.6f);
                Destroy(this.gameObject);
            }
        }
    }
}
