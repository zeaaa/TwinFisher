using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//[AttributeUsage(AttributeTargets.Field)]
public class RenameAttribute : PropertyAttribute {
    public string name;

    public RenameAttribute(string name)
    {
        this.name = name;
    }

}
#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(RenameAttribute))]
public class RenameDrawer : PropertyDrawer
{
    //替换属性名称

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RenameAttribute rename = (RenameAttribute)attribute;
        label.text = rename.name;
        EditorGUI.PropertyField(position, property, label);
    }
    
}
#endif