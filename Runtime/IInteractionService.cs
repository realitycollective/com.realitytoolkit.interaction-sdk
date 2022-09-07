// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Interfaces;
using RealityToolkit.InteractionSDK.Interactables;
using RealityToolkit.InteractionSDK.Interactors;
using System.Collections.Generic;

namespace RealityToolkit.InteractionSDK
{
    /// <summary>
    /// The <see cref="IInteractionService"/> provides easy access to <see cref="IInteractable"/>s
    /// and <see cref="IInteractor"/>s and interaction state in general.
    /// </summary>
    public interface IInteractionService : IService
    {
        /// <summary>
        /// Available <see cref="IInteractor"/>s in the scene.
        /// </summary>
        IReadOnlyList<IInteractor> Interactors { get; }

        /// <summary>
        /// Available <see cref="IInteractable"/>s in the scene.
        /// </summary>
        IReadOnlyList<IInteractable> Interactables { get; }

        /// <summary>
        /// Adds an <see cref="IInteractor"/> to the service's registry.
        /// </summary>
        /// <param name="interactor">The <see cref="IInteractor"/> to add.</param>
        void Add(IInteractor interactor);

        /// <summary>
        /// Removes an <see cref="IInteractor"/> from the service's registry.
        /// </summary>
        /// <param name="interactor">The <see cref="IInteractor"/> to remove.</param>
        void Remove(IInteractor interactor);

        /// <summary>
        /// Adds an <see cref="IInteractable"/> to the service's registry.
        /// </summary>
        /// <param name="interactable">The <see cref="IInteIInteractableractor"/> to add.</param>
        void Add(IInteractable interactable);

        /// <summary>
        /// Removes an <see cref="IInteractable"/> from the service's registry.
        /// </summary>
        /// <param name="interactable">The <see cref="IInteractable"/> to remove.</param>
        void Remove(IInteractable interactable);
    }
}