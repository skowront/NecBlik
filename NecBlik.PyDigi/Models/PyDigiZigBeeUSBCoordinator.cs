using Newtonsoft.Json;
using Python.Runtime;
using NecBlik.Common.WpfExtensions.Interfaces;
using NecBlik.Core.Interfaces;
using NecBlik.PyDigi.Factories;
using NecBlik.Virtual.Models;
using NecBlik.Core.Enums;

namespace NecBlik.PyDigi.Models
{
    public class PyDigiZigBeeUSBCoordinator : VirtualCoordinator
    {
        private PyModule scope;

        private dynamic pyCoordinator;

        private const int sleepTime = 500;

        private const long timeout = 25000L;

        private const int maxProgress = (int)timeout / sleepTime;

        private IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null;

        [JsonProperty]
        public PyDigiUSBConnectionData connectionData { get; set; }
        public delegate void Callback(object arg);

        public PyDigiZigBeeUSBCoordinator(IDeviceFactory zigBeeFactory, PyDigiUSBConnectionData connectionData = null) : base(zigBeeFactory)
        {
            this.connectionData = connectionData ?? new PyDigiUSBConnectionData() { port = "COM4", baud = 9600 };
            this.deviceFactory = new PyDigiZigBeeFactory();
            this.internalType = this.deviceFactory.GetVendorID();
            this.connectionData = connectionData ?? new() { port = string.Empty, baud = 9600 };
            this.Name = Resources.Resources.PyDefaultDigiCoordinatorName;
            try
            {
                using (Py.GIL())
                {
                    this.scope = ZigBeePyEnv.NewInitializedScope();
                    this.scope.Exec(File.ReadAllText(Resources.Resources.PyDigiScriptsLocation + "/" + Resources.Resources.ScriptZigBeeCoordinator_py));
                    this.scope.Exec($"coordinator = Coordinator(\"{this.connectionData.port}\",\"{this.connectionData.baud}\")");
                    this.pyCoordinator = this.scope.Get<dynamic>("coordinator");
                    this.pyCoordinator.Open();
                    this.scope.Set("action", new Action<string,string>((data,address) => {
                        Console.WriteLine("Works!");
                        this.OnDataRecieved(data, address);
                    }));
                    this.scope.Exec("dataReceivedActionHolder = ActionHolder(action)");
                    this.scope.Exec("def my_data_received_callback(xbee_message):\n" +
                                    "\t a = str(xbee_message.remote_device.get_64bit_addr())\n" +
                                    "\t dataReceivedActionHolder.callback.Invoke(bytes(xbee_message.data).decode(encoding=\"UTF-8\"),a); \n");
                    this.scope.Exec("coordinator.xbee.add_data_received_callback(my_data_received_callback)");

                    Console.WriteLine("Initialization fine.");
                }
            }
            catch (Exception ex)
            {

            }
        }

        ~PyDigiZigBeeUSBCoordinator()
        {
            this.pyCoordinator.Close();
        }

        private void ZigBeeDataRecieved(object? sender, object arg)
        {
            using (Py.GIL())
            {
                var scope = ZigBeePyEnv.NewInitializedScope();
                scope.Set("data", (arg as dynamic).data);
                scope.Set("data_str", string.Empty);
                scope.Exec("data_str = bytes(data)");
                var dataString = scope.Get<string>("data_str");
                string senderAddress = (arg as dynamic).remote_node.get_64bit_addr().ToString();
                this.OnDataRecieved(dataString, senderAddress);
                scope.Dispose();
            }
        }

        public override async Task<IEnumerable<IDeviceSource>> GetDevices(IUpdatableResponseProvider<int, bool, string> progressResponseProvider = null)
        {
            this.progressResponseProvider = progressResponseProvider;
            using (Py.GIL())
            {
                pyCoordinator.Open();

                var func = new Action(() => {
                    Console.WriteLine("");
                });
                await this.Discover();

                dynamic devices = this.pyCoordinator.devices;
                List<IDeviceSource> sources = new();
                foreach(var item in devices)
                {
                    PyDigiZigBeeSource source = new PyDigiZigBeeSource(item);
                    sources.Add(source);
                }
                return sources;
            }
        }

        public override async Task Discover()
        {
            progressResponseProvider?.Init(0, PyDigiZigBeeUSBCoordinator.maxProgress, 0);
            var progress = 0;
            using (Py.GIL())
            {
                
                this.pyCoordinator.AddDiscoveryUpdateCallback(new Action<int>((input) =>
                {
                    progress = input;
                    this.progressResponseProvider?.Update(input);
                }));
                var task = Task.Run(() =>
                {
                    scope.Exec("coordinator.DiscoverDevices()");
                });
                await task;
                
            }
            this.progressResponseProvider?.Update(PyDigiZigBeeUSBCoordinator.maxProgress);
            progressResponseProvider?.SealUpdates();
            return;
        }
        public override void Save(string folderPath)
        {
            File.WriteAllText(folderPath + "\\" + Resources.Resources.CoordinatorFile, JsonConvert.SerializeObject(this.connectionData, Formatting.Indented));
        }

        public override string GetVersion()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic version = this.pyCoordinator.get_hardware_version().description;
                return version.ToString();
            }
        }

        public override string GetAddress()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic address = this.pyCoordinator.GetAddress();
                return address.ToString();
            }
        }
        public override string GetCacheId()
        {
            return this.GetAddress();
        }

        public override string GetPanID()
        {
            using (Py.GIL())
            {
                pyCoordinator.Open();
                dynamic panId = this.pyCoordinator.GetPanId();
                return panId;
            }
        }

        public override void Send(string data, string address)
        {
            using(Py.GIL())
            {
                this.pyCoordinator.Send(data,address);
            }
            
        }

        public override void Close()
        {
            using (Py.GIL())
            {
                this.pyCoordinator.Close();
            }
        }

        public override bool Open()
        {
            using (Py.GIL())
            {
                this.pyCoordinator.Open();
            }
            return true;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.Close();
        }
        public override async Task<string> GetStatusOf(string remoteAddress)
        {
            if (remoteAddress == this.Address)
            {
                return this.Open() ? NecBlik.Core.Resources.Statuses.Connected
                    : NecBlik.Core.Resources.Statuses.Disconnected;
            }
            return NecBlik.Core.Resources.Statuses.Unknown;
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class PyDigiUSBConnectionData
        {
            [JsonProperty]
            public int baud;

            [JsonProperty]
            public string port = string.Empty;
        }
    }
}
