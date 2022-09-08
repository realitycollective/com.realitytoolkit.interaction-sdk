// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InputSystem.Interfaces.Controllers;

namespace RealityToolkit.InteractionSDK.Interactors
{
    /// <summary>
    /// A <see cref="ControllerInteractor"/> is an <see cref="IInteractor"/> that is operated
    /// by a controller input device.
    /// </summary>
    public class ControllerInteractor : Interactor, IControllerInteractor
    {
        /// <inheritdoc/>
        public IMixedRealityController Controller { get; set; }
    }
}
