﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NecBlik.Core.Resources {
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
    public class Statuses {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Statuses() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NecBlik.Core.Resources.Statuses", typeof(Statuses).Assembly);
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
        ///   Looks up a localized string similar to Connected.
        /// </summary>
        public static string Connected {
            get {
                return ResourceManager.GetString("Connected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Disconnected.
        /// </summary>
        public static string Disconnected {
            get {
                return ResourceManager.GetString("Disconnected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Idle.
        /// </summary>
        public static string Idle {
            get {
                return ResourceManager.GetString("Idle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Other.
        /// </summary>
        public static string Other {
            get {
                return ResourceManager.GetString("Other", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Running.
        /// </summary>
        public static string Running {
            get {
                return ResourceManager.GetString("Running", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unknown.
        /// </summary>
        public static string Unknown {
            get {
                return ResourceManager.GetString("Unknown", resourceCulture);
            }
        }
    }
}
