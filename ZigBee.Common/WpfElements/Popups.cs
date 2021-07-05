using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ZigBee.Common.WpfElements
{
    /// <summary>
    /// Provides an organised form of icons for ZigBeeGui.
    /// </summary>
    public class Popups
    {
        /// <summary>
        /// Contains some default codes for ZigBee application icons.
        /// </summary>
        public class IconCodes
        {
            public const string Info = "\uf05a";
            public const string Warning = "\uf071";
            public const string Error = "\uf1e2";
        }

        /// <summary>
        /// Contains all necessary information to display an icon in a wpf box.
        /// </summary>
        public class ZigBeeIcon
        {
            public string IconCode { get; } = "\uf05a";
            public Brush IconColor { get; } = Brushes.Black;
            public ZigBeeIcon(string code,Brush color)
            {
                this.IconCode = code;
                this.IconColor = color;
            }
        }

        /// <summary>
        /// Contains instantiated default ZigBee icons.
        /// </summary>
        public class ZigBeeIcons
        {
            public static readonly ZigBeeIcon InfoIcon = new ZigBeeIcon(IconCodes.Info,Brushes.Blue);
            public static readonly ZigBeeIcon WarningIcon = new ZigBeeIcon(IconCodes.Warning, Brushes.Yellow);
            public static readonly ZigBeeIcon ErrorIcon = new ZigBeeIcon(IconCodes.Error, Brushes.Red);
        }
    }
}
