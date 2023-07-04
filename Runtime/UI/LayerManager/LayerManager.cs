#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/// <summary>
/// Helper class for managing layers in Unity.
/// </summary>
public static class LayerManager
{
    private static readonly int FAILED = -1;

    /// <summary>
    /// Adds or sets the layer of the specified GameObject.
    /// If the layer does not exist, it tries to create it.
    /// </summary>
    /// <param name="gameObject">The GameObject to modify.</param>
    /// <param name="layerName">The name of the layer.</param>
    /// <returns>True if the layer was added or set successfully, false otherwise.</returns>
    public static bool AddSetLayer(GameObject gameObject, string layerName)
    {
        var done = SetLayer(gameObject, layerName);

        if (done)
        {
            return true;
        }

        var layerIndex = CreateNewLayer(layerName);

        if (layerIndex != FAILED)
        {
            gameObject.layer = layerIndex;
            return true;
        }
        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Creates a new layer with the specified name.
    /// </summary>
    /// <param name="layerName">The name of the layer.</param>
    /// <remarks> Only available within the Editor. Do not use in runtime code.</remarks>
    /// <returns>The index of the new layer if creation was successful, -1 otherwise.</returns>
    public static int CreateNewLayer(string layerName)
    {
        SerializedObject tagManager = new SerializedObject(
        AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);

        SerializedProperty layersProp = tagManager.FindProperty("layers");

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

        if (layerIndex == FAILED)
        {
            Debug.LogError("Unable to create new layer - all layer indices are used.");
            return FAILED;
        }

        SerializedProperty newLayerProp = layersProp.GetArrayElementAtIndex(layerIndex);
        newLayerProp.stringValue = layerName;

        tagManager.ApplyModifiedProperties();

        return layerIndex;
    }

#endif

    /// <summary>
    /// Sets the layer of the specified GameObject.
    /// </summary>
    /// <param name="gameObject">The GameObject to modify.</param>
    /// <param name="layerName">The name of the layer.</param>
    /// <returns>True if the layer was set successfully, false if the layer does not exist.</returns>
    public static bool SetLayer(GameObject gameObject, string layerName)
    {
        int layerIndex = LayerMask.NameToLayer(layerName);

        if (layerIndex == FAILED)
        {
            return false;
        }

        gameObject.layer = layerIndex;
        return true;
    }

}