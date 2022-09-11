// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.InputSystem.Interfaces;
using RealityToolkit.InteractionSDK.Interactables;
using System.Collections.Generic;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Interactors
{
    /// <summary>
    /// An <see cref="Interactor"/> marks an object that can interact with <see cref="Interactables.IInteractable"/>s.
    /// </summary>
    [DisallowMultipleComponent]
    public class Interactor : MonoBehaviour, IInteractor
    {
        private IInteractionService interactionService;
        private readonly Dictionary<uint, Interactable> focusedInteractables = new Dictionary<uint, Interactable>();

        /// <inheritdoc/>
        public IMixedRealityInputSource InputSource { get; set; }

        /// <inheritdoc/>
        public IMixedRealityPointer Pointer { get; set; }

        /// <summary>
        /// Executed when the <see cref="Interactor"/> is loaded the first time.
        /// </summary>a
        protected virtual async void Awake()
        {
            try
            {
                interactionService = await ServiceManager.Instance.GetServiceAsync<IInteractionService>();
            }
            catch (System.Exception)
            {
                Debug.LogError($"{nameof(Interactor)} requires the {nameof(IInteractionService)} to work.");
                this.Destroy();
                return;
            }

            // We've been destroyed during the await.
            if (this == null) { return; }

            interactionService.Add(this);
        }

        /// <summary>
        /// Executed every frame as long as the <see cref="Interactor"/> is enabled.
        /// </summary>
        protected virtual void Update()
        {
            for (var i = 0; i < InputSource.Pointers.Length; i++)
            {
                var pointer = InputSource.Pointers[i];
                if (pointer.IsInteractionEnabled &&
                    pointer.Result.CurrentPointerTarget.IsNotNull() &&
                    pointer.Result.CurrentPointerTarget.TryGetComponent<Interactable>(out var interactable))
                {
                    focusedInteractables.EnsureDictionaryItem(pointer.PointerId, interactable, true);
                    Pointer = pointer;
                    interactable.OnFocused(this);
                }
                else if (focusedInteractables.TryGetValue(pointer.PointerId, out var unfocusedInteractable))
                {
                    unfocusedInteractable.OnUnfocused(this);
                    Pointer = null;
                    focusedInteractables.SafeRemoveDictionaryItem(pointer.PointerId);
                }
            }
        }

        /// <summary>
        /// Executed when the <see cref="Interactor"/> is disabled.
        /// </summary>
        protected virtual void OnDisable()
        {
            foreach (var item in focusedInteractables)
            {
                item.Value.OnUnfocused(this);
            }

            focusedInteractables.Clear();
        }

        /// <summary>
        /// Executed when the <see cref="Interactor"/> is about to be destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (interactionService == null)
            {
                return;
            }

            interactionService.Remove(this);
        }
    }
}
