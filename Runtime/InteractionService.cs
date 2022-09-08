// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.InputSystem.Interfaces;
using RealityToolkit.InteractionSDK.Interactables;
using RealityToolkit.InteractionSDK.Interactors;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RealityToolkit.InteractionSDK
{
    /// <summary>
    /// Default implementation for <see cref="IInteractionService"/>.
    /// </summary>
    [System.Runtime.InteropServices.Guid("be48b9fe-fe7e-41fc-9e4b-b27589382925")]
    public class InteractionService : BaseServiceWithConstructor, IInteractionService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Custom service name assigned to this instance.</param>
        /// <param name="priority">The service priority determines initialization order.</param>
        /// <param name="profile">The service configuration profile.</param>
        public InteractionService(string name, uint priority, InteractionServiceProfile profile)
            : base(name, priority)
        {
            interactors = new List<IInteractor>();
            interactables = new List<IInteractable>();
            NearInteractionEnabled = profile.NearInteraction;
            FarInteractionEnabled = profile.FarInteraction;
        }

        private readonly List<IInteractor> interactors;
        private readonly List<IInteractable> interactables;
        private InteractorRegistrar interactorRegistrar;

        /// <inheritdoc/>
        public bool NearInteractionEnabled { get; set; }

        /// <inheritdoc/>
        public bool FarInteractionEnabled { get; set; }

        /// <inheritdoc/>
        public IReadOnlyList<IInteractor> Interactors => interactors;

        /// <inheritdoc/>
        public IReadOnlyList<IInteractable> Interactables => interactables;

        /// <inheritdoc/>
        public override void Initialize()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!ServiceManager.Instance.TryGetService<IMixedRealityInputSystem>(out _))
            {
                Debug.LogError($"{nameof(InteractionService)} requires the {nameof(IMixedRealityInputSystem)} to work.");
                return;
            }

            interactorRegistrar = new GameObject($"{nameof(InteractionService)}.{nameof(InteractorRegistrar)}").AddComponent<InteractorRegistrar>();
            if (ServiceManager.Instance.ActiveProfile.DoNotDestroyServiceManagerOnLoad)
            {
                interactorRegistrar.gameObject.DontDestroyOnLoad();
            }
        }

        /// <inheritdoc/>
        public override void Destroy()
        {
            if (interactorRegistrar.IsNotNull())
            {
                interactorRegistrar.gameObject.Destroy();
            }

            base.Destroy();
        }

        /// <inheritdoc/>
        public void Add(IInteractor interactor) => interactors.EnsureListItem(interactor);

        /// <inheritdoc/>
        public void Remove(IInteractor interactor) => interactors.SafeRemoveListItem(interactor);

        /// <inheritdoc/>
        public void Add(IInteractable interactable) => interactables.EnsureListItem(interactable);

        /// <inheritdoc/>
        public void Remove(IInteractable interactable) => interactables.SafeRemoveListItem(interactable);

        /// <inheritdoc/>
        public bool TryGetInteractablesByLabel(string label, out IEnumerable<IInteractable> interactables)
        {
            var results = this.interactables.Where(i => !string.IsNullOrWhiteSpace(label) && string.Equals(i.Label, label));
            if (results.Any())
            {
                interactables = results;
                return true;
            }

            interactables = null;
            return false;
        }

        /// <inheritdoc/>
        public bool TryGetInteractor(IMixedRealityInputSource inputSource, out IInteractor interactor)
        {
            interactor = interactors.FirstOrDefault(i => i.InputSource == inputSource);
            return interactor != null;
        }
    }
}
