// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.InputSystem.Interfaces;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Interactors
{
    /// <summary>
    /// An <see cref="Interactor"/> marks an object that can interact with <see cref="Interactables.IInteractable"/>s.
    /// </summary>
    [DisallowMultipleComponent]
    public class Interactor : MonoBehaviour, IInteractor
    {
        /// <inheritdoc/>
        public IMixedRealityInputSource InputSource { get; set; }

        /// <summary>
        /// Executed when the <see cref="Interactor"/> is loaded the first time.
        /// </summary>a
        protected virtual void Awake()
        {
            if (!ServiceManager.Instance.TryGetService<IInteractionService>(out var interactionService))
            {
                Debug.LogError($"{nameof(Interactor)} requires the {nameof(IInteractionService)} to work.");
                this.Destroy();
                return;
            }

            interactionService.Add(this);
        }

        /// <summary>
        /// Executed when the <see cref="Interactor"/> is about to be destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (!ServiceManager.Instance.TryGetService<IInteractionService>(out var interactionService))
            {
                return;
            }

            interactionService.Remove(this);
        }
    }
}
