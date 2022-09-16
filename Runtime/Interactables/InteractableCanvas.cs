// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.InputSystem.Interfaces;
using UnityEngine;

namespace RealityToolkit.Utilities
{
    /// <summary>
    /// Enables interaction on a <see cref="UnityEngine.Canvas"/>.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    public class InteractableCanvas : MonoBehaviour
    {
        [SerializeField]
        private Canvas canvas;

        /// <summary>
        /// The <see cref="UnityEngine.Canvas"/> targeted.
        /// </summary>
        public Canvas Canvas => canvas;

        private void Start()
        {
            if (canvas.IsNull())
            {
                canvas = GetComponent<Canvas>();
            }

            Debug.Assert(Canvas != null);

            if (ServiceManager.Instance.TryGetService<IMixedRealityInputSystem>(out var inputSystem) &&
                Canvas.isRootCanvas && Canvas.renderMode == RenderMode.WorldSpace)
            {
                Canvas.worldCamera = inputSystem.FocusProvider.UIRaycastCamera;
            }
        }
    }
}