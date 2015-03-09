using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUITextGenerator : MonoBehaviour {
    public GameObject GuiTextObject;
	public void GenerateText(string message, int color){
        GameObject clone = (GameObject)Instantiate(GuiTextObject, this.transform.localPosition, Quaternion.identity);
        clone.transform.parent = this.transform;
        clone.transform.localScale = new Vector3(1, 1, 1);
        clone.transform.localRotation = Quaternion.identity;
        Color setColor = Color.red;
        if (color == 1) setColor = Color.red;
        else if (color == 2) setColor = Color.yellow;
        else if (color == 3) setColor = Color.blue;
        clone.GetComponent<Text>().color = setColor;
        clone.GetComponent<Text>().text = message;
    }
}
