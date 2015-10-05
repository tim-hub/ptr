using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {
	public string nextLevel;
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR

		if(Input.GetMouseButton(0)){
			Application.LoadLevel(nextLevel);
		}
#elif UNITY_ANDROID

		if(Input.touchCount>0){ //when screen was touched

			Application.LoadLevel(nextLevel);
		}
#endif
	}
}
