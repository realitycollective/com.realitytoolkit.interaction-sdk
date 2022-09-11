// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactors;
using System.Collections.Generic;

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// An <see cref="IInteractable"/> marks an object that can be interacted with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Gets or sets the <see cref="IInteractable"/>s label that may be used to
        /// identify the interactable or categorize it
        /// </summary>
        string Label { get; set; }

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

        /// <summary>
        /// The current <see cref="InteractionState"/> of the <see cref="IInteractable"/>.
        /// </summary>
        InteractionState State { get; }

        /// <summary>
        /// Gets the primary <see cref="IInteractor"/>. That is the first one
        /// to start interaction with the <see cref="IInteractable"/>.
        /// </summary>
        /// <remarks><c>null</c> if not interacted with.</remarks>
        IInteractor PrimaryInteractor { get; }

        /// <summary>
        /// Gets all <see cref="IInteractor"/>s currently interacting with the <see cref="IInteractable"/>.
        /// </summary>
        /// <remarks>Empty if not interacted with.</remarks>
        IReadOnlyList<IInteractor> Interactors { get; }
    }
}
