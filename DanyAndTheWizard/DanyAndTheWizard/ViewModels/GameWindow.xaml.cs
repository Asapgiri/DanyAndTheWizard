// <copyright file="GameWindow.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.ViewModels
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

	/// <summary>
	/// Interaction logic for GameWindow.xaml.
	/// </summary>
    public partial class GameWindow : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GameWindow"/> class.
		/// </summary>
		public GameWindow()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Calls UserConstol Switching.
		/// </summary>
		public event EventHandler EscapeEvent;

		/*
		private void Escape(object sender, EventArgs e)
		{
			Window.GetWindow(this).DataContext = this.igm;
		}
		*/

		/// <summary>
		/// Links this escape event with the games to switch EserControls.
		/// </summary>
		public void Eventer()
		{
			this.GameControl.EscapeScreenEvent += this.EscapeEvent;
		}

		/// <summary>
		/// React to a fight.
		/// </summary>
		/// <param name="sender">sender.</param>
		/// <param name="e">args.</param>
		public void FightEventDropped(object sender, EventArgs e)
		{
			if (this.GameControl.Model.Fight.IsInFight)
			{
				this.FightActions.Visibility = Visibility.Visible;
			}
			else
			{
				this.GameControl.Logic.CharacterInteraction();
				this.FightActions.Visibility = Visibility.Hidden;
			}
		}

		/// <summary>
		/// Returns to the game.
		/// </summary>
		public void Return()
		{
			this.GameControl.Resume();
		}

		private void UserControl_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				case Key.Escape:
					if (this.GameControl.IsPaused)
					{
						this.GameControl.Resume();
					}
					else
					{
						this.GameControl.EscapeScreen();
					}

					break;
			}
		}

		private void FireBall(object sender, RoutedEventArgs e)
		{
			this.GameControl.Logic.Fight.Attack("FireBall");
			this.EnemyAttack();
			this.GameControl.EnemyAttackTimer.Start();
		}

		private void RiceBall(object sender, RoutedEventArgs e)
		{
			this.GameControl.Logic.Fight.Attack("RiceBall");
			this.EnemyAttack();
			this.GameControl.EnemyAttackTimer.Start();
		}

		private void Push(object sender, RoutedEventArgs e)
		{
			this.GameControl.Logic.Fight.Attack("Push");
			this.EnemyAttack();
			this.GameControl.EnemyAttackTimer.Start();
		}

		private void Weapon(object sender, RoutedEventArgs e)
		{
			this.GameControl.Logic.Fight.Attack(string.Empty);
			this.EnemyAttack();
			this.GameControl.EnemyAttackTimer.Start();
		}

		private void EnemyAttack()
		{
			this.GameControl.Model.Fight.CanEnemyAttack = true;
			this.FightActions.Visibility = Visibility.Hidden;
		}
	}
}
