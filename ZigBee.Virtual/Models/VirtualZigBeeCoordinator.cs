using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigBee.Core.Interfaces;
using ZigBee.Core.Models;

namespace ZigBee.Virtual.Models
{
    public class VirtualZigBeeCoordinator:ZigBeeCoordinator
    {
        private List<IZigBeeSource> zigBeeSources = new List<IZigBeeSource>();

        private List<Tuple<string, string>> connections = new List<Tuple<string, string>>();

        private IZigBeeFactory zigBeeFactory;

        public VirtualZigBeeCoordinator(IZigBeeFactory zigBeeFactory)
        {
            this.zigBeeFactory = zigBeeFactory;
        }

        public VirtualZigBeeCoordinator(IZigBeeFactory zigBeeFactory, bool setupExampleZigBees)
        {
            this.zigBeeFactory = zigBeeFactory;
            if (setupExampleZigBees)
                this.setupExampleZigBees();
        }

        private void setupExampleZigBees()
        {
            this.zigBeeSources = new List<IZigBeeSource>();
            this.zigBeeSources.Add(this.zigBeeFactory.Build());
            this.zigBeeSources.Add(this.zigBeeFactory.Build());
            this.zigBeeSources.Add(this.zigBeeFactory.Build());
            this.zigBeeSources.Add(this.zigBeeFactory.Build());
            this.zigBeeSources.Add(this.zigBeeFactory.Build());
            this.zigBeeSources.Add(this.zigBeeFactory.Build());

            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[0].GetAddress(),this.zigBeeSources[1].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[1].GetAddress(),this.zigBeeSources[2].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[1].GetAddress(),this.zigBeeSources[3].GetAddress()));
            this.connections.Add(new Tuple<string, string>(this.zigBeeSources[4].GetAddress(),this.zigBeeSources[5].GetAddress()));
        }

        public override IEnumerable<IZigBeeSource> GetDevices()
        {
            return this.zigBeeSources;
        }

        public override IEnumerable<Tuple<string, string>> GetConnections()
        {
            return this.connections;
        }
    }
}
