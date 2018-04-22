using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour {

    public float showTime = 2;
	// Use this for initialization
	void Start () {
        StartCoroutine(Show());
	}
	
    IEnumerator Show()
    {
        yield return new WaitForSeconds(showTime);
        Destroy(gameObject);
    }
}
