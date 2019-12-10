using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

enum SaveTypes
{
    BOX_COLLIDER = 0,
    ALL_SUPPORTED
}

public class AnimationEditorWindow : EditorWindow
{
    GameObject editingGameObject;
    AnimationClip animationClip;
    float minSampleValue = 0;
    float maxSampleValue;

    float sampleValue;
    float lastFrameSample = -1;

    SaveTypes type;

    BoxColliderSerializables boxColliderSerializables;

    [MenuItem("Window/Animation Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(AnimationEditorWindow));
    }

    private void OnGUI()
    {
        editingGameObject = EditorGUILayout.ObjectField("Model To Edit", editingGameObject, typeof(GameObject), true) as GameObject;
        if (editingGameObject != null)
        {
            animationClip = EditorGUILayout.ObjectField("Clip To Edit", animationClip, typeof(AnimationClip), true) as AnimationClip;
            if (animationClip != null)
            {
                maxSampleValue = animationClip.length;
                sampleValue = EditorGUILayout.Slider("SampleLocation", sampleValue, minSampleValue, maxSampleValue);
                if (lastFrameSample != sampleValue)
                {
                    animationClip.SampleAnimation(editingGameObject, sampleValue);
                }
                lastFrameSample = sampleValue;

                type = (SaveTypes)EditorGUILayout.EnumPopup("Component To Save", type);

                if (GUILayout.Button("Set Keyframe"))
                {
                    switch (type)
                    {
                        case SaveTypes.BOX_COLLIDER:
                        {
                            CreateBoxColliderKeyFrame();
                            break;
                        }
                        case SaveTypes.ALL_SUPPORTED:
                        {
                            break;
                        }
                        default:
                            break;
                    }
                }
                if (GUILayout.Button("Save Object Data"))
                {
                    switch (type)
                    {
                        case SaveTypes.BOX_COLLIDER:
                        {
                            SaveBoxColliders();
                            break;
                        }
                        case SaveTypes.ALL_SUPPORTED:
                        {
                            break;
                        }
                        default:
                            break;
                    }
                }
            }

            if (GUILayout.Button("Reset Object"))
            {
                animationClip = null;
                foreach (Transform transform in editingGameObject.GetComponentsInChildren<Transform>())
                {
                    PrefabUtility.RevertObjectOverride(transform, InteractionMode.UserAction);
                }
            }
        }
    }

    private void CreateBoxColliderKeyFrame()
    {
        if(boxColliderSerializables == null)
        {
            boxColliderSerializables = CreateInstance<BoxColliderSerializables>();
        }
        BoxColliderKeyframe keyframeTemp = new BoxColliderKeyframe();
        foreach (BoxCollider collider in editingGameObject.GetComponentsInChildren<BoxCollider>())
        {
            keyframeTemp.Add(new BoxColliderSerializable() { gameObjectName = collider.name, value = new BoxColliderData(collider) });
        }
        keyframeTemp.sampleTime = sampleValue;
        boxColliderSerializables.Insert(keyframeTemp);
    }

    private void SaveBoxColliders()
    {
        if (boxColliderSerializables != null)
        {
            AssetDatabase.CreateAsset(boxColliderSerializables, "Assets/GenerateScriptableObjects/" + animationClip.name + ".asset");
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = boxColliderSerializables;
            boxColliderSerializables = null;
        }
    }
}
