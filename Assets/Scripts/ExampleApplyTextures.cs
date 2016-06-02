using UnityEngine;
using System.Collections;

public class ExampleApplyTextures : MonoBehaviour {

	public ProceduralTexture[] textures;

	// Use this for initialization
	void Awake () {
		Texture2D tex = PTLayers.merge( textures );
		if(gameObject.GetComponent<Renderer>())
				gameObject.GetComponent<Renderer>().material.mainTexture = tex;
	}

	
}
