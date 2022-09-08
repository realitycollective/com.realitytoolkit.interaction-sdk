// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InputSystem.Interfaces.Controllers;

namespace RealityToolkit.InteractionSDK.Interactors
{
    /// <summary>
    /// An <see cref="IInteractor"/> that is operated by an <see cref="IMixedRealityController"/>.
    /// </summary>
    public interface IControllerInteractor : IInteractor
    {
        /// <summary>
        /// The <see cref="IMixedRealityController"/> operating the <see cref="IInteractor"/>.
        /// </summary>
        IMixedRealityController Controller { get; }
    }
}
