﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZigBee.Digi.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ZigBee.Digi.Resources.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to USB Serial Port.
        /// </summary>
        public static string AutoDetectionFilterUSBSerialPort {
            get {
                return ResourceManager.GetString("AutoDetectionFilterUSBSerialPort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Coordinator.
        /// </summary>
        public static string CoordinatorCachePrefix {
            get {
                return ResourceManager.GetString("CoordinatorCachePrefix", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Coordinator.json.
        /// </summary>
        public static string CoordinatorFile {
            get {
                return ResourceManager.GetString("CoordinatorFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digi coordinator.
        /// </summary>
        public static string DefaultDigiCoordinatorName {
            get {
                return ResourceManager.GetString("DefaultDigiCoordinatorName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digi network.
        /// </summary>
        public static string DefaultDigiNetworkName {
            get {
                return ResourceManager.GetString("DefaultDigiNetworkName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Digi.
        /// </summary>
        public static string DigiFactoryId {
            get {
                return ResourceManager.GetString("DigiFactoryId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Network.json.
        /// </summary>
        public static string NetworkFile {
            get {
                return ResourceManager.GetString("NetworkFile", resourceCulture);
            }
        }
    }
}
