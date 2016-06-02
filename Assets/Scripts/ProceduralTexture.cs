// Copyright (c) 2013 Russell Savage - Dented Pixel
// 
// Noise Textures version 0.1 - http://dentedpixel.com/developer-diary/
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using UnityEngine;
using System.Collections;

public class ProceduralTexture : MonoBehaviour {

	public enum TextureType{
		Wood,
		Metal,
		Clouds,
		Marble,
		Sand,
		WoodStripes
	}

	public bool isDirty = true;
	public TextureType textureType;
	public int noiseLayer = 0;
	public int turbolenceSize = 8;
	public float turbolenceScaleX = 4.0f;
	public float turbolenceScaleY = 4.0f;
	public float turbolenceStrength = 1.0f;
	public int pixWidth = 512;
	public int pixHeight = 512;
	public float scale = 1.0f;
	public Color fromColor = Color.white;
	public Color toColor = Color.black;
	public float cutoff = -1.0f;

	public float xOrg;
	public float yOrg;

	private Texture2D noiseTex;
	private Color[] pix;

	public TextureType lastSelectedType;

	public int width{
		get{  return pixWidth; }
	}

	public int height{
		get{  return pixHeight; }
	}

	void Awake () {
		init( );

		if(gameObject.GetComponent<Renderer>())
			gameObject.GetComponent<Renderer>().material.mainTexture = noiseTex;
	}

	public Color[] pixColors(){
		init( );
		return pix;
	}

	void init( ){
		if(noiseTex==null || isDirty){
			isDirty = false;
			if(lastSelectedType!=textureType){
				// Set good default values
				if(textureType==TextureType.Marble){
					scale = 0.15f;
					turbolenceSize = 9;
					turbolenceStrength = 2.8f;
				}else if(textureType==TextureType.Wood){
					scale = 294f;
					turbolenceSize = 6;
					turbolenceScaleX = 1.07f;
					turbolenceScaleY = 0.19f;
					turbolenceStrength = 11.4f;
					fromColor = new Color(223f/255f,188f/255f,112f/255f);
					toColor = new Color(169f/255f,122f/255f,60f/255f);
				}else if(textureType==TextureType.Sand){
					turbolenceSize = 2;
					turbolenceStrength = 1.0f;
				}
				lastSelectedType = textureType;
			}
			noiseTex = new Texture2D(pixWidth, pixHeight);
			pix = new Color[noiseTex.width * noiseTex.height];
			
			for (int y = 0; y < noiseTex.height; y++) {
				for (int x = 0; x < noiseTex.width; x++) {
					int i = y * noiseTex.width + x;
					if(textureType==TextureType.Marble){
						float xCoord = xOrg + x / (float)noiseTex.width * scale;
						float yCoord = yOrg + y / (float)noiseTex.height * scale;
						
						float xyValue = xCoord + yCoord + turbulence(x* scale, y* scale, turbolenceSize)*turbolenceStrength;
						float sample = Mathf.Abs( Mathf.Sin(xyValue) );

						float fromRatio = sample;
						float toRatio = 1.0f-sample;
						pix[i] = new Color(fromColor.r*fromRatio, fromColor.g*fromRatio, fromColor.b*fromRatio) + new Color(toColor.r*toRatio, toColor.g*toRatio, toColor.b*toRatio);
					}else if(textureType==TextureType.Wood){
						float xCoord = xOrg + x / (float)noiseTex.width * scale;
						//float yCoord = yOrg + y / (float)noiseTex.height * scale;
						
						float xyValue = xCoord + turbulence(x / (float)noiseTex.width*turbolenceScaleX * scale, y / (float)noiseTex.height*turbolenceScaleY * scale, turbolenceSize)*turbolenceStrength;
						float sample = Mathf.Sin(xyValue);

						float fromRatio = sample;
						float toRatio = 1.0f-sample;
						pix[i] = new Color(fromColor.r*fromRatio, fromColor.g*fromRatio, fromColor.b*fromRatio, fromColor.a*fromRatio) + new Color(toColor.r*toRatio, toColor.g*toRatio, toColor.b*toRatio, toColor.a*toRatio);
					}else if(textureType==TextureType.Sand){
						float xCoord = xOrg + x / (float)noiseTex.width * scale;
						float yCoord = yOrg + y / (float)noiseTex.height * scale;
						
						float sample = turbolenceSize<=1 ? Mathf.PerlinNoise(xCoord, yCoord) : turbulence(xCoord, yCoord, turbolenceSize)*turbolenceStrength;
						//Debug.Log("xCoord:"+xCoord+" yCoord:"+yCoord+" sample:"+sample+" turbolenceSize:"+turbolenceSize);
						float fromRatio = sample;
						float toRatio = 1.0f-sample;
						pix[i] = new Color(fromColor.r*fromRatio, fromColor.g*fromRatio, fromColor.b*fromRatio) + new Color(toColor.r*toRatio, toColor.g*toRatio, toColor.b*toRatio);
					}else if(textureType==TextureType.WoodStripes){
						float xCoord = xOrg + x / (float)noiseTex.width * scale;
						//float yCoord = yOrg + y / (float)noiseTex.height * scale;
						
						float xyValue = xCoord + turbulence(x / (float)noiseTex.width*turbolenceScaleX * scale, y / (float)noiseTex.height*turbolenceScaleY * scale, turbolenceSize)*turbolenceStrength;
						float sample = Mathf.Sin(xyValue);

						if(cutoff>0.0f)
							sample = (sample-cutoff) / (1.0f-cutoff);
						float fromRatio = sample;
						float toRatio = 1.0f-sample;

						pix[i] = new Color(fromColor.r*fromRatio, fromColor.g*fromRatio, fromColor.b*fromRatio, fromColor.a*fromRatio) + new Color(toColor.r*toRatio, toColor.g*toRatio, toColor.b*toRatio, toColor.a*toRatio);
					}
				}
			}
			
			noiseTex.SetPixels(pix);
			noiseTex.Apply();
		}
	}
	
	void OnDrawGizmosSelected(){
		init();

		Gizmos.DrawGUITexture( new Rect(0f,0f,Screen.height,Screen.height), noiseTex);
	}

	float turbulence( float x, float y, float size ){
	    float value = 0.0f;
	    float initialSize = size;
	    
	    while(size >= 1) {
	        value += Mathf.PerlinNoise(x / size, y / size) * size;
	        size /= 2.0f;
	    }
	    
	    return(value / initialSize);
	}
}
