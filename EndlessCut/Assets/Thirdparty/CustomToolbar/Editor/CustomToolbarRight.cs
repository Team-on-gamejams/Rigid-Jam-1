﻿using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityToolbarExtender {

	[InitializeOnLoad]
	public static class CustomToolbarRight {
		static CustomToolbarRight() {
			ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI() {
			EditorGUIUtility.SetIconSize(new Vector2(17, 17));
			if (GUILayout.Button(new GUIContent((Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Thirdparty/CustomToolbar/Editor/Icons/LookDevResetEnv@2x.png", typeof(Texture2D))), ToolbarStyles.commandButtonStyle)) {
				if (EditorApplication.isPlaying) {
					SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				}
			}

			Time.timeScale = EditorGUILayout.Slider("", Time.timeScale, 0.1f, 2f, GUILayout.Width(150));
		}
	}
}