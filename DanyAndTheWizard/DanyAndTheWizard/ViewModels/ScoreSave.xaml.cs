// <copyright file="ScoreSave.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using DanyAndTheWizard.Logic.Events;

	/// <summary>
	/// Interaction logic for ScoreSave.xaml.
	/// </summary>
    public partial class ScoreSave : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ScoreSave"/> class.
		/// </summary>
		public ScoreSave()
		{
			this.InitializeComponent();
			this.Loaded += this.ScoreSave_Loaded;
		}

		/// <summary>
		/// Event to switch UC.
		/// </summary>
		public event EventHandler UcSwitchEvent;

		private void ScoreSave_Loaded(object sender, RoutedEventArgs e)
		{
			this.ShowText.Content = "Your final score was: " +
				(((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.Model.Highscores.Score *
				(TimeSpan.TicksPerHour / ((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.Model.GameTime.Ticks));
			this.Background = ((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.Model.GetMap.MapBrush;
		}

		private void End_Game_Click(object sender, RoutedEventArgs e)
		{
			((Window.GetWindow(this) as MainWindow).UcDic["Game"] as GameWindow).GameControl.Logic.Save.SaveScore(this.SaveName.Text);
			(Window.GetWindow(this) as MainWindow).UcDic.Remove("Game");
			this.UcSwitchEvent.Invoke(this, new UserSwitchingEventArgs("Menu"));
		}
	}
}
