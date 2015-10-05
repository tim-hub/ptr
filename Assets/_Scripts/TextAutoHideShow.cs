using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextAutoHideShow : MonoBehaviour {

	public float alphaChangingRate=5f;
	private Color textColor;
	Color textColorAlpha0;
	Color textColorAlpha255;

	void Start(){
		textColor=GetComponent<Text>().color;

	}
	
	// Update is called once per frame
	void Update () {
		float newA;
		newA =Mathf.Cos(Time.timeSinceLevelLoad)*127.5f+127.5f;

		textColor.a=Mathf.Clamp(newA,0f,255f);
		//Debug.Log(textColor.a);
		//textColor.a=0f;
		GetComponent<Text>().color=textColor;

	}
}
