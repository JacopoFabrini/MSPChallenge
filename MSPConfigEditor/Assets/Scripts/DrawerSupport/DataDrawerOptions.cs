using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ColourPalette;
using System;

public enum CursorState { Default, Scale }

[CreateAssetMenu(fileName = "DataDrawerOption", menuName = "MSP Config")]
public class DataDrawerOptions : SerializedScriptableObject
{
    [SerializeField]
    float m_editorVersion;

    [SerializeField]
    Dictionary<Type, GameObject> m_drawerPrefabs;

    [SerializeField]
    Dictionary<Type, GameObject> m_spacerPrefabs;

    [SerializeField]
    Dictionary<Type, GameObject> m_drawerSupportPrefabs;

    [SerializeField]
    Dictionary<ContextMenuSprite, Sprite> m_contextMenuSprites;

    [SerializeField]
    GameObject m_listMovePreviewPrefab;

    [SerializeField]
    GameObject m_listElementIndexerPrefab;

    [SerializeField]
    ColourAsset m_fatalErrorColour;
    [SerializeField]
    ColourAsset m_errorColour;
    [SerializeField]
    ColourAsset m_warningColour;

    [SerializeField]
    Texture2D m_cursorDefault;
    [SerializeField]
    Texture2D m_cursorScale;

    public GameObject ListMovePreviewPrefab { get => m_listMovePreviewPrefab; }
    public GameObject ListElementIndexerPrefab { get => m_listElementIndexerPrefab; }
    public ColourAsset FatalErrorColour { get => m_fatalErrorColour; }
    public ColourAsset ErrorColour { get => m_errorColour; }
    public ColourAsset WarningColour { get => m_warningColour; }
    public float EditorVersion { get => m_editorVersion; }

    public GameObject GetDrawerForType(Type a_type)
    {
        GameObject prefab;
        if (!m_drawerPrefabs.TryGetValue(a_type, out prefab))
            Debug.LogError("Couldnt find drawer for type: " + a_type.ToString());
        return prefab;
    }

    public GameObject GetSpacerForType(Type a_type)
    {
        GameObject prefab;
        if (!m_spacerPrefabs.TryGetValue(a_type, out prefab))
            Debug.LogError("Couldnt find spacer for type: " + a_type.ToString());
        return prefab;
    }

    public GameObject GetDrawerSupportForType(Type a_type)
    {
        GameObject prefab;
        if (!m_drawerSupportPrefabs.TryGetValue(a_type, out prefab))
            Debug.LogError("Couldnt find drawer support for type: " + a_type.ToString());
        return prefab;
    }

    public Sprite GetContextMenuSprite(ContextMenuSprite a_spriteType)
    {
        return m_contextMenuSprites[a_spriteType];
    }

    public void SetCursorState(CursorState a_newState)
    {
        switch (a_newState)
        {
            case CursorState.Scale:
                Cursor.SetCursor(m_cursorScale, Vector2.zero, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(m_cursorDefault, Vector2.zero, CursorMode.Auto);
                break;
        }
    }
}
