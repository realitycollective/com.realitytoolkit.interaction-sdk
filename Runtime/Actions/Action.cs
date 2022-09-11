// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InteractionSDK.Interactables;
using UnityEngine;

namespace RealityToolkit.InteractionSDK.Actions
{
    /// <summary>
    /// Base implementation for <see cref="IAction"/>s.
    /// </summary>
    public abstract class Action : MonoBehaviour, IAction
    {
        protected IInteractable Interactable { get; private set; }

        /// <summary>
        /// Executed when the <see cref="Action"/> is loaded the first time.
        /// </summary>
        protected virtual void Awake()
        {
            Interactable = GetComponent<IInteractable>();
            enabled = false;
        }

        /// <inheritdoc/>
        public virtual void Activate() => enabled = true;

        /// <inheritdoc/>
        public virtual void OnReset() => enabled = false;
    }
}
