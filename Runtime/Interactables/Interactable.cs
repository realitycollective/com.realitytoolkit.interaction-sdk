// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// An <see cref="Interactable"/> marks an object that can be interacted with.
    /// </summary>
    public class Interactable : MonoBehaviour, IInteractable
    {
        private NearInteractable nearInteractable;
        private FarInteractable farInteractable;

        /// <summary>
        /// Exeuted whenever the <see cref="Interactable"/> is enabled.
        /// </summary>
        private void OnEnable()
        {
            nearInteractable = GetComponent<NearInteractable>();
            farInteractable = GetComponent<FarInteractable>();
        }

        /// <inheritdoc/>
        public bool IsValid => isActiveAndEnabled && (NearInteractionEnabled || FarInteractionEnabled);

        /// <inheritdoc/>
        public bool NearInteractionEnabled => nearInteractable.IsNotNull();

        /// <inheritdoc/>
        public bool FarInteractionEnabled => farInteractable.IsNotNull();
    }
}
