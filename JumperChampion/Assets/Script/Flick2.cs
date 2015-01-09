using UnityEngine;
using System.Collections;

public class Flick2 : MonoBehaviour {
	private bool isFlick;
	private bool isClick;
	private Vector3 touchStartPos;
	private Vector3 touchEndPos;
	private int direction;
	
	public void Update () {
		if(Input.GetKeyDown (KeyCode.Mouse0))
		{
			isFlick = true;
			touchStartPos = new Vector3(Input.mousePosition.x ,
			                            Input.mousePosition.y ,
			                            Input.mousePosition.z);
			Invoke ("FlickOff" , 0.2f);
		}
		if(Input.GetKey (KeyCode.Mouse0))
		{
			touchEndPos = new Vector3(Input.mousePosition.x ,
			                          Input.mousePosition.y ,
			                          Input.mousePosition.z);
			if(touchStartPos != touchEndPos )
			{
				ClickOff ();
			}
		}
		if(Input.GetKeyUp (KeyCode.Mouse0))
		{
			touchEndPos = new Vector3(Input.mousePosition.x ,
			                          Input.mousePosition.y ,
			                          Input.mousePosition.z);
			Debug.Log (touchEndPos);
			if(IsFlick ())
			{
				Debug.Log ("Flick");
				float directionX = touchEndPos.x - touchStartPos.x;
				float directionY = touchEndPos.y - touchStartPos.y;
				Debug.Log ("DirectionX : " + directionX);
				Debug.Log ("DirectionY : " + directionY);
				if(Mathf.Abs (directionY) < Mathf.Abs (directionX))
				{
					if(0 < directionX)
					{
						Debug.Log ("Flick : Right");
						direction = 6;
					}
					else
					{
						Debug.Log ("Flick : Left");
						direction = 4;
					}
				}
				else if(Mathf.Abs (directionX) < Mathf.Abs (directionY))
				{
					if(0 < directionY)
					{
						Debug.Log ("Flick : Up");
						direction = 8;
					}
					else
					{
						Debug.Log ("Flick : Down");
						direction = 2;
					}
				}
				else
				{
					Debug.Log ("Flick : Not, It's Tap");
					FlickOff();
				}
			}
			else
			{
				Debug.Log ("Long Touch");
				direction = 5;
			}
		}
	}
	public void FlickOff()
	{
		direction = 5;
		isFlick = false;
	}
	
	public bool IsFlick()
	{
		return isFlick;
	}
	
	
	public void ClickOn()
	{
		isClick = true;
		Invoke ("ClickOff" , 0.2f);
	}
	
	public bool IsClick()
	{
		return isClick;
	}
	
	public void ClickOff()
	{
		isClick = false;
	}
}
