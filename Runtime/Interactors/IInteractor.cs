// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InputSystem.Interfaces;

namespace RealityToolkit.InteractionSDK.Interactors
{
    /// <summary>
    /// An <see cref="IInteractor"/> marks an object that can interact with <see cref="Interactables.IInteractable"/>s.
    /// </summary>
    public interface IInteractor
    {
        /// <summary>
        /// The registered <see cref="IMixedRealityInputSource"/> for this interactor.
        /// </summary>
        IMixedRealityInputSource InputSource { get; }

        /// <summary>
        /// Does the <see cref="IInteractor"/> support near interaction?
        /// </summary>
        bool NearInteractionEnabled { get; }

        /// <summary>
        /// Does the <see cref="IInteractor"/> support far interaction?
        /// </summary>
        bool FarInteractionEnabled { get; }
    }
}
