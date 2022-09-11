// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactables;

namespace RealityToolkit.InteractionSDK.Actions
{
    /// <summary>
    /// This <see cref="IAction"/> will focus lock <see cref="InputSystem.Interfaces.IMixedRealityPointer"/>s on the <see cref="IInteractable"/>
    /// object depending on its <see cref="InteractionState"/>.
    /// </summary>
    public class FocusLockAction : Action
    {
        /// <inheritdoc/>
        public override void OnStateChanged(InteractionState state)
        {
            if (!Interactable.IsValid ||
                Interactable.PrimaryInteractor == null)
            {
                return;
            }

            for (var i = 0; i < Interactable.PrimaryInteractor.InputSource.Pointers.Length; i++)
            {
                Interactable.PrimaryInteractor.InputSource.Pointers[i].IsFocusLocked = state == InteractionState.Selected;
            }
        }
    }
}
