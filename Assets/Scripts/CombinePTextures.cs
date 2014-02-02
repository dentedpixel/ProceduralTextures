using UnityEngine;
using System.Collections;

public class CombinePTextures : MonoBehaviour {

	public ProceduralTexture texture1;
	public ProceduralTexture texture2;

	// Use this for initialization
	void Awake () {
		Color[] color1 = texture1.pixColors();
		Color[] color2 = texture2.pixColors();

		if(texture1.pixWidth==texture2.pixWidth && texture1.pixHeight==texture2.pixHeight){
			Texture2D noiseTex = new Texture2D(texture1.pixWidth, texture1.pixHeight);
			Color[] pix = new Color[noiseTex.width * noiseTex.height];

			for (int y = 0; y < noiseTex.height; y++) {
				for (int x = 0; x < noiseTex.width; x++) {
					int i = y * noiseTex.width + x;

					float fromRatio = color2[i].a > 1.0f ? 1.0f : color2[i].a;
					if(fromRatio<0.0f)
						fromRatio = 0.0f;

					float toRatio = 1.0f-fromRatio;
					// Debug.Log("from:"+fromRatio+" to:"+toRatio);
					// pix[i] = new Color(color1[i].r*toRatio+color2[i].r*fromRatio, color1[i].g*toRatio+color2[i].g*fromRatio, color1[i].b*toRatio+color2[i].b*fromRatio);
					pix[i] = color1[i]*toRatio+color2[i]*fromRatio;
				}
			}

			noiseTex.SetPixels(pix);
			noiseTex.Apply();

			if(gameObject.renderer)
				gameObject.renderer.material.mainTexture = noiseTex;
		}else{
			Debug.LogError("Height and Width of input textures do not match");
		}

		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
