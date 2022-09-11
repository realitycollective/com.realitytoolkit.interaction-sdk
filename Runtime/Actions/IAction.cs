// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactables;

namespace RealityToolkit.InteractionSDK.Actions
{
    /// <summary>
    /// An <see cref="IAction"/> is an action performed when the <see cref="IInteractable"/> is being interacted with.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Handles a change in the <see cref="IInteractable.State"/>.
        /// </summary>
        /// <param name="state">The new <see cref="InteractionState"/> of the <see cref="IInteractable"/>.</param>
        void OnStateChanged(InteractionState state);
    }
}
