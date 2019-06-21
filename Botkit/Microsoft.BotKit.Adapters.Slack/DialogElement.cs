// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.BotKit.Adapters.Slack
{
    /// <summary>
    /// Elements related to the Dialog.
    /// </summary>
    public class DialogElement
    {
        /// <summary>
        /// Gets or Sets the label for the dialog element.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets the name for the dialog element.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the value for the dialog element.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets the pair of values for an option List in the dialog element.
        /// </summary>
        public Dictionary<string, string> OptionList { get; set; }

        /// <summary>
        /// Gets or Sets options for the dialog element.
        /// </summary>
        public ISlackAdapterOptions Options { get; set; }

        /// <summary>
        /// Gets or Sets the type for the dialog element.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or Sets the subtype for the dialog element.
        /// </summary>
        public string Subtype { get; set; }
    }
}
