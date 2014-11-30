using UnityEngine;
using System.Collections;

public class CombinePTextures : MonoBehaviour {

	public ProceduralTexture[] textures;

	// Use this for initialization
	void Awake () {
		Texture2D tex = mergeLayers( textures );
		if(gameObject.renderer)
				gameObject.renderer.material.mainTexture = tex;
	}

	Texture2D mergeLayers( ProceduralTexture[] colorLayers ){
		if(colorLayers.Length<=0)
			return null;

		ProceduralTexture tex = colorLayers[0];

		Color[] pix = new Color[tex.width * tex.height];

		for(int k = 0; k < colorLayers.Length; k++){
			tex = colorLayers[k];
			Color[] add = tex.pixColors();
			for (int y = 0; y < tex.height; y++) {
				for (int x = 0; x < tex.width; x++) {
					int i = y * tex.width + x;

					float fromRatio = Mathf.Clamp( add[i].a, 0.0f, 1.0f );
					float toRatio = 1.0f-fromRatio;
					// Debug.Log("from:"+fromRatio+" to:"+toRatio);
					// pix[i] = new Color(color1[i].r*toRatio+color2[i].r*fromRatio, color1[i].g*toRatio+color2[i].g*fromRatio, color1[i].b*toRatio+color2[i].b*fromRatio);
					pix[i] = pix[i]*toRatio + add[i]*fromRatio;
				}
			}
		}

		Texture2D mergedTex = new Texture2D(tex.pixWidth, tex.pixHeight);
		mergedTex.SetPixels(pix);
		mergedTex.Apply();

		return mergedTex;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
