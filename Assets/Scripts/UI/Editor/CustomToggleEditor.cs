using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(CustomToggle))]
[CanEditMultipleObjects]
public class CustomToggleEditor : ToggleEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var prop = serializedObject.FindProperty("OnToggleIsOn");
        EditorGUILayout.PropertyField(prop, true);
        serializedObject.ApplyModifiedProperties();

    }
}
