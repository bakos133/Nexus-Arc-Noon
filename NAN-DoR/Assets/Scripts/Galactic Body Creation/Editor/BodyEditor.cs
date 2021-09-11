using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(BodyGenerator))]
public class BodyEditor : Editor
{
    BodyGenerator body;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //DrawSettingsEditor(body.shapeSettings, body.OnShapeSettingsUpdated, ref body.shapeSF);
      //  DrawSettingsEditor(body.colorSettings, body.OnColorSettingsUpdated, ref body.colorSF);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout)
    {
        using (var check = new EditorGUI.ChangeCheckScope()) 
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            if (foldout)
            {
                Editor editor = CreateEditor(settings);
                editor.OnInspectorGUI();

                if (check.changed)
                {
                    if (onSettingsUpdated != null)
                    {
                        onSettingsUpdated();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        body = (BodyGenerator)target;
    }
}