using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rtpmidi_session_repeater
{
    public record Config
    {
        public ushort Port { get; set; } = 5004;
        public required string SessionName { get; set; }
        public required IEnumerable<Peer> Peers { get; set; }
    }

    public record Peer
    {
        public required string Host { get; set; }
        public ushort Port { get; set; } = 5004;
    }
}