// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactables;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Actions
{
    /// <summary>
    /// This <see cref="IAction"/> will change the main material used on a <see cref="MeshRenderer"/> on the <see cref="IInteractable"/>
    /// depending on its <see cref="InteractionState"/>.
    /// </summary>
    public class ChangeMaterialAction : Action
    {
        [SerializeField]
        private Material normalMaterial = null;

        [SerializeField]
        private Material focusedMaterial = null;

        [SerializeField]
        private Material selectedMaterial = null;

        [SerializeField]
        private MeshRenderer meshRenderer = null;

        /// <inheritdoc/>
        public override void OnStateChanged(InteractionState state)
        {
            if (!Interactable.IsValid)
            {
                return;
            }

            switch (state)
            {
                case InteractionState.Normal:
                    meshRenderer.material = normalMaterial;
                    break;
                case InteractionState.Focused:
                    meshRenderer.material = focusedMaterial;
                    break;
                case InteractionState.Selected:
                    meshRenderer.material = selectedMaterial;
                    break;
            }
        }
    }
}