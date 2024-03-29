﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NecBlik.Models;

namespace NecBlik
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string SettingsFile = "settings.json";

        private ApplicationSettings applicationSettings = new ApplicationSettings();

        public ApplicationSettings ApplicationSettings
        {
            get { return applicationSettings; }
            set { this.applicationSettings = value; this.OnSettingsChanged(); }
        }

        public App()
        {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;

            if(!File.Exists(SettingsFile))
            {
                var fs = File.Create(SettingsFile);
                if (!File.Exists(SettingsFile))
                    return;
                fs.Close();
                File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(this.ApplicationSettings, Formatting.Indented));
            }
            else
            {
                this.ApplicationSettings = JsonConvert.DeserializeObject<ApplicationSettings>(File.ReadAllText(SettingsFile));
            }

            this.InitializeAppFolder();

            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = System.Globalization.CultureInfo.CreateSpecificCulture(this.applicationSettings.Language);
        }

        private void InitializeAppFolder()
        {
            if(!Directory.Exists(NecBlik.Core.Resources.Resources.MainFolderForLibraries))
                Directory.CreateDirectory(NecBlik.Core.Resources.Resources.MainFolderForLibraries);
            var path = Path.GetFullPath(NecBlik.Core.Resources.Resources.MainFolderForLibraries);
        }

        private void OnSettingsChanged()
        {

            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
            WPFLocalizeExtension.Engine.LocalizeDictionary.Instance.Culture = System.Globalization.CultureInfo.CreateSpecificCulture(this.applicationSettings.Language);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (!File.Exists(SettingsFile))
            {
                File.Create(SettingsFile);
                if (!File.Exists(SettingsFile))
                    return;
                File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(this.ApplicationSettings, Formatting.Indented));
            }
            else
            {
                File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(this.ApplicationSettings, Formatting.Indented));
            }
        }
    }
}
