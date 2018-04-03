using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillBar : MonoBehaviour {
    public GameObject fillBar;
	// Use this for initialization
	void Start () {
		
	}
	
    //Scale should be between 0 and 1
	public void SetScale(float scale)
    {
        fillBar.transform.localScale = new Vector3(scale, 1f, 1f);
    }
}
