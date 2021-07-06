using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ConfigEditorSettings", menuName = "ConfigEditor/ConfigEditorSettings")]
public class ConfigEditorSettings : SerializedScriptableObject
{
	public string m_minSupportedClientVersion;
	public string m_maxSupportedClientVersion;
}