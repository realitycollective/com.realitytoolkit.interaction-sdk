// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// An <see cref="IInteractable"/> marks an object that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Is the interactablel valid for interaciton?
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Does the interactable allow near interaction?
        /// </summary>
        bool NearInteractionEnabled { get; }

        /// <summary>
        /// Does the interactable allow far interaction?
        /// </summary>
        bool FarInteractionEnabled { get; }
    }
}
