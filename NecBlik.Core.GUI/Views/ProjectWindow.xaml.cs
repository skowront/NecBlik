﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NecBlik.Core.GUI.ViewModels;

namespace NecBlik.Core.GUI.Views
{
    /// <summary>
    /// Interaction logic for ProjectWindow.xaml
    /// </summary>
    public partial class ProjectWindow
    {
        public ProjectWindow()
        {
            InitializeComponent();
        }

        public ProjectWindow(ProjectViewModel projectViewModel):this()
        {
            this.DataContext = projectViewModel;
        }
    }
}
