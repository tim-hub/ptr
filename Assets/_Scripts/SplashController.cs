using UnityEngine;
using System.Collections;

public class SplashController : MonoBehaviour {
	public string nextLevel;
	
	public void NextLevel(){ //click button 
		Application.LoadLevel(nextLevel);
	}

	public void Update(){ //or click screen
#if UNITY_EDITOR
		if(Input.GetMouseButton(0)){

			NextLevel();
		}
#elif UNITY_ANDROID
		if(Input.touchCount>=1){
			NextLevel();
		}

#endif
	}
}
