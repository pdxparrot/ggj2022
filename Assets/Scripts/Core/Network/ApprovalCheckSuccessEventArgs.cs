#pragma warning disable 0618    // disable obsolete warning for now

using System;

namespace pdxpartyparrot.Core.Network
{
    public sealed class ApprovalCheckSuccessEventArgs : EventArgs
    {
        public ulong ClientId { get; }

        public ApprovalCheckSuccessEventArgs(ulong clientId)
        {
            ClientId = clientId;
        }
    }
}
