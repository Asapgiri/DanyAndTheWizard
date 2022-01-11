// <copyright file="MenuWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using DanyAndTheWizard.Logic.Events;

	/// <summary>
	/// Interaction logic for MenuWindow.xaml.
	/// </summary>
	public partial class MenuWindow : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MenuWindow"/> class.
		/// </summary>
		public MenuWindow()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// An event for game creation.
		/// </summary>
		public event EventHandler GameCreationEvent;

		/// <summary>
		/// Calls UserControl switch.
		/// </summary>
		public event EventHandler UcSwitchEvent;

		private void New_Game_Click(object sender, RoutedEventArgs e)
		{
			this.GameCreationEvent.Invoke(this, new UserSwitchingEventArgs("Game"));
			this.UcSwitchEvent.Invoke(this, new UserSwitchingEventArgs("Game"));
		}

		private void Load_Game_Click(object sender, RoutedEventArgs e)
		{
			this.UcSwitchEvent.Invoke(this, new UserSwitchingEventArgs("LoadGame"));
		}

		private void Exit_Click(object sender, RoutedEventArgs e)
		{
			Window.GetWindow(this).Close();
		}

		private void Highscores_Click(object sender, RoutedEventArgs e)
		{
			this.UcSwitchEvent.Invoke(this, new UserSwitchingEventArgs("Highscores"));
		}
	}
}
