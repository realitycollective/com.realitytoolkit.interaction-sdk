// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Services;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Interactables
{
    /// <summary>
    /// A <see cref="SelectInteractable"/> is an <see cref="IInteractable"/> that can be
    /// interacted with via grab interaction.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Interactable))]
    public class GrabInteractable : MonoBehaviour, IMixedRealityInputHandler
    {
        private Interactable interactable;
        private IInteractionService interactionService;

        /// <summary>
        /// Executed when the <see cref="GrabInteractable"/> is loaded the first time.
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
                eventData.MixedRealityInputAction != interactionService.GrabInputAction)
            {
                return;
            }

            eventData.Use();
        }

        /// <inheritdoc/>
        public void OnInputUp(InputEventData eventData)
        {

        }
    }
}
