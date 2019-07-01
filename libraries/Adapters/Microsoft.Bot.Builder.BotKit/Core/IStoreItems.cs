﻿// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Builder.BotKit.Core
{
    /// <summary>
    /// Interface for defining implementation of the StoreItems.
    /// </summary>
    public interface IStoreItems
    {
        /// <summary>
        /// Gets or sets the Key of the Store Item.
        /// </summary>
        /// <value>The key of the Store item.</value>
        string Key { get; set; }
    }
}
