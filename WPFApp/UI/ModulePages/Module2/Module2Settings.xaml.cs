using Oratoria36.Models.Settings;
using Oratoria36.Service;
using Oratoria36.UI.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Oratoria36.UI.ModulePages.Module2
{
    public partial class Module2Settings : Page, ISettingsPageConfig
    {
        public Module2Settings()
        {
            InitializeComponent();
            //((ISettingsPageConfig)this).ConfigCommonSettings(CommonSettingsGrid, CommonDeviceSettings.CommonSettings);
        }
    }
}
