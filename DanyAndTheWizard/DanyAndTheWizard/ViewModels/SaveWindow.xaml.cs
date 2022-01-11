// <copyright file="SaveWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using DanyAndTheWizard.Logic.Events;

	/// <summary>
	/// Interaction logic for SaveWindow.xaml.
	/// </summary>
	public partial class SaveWindow : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SaveWindow"/> class.
		/// </summary>
		public SaveWindow()
		{
			this.InitializeComponent();
			this.Loaded += this.SaveWindow_Loaded;
		}

		/// <summary>
		/// Calls UserControl switch.
		/// </summary>
		public event EventHandler CallUc;

		private void SaveWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.Background = ((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.Model.GetMap.MapBrush;
		}

		private void Save_Click(object sender, RoutedEventArgs e)
		{
			((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.SaveGame(this.SaveName.Text);
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("Game"));
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("InGameMenu"));
		}
	}
}
