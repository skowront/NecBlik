using FamFamFam.Flags.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NecBlik.Models;

namespace NecBlik.ViewModels
{
    public class ApplicationSettingsViewModel
    {
        public ApplicationSettings Model { get; set; }

        public string Language
        {
            get
            {
                return this.Model.Language;
            }
            set
            {
                this.Model.Language = value;
            }
        }

        public CountryData CountryLanguage
        {
            get
            {
                var country = CountryData.AllCountries.Where((c) => { return c.Iso2.ToUpper() == this.Model.Language.ToUpper(); }).FirstOrDefault(
                    CountryData.AllCountries.Where((c) => { return c.Iso2.ToUpper() == "GB"; }).First());
                return country;
            }

            set
            {
                this.Model.Language = value.Iso2;
            }
        }

        public ApplicationSettingsViewModel(ApplicationSettings applicationSettings)
        {
            this.Model = applicationSettings;
        }
    }
}
