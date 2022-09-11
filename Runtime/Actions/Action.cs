// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace RealityToolkit.InteractionSDK.Actions
{
    /// <summary>
    /// Base implementation for <see cref="IAction"/>s.
    /// </summary>
    public abstract class Action : MonoBehaviour, IAction
    {
        /// <inheritdoc/>
        public void Activate() { }

        /// <inheritdoc/>
        public void OnReset() { }
    }
}
