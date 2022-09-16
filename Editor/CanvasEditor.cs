// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Editor;
using RealityToolkit.InputSystem.Interfaces;
using RealityToolkit.Utilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR && !UNITY_2021_1_OR_NEWER
using SceneManagement = UnityEditor.Experimental.SceneManagement;
#elif UNITY_EDITOR
using SceneManagement = UnityEditor.SceneManagement;
#endif

namespace RealityToolkit.InteractionSDK.Editor
{
    /// <summary>
    /// Extension to the default <see cref="Canvas"/> inspector that makes sure the canvas works with the interaction SDK.
    /// </summary>
    [CustomEditor(typeof(Canvas))]
    public class CanvasEditor : UnityEditor.Editor
    {
        private Canvas canvas;

        private static bool IsServiceManagerValid =>
            ServiceManager.Instance != null &&
            ServiceManager.Instance.HasActiveProfile &&
            ServiceManager.Instance.TryGetService<IMixedRealityInputSystem>(out _);

        private bool ShouldEnsureComponent => !(Application.isPlaying ||
                    SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null ||
                    !IsServiceManagerValid ||
                    !MixedRealityPreferences.ShowCanvasUtilityPrompt);

        private void OnEnable()
        {
            canvas = (Canvas)target;
            if (ShouldEnsureComponent)
            {
                canvas.gameObject.EnsureComponent<InteractableCanvas>();
            }
        }
    }
}