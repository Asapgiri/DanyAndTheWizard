// <copyright file="LoadGameWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
	using System;
	using System.Windows;
	using System.Windows.Controls;
	using DanyAndTheWizard.Logic.Events;
	using DanyAndTheWizard.Logic.InternalClasses;

	/// <summary>
	/// Interaction logic for LoadGameWindow.xaml.
	/// </summary>
	public partial class LoadGameWindow : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LoadGameWindow"/> class.
		/// </summary>
		public LoadGameWindow()
		{
			this.InitializeComponent();
			this.Loaded += this.LoadGameWindow_Loaded;
		}

		/// <summary>
		/// Calls UserControl switching.
		/// </summary>
		public event EventHandler CallUc;

		private void LoadGameWindow_Loaded(object sender, RoutedEventArgs e)
		{
			this.Stacky.Children.Clear();
			Save save = new Save();

			foreach (string item in save.GetSaveNames())
			{
				Grid grid = new Grid();
				grid.ColumnDefinitions.Add(new ColumnDefinition());
				grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(120) });

				TextBlock tb = new TextBlock();
				tb.Text = item;
				tb.FontSize = 20;
				tb.FontWeight = FontWeights.Bold;
				grid.Children.Add(tb);

				Button bt = new Button();
				bt.Content = "Load";
				bt.Click += (s, ee) =>
				{
					this.CallUc.Invoke(s, new UserSwitchingEventArgs("Game"));
					GameWindow mw = (Window.GetWindow(s as UserControl) as MainWindow).UcDic["Game"] as GameWindow;
					mw.GameControl.LoadFromSave = true;
					mw.GameControl.SaveName = item;
					this.CallUc.Invoke(s, new UserSwitchingEventArgs("Game"));
				};
				Grid.SetColumn(bt, 1);
				grid.Children.Add(bt);

				this.Stacky.Children.Add(grid);
			}
		}

		private void Exit_To_Main_MenuWindow_Click(object sender, RoutedEventArgs e)
		{
			this.CallUc.Invoke(this, new UserSwitchingEventArgs("Menu"));
		}
	}
}
