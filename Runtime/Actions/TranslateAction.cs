// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactors;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Actions
{
    public class TranslateAction : Action, IAction
    {
        private Vector3? previousInteractorPosition;
        private IControllerInteractor primaryInteractor;

        private void Update()
        {
            var interactorPosition = primaryInteractor.Controller.Visualizer.GameObject.transform.position;
            var delta = interactorPosition - previousInteractorPosition.Value;
            transform.Translate(delta);
            previousInteractorPosition = interactorPosition;
        }

        /// <inheritdoc/>
        public override void Activate()
        {
            // This action only supports controller interactors.
            if (!Interactable.IsValid ||
                Interactable.PrimaryInteractor == null ||
                Interactable.PrimaryInteractor is not IControllerInteractor primaryInteractor)
            {
                return;
            }

            this.primaryInteractor = primaryInteractor;
            previousInteractorPosition = primaryInteractor.Controller.Visualizer.GameObject.transform.position;
            base.Activate();
        }
    }
}
