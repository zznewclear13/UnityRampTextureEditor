using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(RampTexture))]
public class RampTextureEditor : Editor
{
    RampTexture rampTexture;
    SerializedProperty widthProp;
    SerializedProperty heightPerRampProp;
    SerializedProperty gradientsProp;
    ReorderableList rList;

    private void OnEnable()
    {
        rampTexture = serializedObject.targetObject as RampTexture;
        widthProp = serializedObject.FindProperty("width");
        heightPerRampProp = serializedObject.FindProperty("heightPerRamp");
        gradientsProp = serializedObject.FindProperty("gradients");

        rList = new ReorderableList(serializedObject, gradientsProp, true, true, true, true);
        
        rList.drawHeaderCallback = (rect)=>
        {
            EditorGUI.LabelField(rect, "Gradients");
        };
        rList.drawElementCallback = (rect, index, isActive, isFucused)=>
        {
            var prop = gradientsProp.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, prop);
        };
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            EditorGUILayout.PropertyField(widthProp);
            EditorGUILayout.PropertyField(heightPerRampProp);
            rList.DoLayoutList();

            if (check.changed)
            {
                serializedObject.ApplyModifiedProperties();
                rampTexture.UpdateTexture();            
            }
        }
    }
}