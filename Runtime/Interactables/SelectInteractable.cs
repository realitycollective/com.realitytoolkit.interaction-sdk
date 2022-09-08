// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Services;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using UnityEngine;
using UnityEngine.Events;

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// A <see cref="SelectInteractable"/> is an <see cref="IInteractable"/> that can be
    /// interacted with via select interaction.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Interactable))]
    public class SelectInteractable : MonoBehaviour, IMixedRealityInputHandler
    {
        [SerializeField]
        [Tooltip("Event raised whenever the interactable was selected.")]
        private UnityEvent selected = null;

        private Interactable interactable;
        private IInteractionService interactionService;

        /// <summary>
        /// Executed when the <see cref="SelectInteractable"/> is loaded the first time.
        /// </summary>
        private async void Awake()
        {
            interactable = GetComponent<Interactable>();
            interactionService = await ServiceManager.Instance.GetServiceAsync<IInteractionService>();
        }

        /// <inheritdoc/>
        public void OnInputDown(InputEventData eventData)
        {
            if (eventData.used ||
                eventData.MixedRealityInputAction != interactionService.SelectInputAction ||
                !interactionService.TryGetInteractor(eventData.InputSource, out var interactor))
            {
                return;
            }

            eventData.Use();
            interactable.OnSelected(interactor);
            selected?.Invoke();
        }

        /// <inheritdoc/>
        public void OnInputUp(InputEventData eventData)
        {
            if (eventData.MixedRealityInputAction != interactionService.SelectInputAction)
            {
                return;
            }

            interactionService.TryGetInteractor(eventData.InputSource, out var interactor);
            interactable.OnDeselected(interactor);
        }
    }
}
