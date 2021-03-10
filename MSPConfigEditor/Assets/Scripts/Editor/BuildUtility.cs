using UnityEditor;

class BuildUtility
{
	[MenuItem("MSPConfigEditor/Build project")]
	public static void MyBuild()
	{
		ConfigEditorSettings settings = null;
		string[] assetGUIDs = AssetDatabase.FindAssets("ConfigEditorSettings");
		if(assetGUIDs == null || assetGUIDs.Length == 0)
		{
			EditorUtility.DisplayDialog("Build Config Editor", "No ConfigEditorSettings found. Please add a ConfigEditorSettings asset to the project.", "Ok");
			return;
		}
		else
		{
			settings = (ConfigEditorSettings)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assetGUIDs[0]), typeof(ConfigEditorSettings));
			if(settings == null)
			{
				EditorUtility.DisplayDialog("Build Config Editor", "No ConfigEditorSettings found. Please add a ConfigEditorSettings asset to the project.", "Ok");
				return;
			}
		}

		//Build a dev and non-dev player
		if (EditorUtility.DisplayDialog("Build Config Editor", $"Config editor will be built with a minimum client version of [{(string.IsNullOrEmpty(settings.m_minSupportedClientVersion) ? "none" : settings.m_minSupportedClientVersion)}] and a maximum version of [{(string.IsNullOrEmpty(settings.m_maxSupportedClientVersion) ? "none" : settings.m_maxSupportedClientVersion)}]. If this is incorrect pelase edit the ConfigEditorSettings asset.", "Confirm", "Cancel"))
		{
			string path = EditorUtility.SaveFolderPanel("Choose folder to build game", "", "");
			if (path.Length != 0)
			{
				BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + "/MSPConfigEditor.exe", EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
				//BuildPipeline.BuildPlayer(EditorBuildSettings.scenes, path + "/DevBuild/msp_dev.exe", EditorUserBuildSettings.activeBuildTarget, BuildOptions.Development);
			}
		}
	}
}

