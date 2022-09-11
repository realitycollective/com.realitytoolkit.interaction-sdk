// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace RealityToolkit.InteractionSDK.Actions
{
    /// <summary>
    /// An <see cref="IAction"/> is an action performed when the <see cref="Interactables.IInteractable"/> is being interacted with.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// Activates the action.
        /// </summary>
        void Activate();

        /// <summary>
        /// Resets the action and performs clean up needed to release the <see cref="Interactables.IInteractable"/>
        /// fro the <see cref="IAction"/>s effect.
        /// </summary>
        void OnReset();
    }
}
