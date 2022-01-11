// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Media;
	using DanyAndTheWizard.Logic.Events;
	using DanyAndTheWizard.ViewModels;

	/// <summary>
	/// Interaction logic for MainWindow.xaml.
	/// </summary>
	public partial class MainWindow : Window
	{
		private Dictionary<string, UserControl> ucDic;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindow"/> class.
		/// </summary>
		public MainWindow()
		{
			this.InitializeComponent();
			RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.NearestNeighbor);
			this.UcDic = new Dictionary<string, UserControl>();
			this.UcDic.Add("Menu", new MenuWindow());
			this.UcDic.Add("InGameMenu", new InGameMenu());
			this.UcDic.Add("LoadGame", new LoadGameWindow());

			// this.ucDic.Add("Settings", new Settings());
			this.UcDic.Add("Highscores", new HighScoresWindow());
			this.UcDic.Add("ScoreSave", new ScoreSave());

			this.DataContext = this.UcDic["Menu"];

			(this.UcDic["Menu"] as MenuWindow).GameCreationEvent += this.OnNewUcEvent;
			(this.UcDic["Menu"] as MenuWindow).UcSwitchEvent += this.OnUcCallEvent;

			// (this.ucDic["InGameMenu"] as InGameMenu).CallUc += this.OnNewUcEvent;
			(this.UcDic["InGameMenu"] as InGameMenu).CallUc += this.OnUcCallEvent;
			(this.UcDic["Highscores"] as HighScoresWindow).CallUc += this.OnUcCallEvent;
			(this.UcDic["ScoreSave"] as ScoreSave).UcSwitchEvent += this.OnUcCallEvent;

			(this.UcDic["LoadGame"] as LoadGameWindow).CallUc += this.OnUcCallEvent;

			// (this.ucDic["Settings"] as Settings).CallUc += this.OnUcCallEvent;
		}

		/// <summary>
		/// Gets or sets the dictionary for UserControls.
		/// </summary>
		public Dictionary<string, UserControl> UcDic { get => this.ucDic; set => this.ucDic = value; }

		private void OnNewUcEvent(object sender, EventArgs e)
		{
			switch ((e as UserSwitchingEventArgs).NextScreen)
			{
				case "Game":
					if (!this.UcDic.ContainsKey("Game"))
					{
						this.UcDic.Add((e as UserSwitchingEventArgs).NextScreen, new GameWindow());
					}
					else
					{
						this.UcDic["Game"] = new GameWindow();
					}

					(this.UcDic["Game"] as GameWindow).EscapeEvent += this.OnUcCallEvent;
					(this.UcDic["Game"] as GameWindow).Eventer();

					break;
				case "LoadGame":
					if (!this.UcDic.ContainsKey("LoadGame"))
					{
						this.UcDic.Add((e as UserSwitchingEventArgs).NextScreen, new LoadGameWindow());
					}
					else
					{
						this.UcDic["LoadGame"] = new LoadGameWindow();
					}

					break;
				case "Save":
					if (!this.UcDic.ContainsKey("Save"))
					{
						this.UcDic.Add("Save", new SaveWindow());
						(this.UcDic["Save"] as SaveWindow).CallUc += this.OnUcCallEvent;
					}
					else
					{
						this.OnUcCallEvent(this, e);
					}

					break;
				default:
					break;
			}
		}

		private void OnUcCallEvent(object sender, EventArgs e)
		{
			if (this.UcDic.ContainsKey((e as UserSwitchingEventArgs).NextScreen))
			{
				this.DataContext = this.UcDic[(e as UserSwitchingEventArgs).NextScreen];
			}
			else
			{
				this.OnNewUcEvent(this, e);
			}
		}
	}
}
