using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

	public float timeOut=2f;

	void Awake(){

		Destroy(gameObject,timeOut);
	}
}
