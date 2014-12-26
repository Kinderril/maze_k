using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CheckList : MonoBehaviour {

	// Use this for initialization
    Toggle[] toggles ;

    void Awake()
    {
        toggles = GetComponentsInChildren<Toggle>();
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener(OnValueChange);
        }
    }

    private void OnValueChange(bool arg0)
    {
        
    }
    
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
