// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Interfaces;
using RealityToolkit.InputSystem.Interfaces.Controllers.Hands;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using RealityToolkit.InputSystem.Listeners;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Interactors
{
    /// <summary>
    /// The <see cref="InteractorRegistrar"/> is responsible for automatic detection of
    /// new <see cref="IInteractor"/>s and manages their registration with the <see cref="IInteractionService"/>.
    /// </summary>
    public class InteractorRegistrar : InputSystemGlobalListener, IMixedRealitySourceStateHandler
    {
        private IMixedRealityInputSystem inputService;
        private IInteractionService interactionService;

        /// <summary>
        /// Executed when the <see cref="InteractorRegistrar"/> is loaded the first time.
        /// </summary>
        private void Awake()
        {
            if (!ServiceManager.Instance.TryGetService(out interactionService))
            {
                Debug.LogError($"{nameof(InteractorRegistrar)} requires the {nameof(IInteractionService)} to work.");
                gameObject.Destroy();
                return;
            }

            if (!ServiceManager.Instance.TryGetService(out inputService))
            {
                Debug.LogError($"{nameof(InteractorRegistrar)} requires the {nameof(IMixedRealityInputSystem)} to work.");
                gameObject.Destroy();
                return;
            }

            foreach (var inputSource in inputService.DetectedInputSources)
            {
                RegisterInputSource(inputSource);
            }
        }

        private void RegisterInputSource(IMixedRealityInputSource inputSource)
        {
            if (interactionService.TryGetInteractor(inputSource, out _))
            {
                return;
            }

            Interactor interactor = null;
            if (inputService.TryGetController(inputSource, out var controller))
            {
                interactor = (Interactor)controller.Visualizer.GameObject.EnsureComponent(
                    controller is IMixedRealityHandController ?
                    typeof(HandControllerInteractor) :
                    typeof(ControllerInteractor));
            }
            else
            {
                interactor = new GameObject($"{nameof(Interactor)}.{inputSource.SourceName}").AddComponent<Interactor>();
                interactor.transform.SetParent(gameObject.transform);
            }

            interactor.InputSource = inputSource;
        }

        /// <inheritdoc/>
        public void OnSourceDetected(SourceStateEventData eventData) => RegisterInputSource(eventData.InputSource);

        /// <inheritdoc/>
        public void OnSourceLost(SourceStateEventData eventData)
        {
            if (!interactionService.TryGetInteractor(eventData.InputSource, out var interactor))
            {
                return;
            }

            if (interactor is ControllerInteractor)
            {
                // Controller interactors are attached to a controller and thus
                // we only want to destroy the interactor instance but do not touch
                // the controller game object itself as the developer might have other
                // plans for it.
                ((Interactor)interactor).Destroy();
            }
            else
            {
                // Generic interactors have a dedicated game object that has no purpose
                // anymore once the interactor is gone.
                ((Interactor)interactor).gameObject.Destroy();
            }
        }
    }
}
