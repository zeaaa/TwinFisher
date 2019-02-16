using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(TFObject))]
public class TFObjectInspector : Editor {

    SerializedProperty m_enum;
    void OnEnable()
    {
        //tfOject = (check)target;
        m_enum = serializedObject.FindProperty("objectTpye");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_enum);

        if (m_enum.enumValueIndex == 0) {
           
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("_scoreValue"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("_length"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("_weight"));
            EditorGUILayout.PropertyField(this.serializedObject.FindProperty("_speed"));
            //GUI.color = Color.red;
        }
        
 
        serializedObject.ApplyModifiedProperties();
    }

}
