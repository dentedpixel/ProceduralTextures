using UnityEngine;
using UnityEditor;
using System.Collections;

[CanEditMultipleObjects, CustomEditor(typeof(ProceduralTexture))]
public class ProceduralTextureEditor : Editor {

	SerializedProperty isDirtyProp;
	SerializedProperty noiseLayerProp;
	SerializedProperty scaleProp;
	SerializedProperty turbolenceScaleXProp;
	SerializedProperty turbolenceScaleYProp;
	SerializedProperty widthProp;
	SerializedProperty heightProp;
	SerializedProperty textureTypeProp;
	SerializedProperty turbolenceSizeProp;
	SerializedProperty turbolenceStrengthProp;
	SerializedProperty fromColorProp;
	SerializedProperty toColorProp;
	SerializedProperty cutoffProp;

    void OnEnable(){
    	isDirtyProp = serializedObject.FindProperty("isDirty");
    	noiseLayerProp = serializedObject.FindProperty("noiseLayer");
    	textureTypeProp = serializedObject.FindProperty("textureType");
    	turbolenceSizeProp = serializedObject.FindProperty("turbolenceSize");
    	turbolenceScaleXProp = serializedObject.FindProperty("turbolenceScaleX");
    	turbolenceScaleYProp = serializedObject.FindProperty("turbolenceScaleY");
    	turbolenceStrengthProp = serializedObject.FindProperty("turbolenceStrength");
    	scaleProp = serializedObject.FindProperty("scale");
    	widthProp = serializedObject.FindProperty("pixWidth");
    	heightProp = serializedObject.FindProperty("pixHeight");
    	fromColorProp = serializedObject.FindProperty("fromColor");
    	toColorProp = serializedObject.FindProperty("toColor");
    	cutoffProp = serializedObject.FindProperty("cutoff");
    }

    public override void OnInspectorGUI() {
		serializedObject.Update ();

		GUI.changed = false;

		EditorGUILayout.PropertyField( noiseLayerProp );

		EditorGUILayout.PropertyField( textureTypeProp );

		scaleProp.floatValue = EditorGUILayout.Slider("scale", scaleProp.floatValue, 0.0f, 2000.0f, null);

		EditorGUILayout.PropertyField( widthProp );
		if(widthProp.intValue<1)
			widthProp.intValue = 1;
		EditorGUILayout.PropertyField( heightProp );
		if(heightProp.intValue<1)
			heightProp.intValue = 1;

		//EditorGUILayout.BeginHorizontal();
		fromColorProp.colorValue = EditorGUILayout.ColorField("From Color", fromColorProp.colorValue); 
		toColorProp.colorValue = EditorGUILayout.ColorField("To Color", toColorProp.colorValue); 
		//EditorGUILayout.EndHorizontal();

		turbolenceSizeProp.intValue = (int)EditorGUILayout.Slider("turbolence size", turbolenceSizeProp.intValue, 0.0f, 50.0f, null);
		turbolenceScaleXProp.floatValue = EditorGUILayout.Slider("turbolence scale X", turbolenceScaleXProp.floatValue, 0.0f, 3.0f, null);
		turbolenceScaleYProp.floatValue = EditorGUILayout.Slider("turbolence scale Y", turbolenceScaleYProp.floatValue, 0.0f, 3.0f, null);
		turbolenceStrengthProp.floatValue = EditorGUILayout.Slider("turbolence strength", turbolenceStrengthProp.floatValue, 0.0f, 50.0f, null);

		cutoffProp.floatValue = EditorGUILayout.Slider("cutoff", cutoffProp.floatValue, -1.0f, 1.0f, null);

		if (GUI.changed)
			isDirtyProp.boolValue = true;

		serializedObject.ApplyModifiedProperties();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
