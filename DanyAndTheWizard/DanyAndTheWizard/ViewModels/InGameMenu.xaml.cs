// <copyright file="InGameMenu.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using DanyAndTheWizard.Logic.Events;

	/// <summary>
	/// Interaction logic for InGameMenuWindow.xaml.
	/// </summary>
    public partial class InGameMenu : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InGameMenu"/> class.
		/// </summary>
		public InGameMenu()
		{
			this.InitializeComponent();
			this.Loaded += this.InGameMenu_Loaded;
		}

		/// <summary>
		/// Calls UserControl switching.
		/// </summary>
		public event EventHandler CallUc;

		/// <summary>
		/// Calls Exit from GameWindow.
		/// </summary>
		public event EventHandler Exit;

		private void InGameMenu_Loaded(object sender, RoutedEventArgs e)
		{
			this.Background = ((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.Model.GetMap.MapBrush;
		}

		/*public InGameMenuWindow(UserControl control)
		{
			this.control = control;
			this.InitializeComponent();
		}*/

		private void Save_Game_Click(object sender, RoutedEventArgs e)
		{
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("Save"));
		}

		private void Load_Game_Click(object sender, RoutedEventArgs e)
		{
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("LoadGame"));
		}

		private void Exit_To_Main_MenuWindow_Click(object sender, RoutedEventArgs e)
		{
			this.Exit.Invoke(this, EventArgs.Empty);
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("Menu"));
			GC.Collect();

			// System.Windows.Forms.Application.Restart();
			// Application.Current.Shutdown();
		}

		private void Return_To_Game_Click(object sender, RoutedEventArgs e)
		{
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("Game"));
		}
	}
}
