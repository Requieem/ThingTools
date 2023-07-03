#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class LayerManager
{
    // LayerMask.NameToLayer returns -1 if the layer doesn't exist
    private static readonly int FAILED = -1;

#if UNITY_EDITOR
    public static bool AddSetLayer(GameObject gameObject, string layerName)
    {
        // Check if the layer exists
        var done = SetLayer(gameObject, layerName);

        // If the layer exists, return now
        if (done) return true;

        // If not, try to create it
        var layerIndex = CreateNewLayer(layerName);

        // If the creation was unsuccesful, leave the index as-is
        if (layerIndex != FAILED)
        {
            gameObject.layer = layerIndex;
            return true;
        }
        else return false;
    }
    public static int CreateNewLayer(string layerName)
    {
        SerializedObject tagManager = new(
            AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty layersProp = tagManager.FindProperty("layers");

        // Find the next available layer index
        int layerIndex = FAILED;
        for (int i = 8; i < layersProp.arraySize; i++)
        {
            SerializedProperty layerProp = layersProp.GetArrayElementAtIndex(i);
            if (layerProp.stringValue == "")
            {
                layerIndex = i;
                break;
            }
        }

        // If all layer indices are used, return -1
        if (layerIndex == FAILED)
        {
            Debug.LogError("Unable to create new layer - all layer indices are used.");
            return FAILED;
        }

        // Set the new layer name and assign it to the game object
        SerializedProperty newLayerProp = layersProp.GetArrayElementAtIndex(layerIndex);
        newLayerProp.stringValue = layerName;

        tagManager.ApplyModifiedProperties();

        return layerIndex;
    }
#endif

    public static bool SetLayer(GameObject gameObject, string layerName)
    {
        // Check if the layer exists
        int layerIndex = LayerMask.NameToLayer(layerName);

        // If the layer doesn't exist, create it
        if (layerIndex == FAILED)
        {
            return false;
        }

        // Use the layer index as needed
        gameObject.layer = layerIndex;

        return true;
    }

}
