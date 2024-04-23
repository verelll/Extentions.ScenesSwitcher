using System.IO;
using Extentions.GUIToolbar;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Extentions.ScenesSwitcher
{
	[InitializeOnLoad]
	public static class ScenesSwitcherToolbar
	{
		private static readonly string[] _scenesPath;
		private static readonly string[] _scenesName;
		private static int _selectedSceneIndex = 0;

		static ScenesSwitcherToolbar()
		{
			var scenes = EditorBuildSettings.scenes;
			_scenesPath = new string[scenes.Length];
			_scenesName = new string[scenes.Length];
			
			for (int sceneIndex = 0; sceneIndex < scenes.Length; sceneIndex++)
			{
				var scenePath = scenes[sceneIndex].path;
				var sceneName = Path.GetFileNameWithoutExtension(scenePath);
				
				_scenesPath[sceneIndex] = scenePath;
				_scenesName[sceneIndex] = sceneName;
			}

			GUIToolbarExtender.AddLeftGUIHandler(OnToolbarGUI);
		}

		private static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();
			if (EditorGUILayout.DropdownButton(new GUIContent(_scenesName[_selectedSceneIndex]), FocusType.Passive))
			{
				GenericMenu menu = new GenericMenu();
				for (int i = 0; i < _scenesName.Length; i++)
				{
					int index = i; 
					menu.AddItem(new GUIContent(_scenesName[i]), false, () => { _selectedSceneIndex = index; });
				}
				menu.ShowAsContext();
			}
			if (GUILayout.Button("Load Scene"))
			{
				EditorApplication.SaveCurrentSceneIfUserWantsTo();
				EditorSceneManager.OpenScene(_scenesPath[_selectedSceneIndex]);
			}
		}
	}
}
