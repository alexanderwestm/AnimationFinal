using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
Asset Selector
Position Snapping Option (Choose rounding direction or just round?) (Choose what value to round to)
Rotation and scale selection
    Random Option
    Or Slider Range
 * */

[CustomEditor(typeof(PlaceObject))]
public class PlaceObjectEditor : Editor
{
    //GameObject[] prefabs;
	static GameObject selectedPrefab;
    static List<GameObject> spawnedObjects = new List<GameObject>();

    static bool autoAdjust = true;

    enum RoundType
    {
        NORMAL = 0,
        UP,
        DOWN
    }
    RoundType roundingType;

    static bool areRoundingX, areRoundingY, areRoundingZ;
    static float roundingNumX = 0, roundingNumY = 0, roundingNumZ = 0;


    static bool areRandomRotation;
    static Vector3 lowerRandomRotation = new Vector3(0, 0, 0);
    static Vector3 upperRandomRotation  =  new Vector3(360, 360, 360);
    static Vector3 setRotation;

    static bool areRandomScale;
    static Vector3 lowerRandomScale = new Vector3(1, 1, 1);
    static Vector3 upperRandomScale = new Vector3(50, 50, 50);
    static Vector3 setScale = new Vector3(1, 1, 1);

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		selectedPrefab = (GameObject)EditorGUILayout.ObjectField(selectedPrefab, typeof(Object), true);
        EditorGUILayout.Space();

        autoAdjust = EditorGUILayout.Toggle("Auto Adjust Position on Face", autoAdjust);
        EditorGUILayout.Space();

        if(areRoundingX || areRoundingY || areRoundingZ)
        {
            roundingType = (RoundType)EditorGUILayout.EnumPopup("How To Round", roundingType);
        }

        areRoundingX = EditorGUILayout.Toggle("Round X Position", areRoundingX);
        if(areRoundingX)
        {
            roundingNumX = EditorGUILayout.FloatField("", roundingNumX);
        }

        areRoundingY = EditorGUILayout.Toggle("Round Y Position", areRoundingY);
        if (areRoundingY)
        {
            roundingNumY = EditorGUILayout.FloatField("", roundingNumY);
        }

        areRoundingZ = EditorGUILayout.Toggle("Round Z Position", areRoundingZ);
        if (areRoundingZ)
        {
            roundingNumZ = EditorGUILayout.FloatField("", roundingNumZ);
        }

        //areRoundingPosition = EditorGUILayout.Toggle("Round To Nearest", areRoundingPosition);
        //if (areRoundingPosition)
        //{
        //    roundingType = (RoundType)EditorGUILayout.EnumPopup("How To Round", roundingType);
        //    roundingNumber = EditorGUILayout.FloatField("Round To What", roundingNumber);
        //}
        EditorGUILayout.Space();

        areRandomRotation = EditorGUILayout.Toggle("Randomize Rotation", areRandomRotation);
        if(areRandomRotation)
        {
            lowerRandomRotation = EditorGUILayout.Vector3Field("Lower Bounds", lowerRandomRotation);
            upperRandomRotation = EditorGUILayout.Vector3Field("Upper Bounds", upperRandomRotation);
        }
        else
        {
            setRotation = EditorGUILayout.Vector3Field("Degrees Rotate", setRotation);
        }
        EditorGUILayout.Space();

        areRandomScale = EditorGUILayout.Toggle("Randomize Scale", areRandomScale);
        if(areRandomScale)
        {
            lowerRandomScale = EditorGUILayout.Vector3Field("Lower Bounds", lowerRandomScale);
            upperRandomScale = EditorGUILayout.Vector3Field("Upper Bounds", upperRandomScale);
        }
        else
        {
            setScale = EditorGUILayout.Vector3Field("Scale", setScale);
        }
	}

	private void OnSceneGUI()
	{
		Ray spawnRay = HandleUtility.GUIPointToWorldRay(UnityEngine.Event.current.mousePosition);
		if(UnityEngine.Event.current.type == UnityEngine.EventType.KeyDown)
		{
            if (UnityEngine.Event.current.keyCode == KeyCode.E)
            {
                Spawn(spawnRay);
            }
            else if(UnityEngine.Event.current.keyCode == KeyCode.X)
            {
                if (spawnedObjects.Count > 0)
                { 
                    GameObject temp = spawnedObjects[spawnedObjects.Count - 1];
                    spawnedObjects.Remove(temp);
                    DestroyImmediate(temp);
                }
            }
		}
	}

	private void Spawn(Ray spawnRay)
	{
		RaycastHit hit;
		if (Physics.Raycast(spawnRay, out hit, 1000))
		{
			GameObject spawnObject = Instantiate(selectedPrefab, hit.point, selectedPrefab.transform.rotation);
            Vector3 rotation, scale, positionChange = new Vector3(0, 0, 0);
            if (autoAdjust)
            {
                Vector3 size = spawnObject.GetComponent<Collider>().bounds.size;
                Vector3 normal = hit.normal;
                spawnObject.transform.position += new Vector3(size.x * normal.x, size.y * normal.y, size.z * normal.z) / 2;
            }

            if (areRoundingX && roundingNumX != 0)
            {
                positionChange.x = RoundNum(spawnObject.transform.position.x, roundingNumX);
            }
            if(areRoundingY && roundingNumY != 0)
            {
                positionChange.y = RoundNum(spawnObject.transform.position.y, roundingNumY);
            }
            if(areRoundingZ && roundingNumZ != 0)
            {
                positionChange.z = RoundNum(spawnObject.transform.position.z, roundingNumZ);
            }
            if (areRandomRotation)
            {
                rotation = new Vector3( Random.Range(lowerRandomRotation.x, upperRandomRotation.x),
                                        Random.Range(lowerRandomRotation.y, upperRandomRotation.y),
                                        Random.Range(lowerRandomRotation.z, upperRandomRotation.z));
            }
            else
            {
                rotation = setRotation;
            }
            if(areRandomScale)
            {
                scale = new Vector3(Random.Range(lowerRandomScale.x, upperRandomScale.x), 
                                    Random.Range(lowerRandomScale.y, upperRandomScale.y), 
                                    Random.Range(lowerRandomScale.z, upperRandomScale.z));
            }
            else
            {
                scale = setScale;
            }

            Debug.Log(positionChange);
            spawnObject.transform.position += positionChange;
            spawnObject.transform.rotation = Quaternion.Euler(rotation);
            spawnObject.transform.localScale = scale;

            spawnedObjects.Add(spawnObject);
		}
	}

    public float RoundNum(float value, float roundNum)
    {
        if(roundNum == 0)
        {
            return 0;
        }

        float remainder = 0.0f;
        bool round = false;

        remainder = Mathf.Abs(value) % roundNum;
        Debug.Log("Remainder: " + remainder);
        Debug.Log("Value: " + value);
        Debug.Log("Rounding: " + roundNum);
        if(remainder >= roundNum / 2.0f)
        {
            round = true;
        }
        else
        {
            round = false;
        }

        round = ((roundingType == RoundType.NORMAL && round) || roundingType == RoundType.UP);

        if (round)
        {
            return remainder;
        }
        return -(roundNum - remainder);
    }

}
