// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactables;
using RealityToolkit.InteractionSDK.Interactors;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Actions
{
    public class TranslateAction : Action, IAction
    {
        private Vector3? previousInteractorPosition;
        private IControllerInteractor primaryInteractor;

        /// <summary>
        /// Update is called every frame, if the <see cref="IAction"/> is enabled.
        /// </summary>
        private void Update()
        {
            var interactorPosition = primaryInteractor.Controller.Visualizer.GameObject.transform.position;
            var delta = interactorPosition - previousInteractorPosition.Value;
            transform.Translate(delta);
            previousInteractorPosition = interactorPosition;
        }

        /// <inheritdoc/>
        public override void OnStateChanged(InteractionState state)
        {
            // This action only supports controller interactors.
            if (!Interactable.IsValid ||
                Interactable.PrimaryInteractor == null ||
                Interactable.PrimaryInteractor is not IControllerInteractor primaryInteractor)
            {
                enabled = false;
                return;
            }

            if (state == InteractionState.Selected)
            {
                this.primaryInteractor = primaryInteractor;
                previousInteractorPosition = primaryInteractor.Controller.Visualizer.GameObject.transform.position;
                enabled = true;
            }
            else
            {
                enabled = false;
            }
        }
    }
}
