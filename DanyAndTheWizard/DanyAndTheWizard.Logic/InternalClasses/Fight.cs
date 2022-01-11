// <copyright file="Fight.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.InternalClasses
{
	using System;
	using DanyAndTheWizard.Logic.Interfaces;

	/// <summary>
	/// The class to have fights.
	/// </summary>
	public class Fight : IFight
	{
		private DatwModel model;
		private Random rnd = new Random();

		/// <summary>
		/// Initializes a new instance of the <see cref="Fight"/> class.
		/// </summary>
		/// <param name="model">Game model.</param>
		public Fight(DatwModel model)
		{
			this.model = model;
		}

		/// <inheritdoc/>
		public event EventHandler FightEvent;

		/// <summary>
		/// Start a fight with an enemy.
		/// </summary>
		/// <param name="enemyName">the name of the enemy.</param>
		public void StartFight()
		{
			this.model.Fight.IsInFight = true;
			this.model.Fight.IsFightEnded = false;
			this.model.Fight.Enemy.Hp = this.model.Fight.Enemy.MaxHP;
			this.FightEvent.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// To attack the enemy Character.
		/// </summary>
		/// <param name="attackName">The name of the enemy attack.</param>
		public void Attack(string attackName)
		{
			double value = 0, critchance = 0.1;
			bool critical = false;
			switch (attackName)
			{
				case "FireBall":
					value = 20;
					break;
				case "RiceBall":
					value = 30;
					break;
				case "Push":
					value = 10;
					break;
				default:
					if (this.model.GetPlayer.Equipped.Weapon != null)
					{
						value = this.model.GetPlayer.Equipped.Weapon.Damage;
						critchance = this.model.GetPlayer.Equipped.Weapon.CriticalChance;
					}

					break;
			}

			critical = (critchance * 100) > this.rnd.Next(0, 101) ? true : false;
			value = value * (this.model.GetPlayer.MaxHP / 200) *
							(critical ? 2 : 1) *
							(value / this.model.Fight.Enemy.Equipped.Armor.Defense);

			if (value > 0)
			{
				this.model.Fight.Enemy.Hp -= value;
			}

			this.model.IsInteractionRunning = true;
			this.model.Dialog.ActualDialog = $"[System]\nYou used {attackName}.\nIt caused: {value} Damage.";
			if (critical)
			{
				this.model.Dialog.ActualDialog += "\n*CRITICAL*";
			}

			if (this.model.Fight.Enemy.Hp <= 0)
			{
				this.Win();
			}
		}

		/// <summary>
		/// The attack of the enemy.
		/// </summary>
		/// <param name="attackName">Name of the attack.</param>
		public void EnemyAttack(string attackName)
		{
			double value = 0, critchance = 0.1;
			bool critical = false;
			switch (attackName)
			{
				case "FireBall":
					value = 20;
					break;
				case "RiceBall":
					value = 30;
					break;
				case "Push":
					value = 10;
					break;
				default:
					if (this.model.Fight.Enemy.Equipped.Weapon != null)
					{
						value = this.model.Fight.Enemy.Equipped.Weapon.Damage;
						critchance = this.model.Fight.Enemy.Equipped.Weapon.CriticalChance;
					}

					break;
			}

			critical = (critchance * 100) > this.rnd.Next(0, 101) ? true : false;
			value = value * (this.model.Fight.Enemy.Age / 50) *
							(critical ? 2 : 1) *
							(value / this.model.GetPlayer.Equipped.Armor.Defense);

			if (value > 0)
			{
				this.model.GetPlayer.Hp -= value;
			}
			else if (value < 0)
			{
				value = 0;
			}

			this.model.IsInteractionRunning = true;
			this.model.Dialog.ActualDialog = $"[System]\n{this.model.Fight.Enemy.Name} used {attackName}.\nIt caused: {value} Damage.";
			if (critical)
			{
				this.model.Dialog.ActualDialog += "\n*CRITICAL*";
			}

			if (this.model.GetPlayer.Hp <= 0)
			{
				this.Lose();
			}
		}

		/// <summary>
		/// After losing a game.
		/// </summary>
		public void Lose()
		{
			this.model.Fight.IsWin = false;
			this.model.Dialog.ActualScene = "Defeat";

			// this.EndFight();
			this.model.Fight.IsFightEnded = true;
		}

		/// <summary>
		/// Winning a game.
		/// </summary>
		public void Win()
		{
			this.model.Fight.IsWin = true;
			this.model.Highscores.EnemiesKilled++;
			this.model.Highscores.Score += (this.model.Fight.Enemy.CharacterType == "Boss" ? 10 : 1) *
				(this.model.MapName == "City" ? 1 : this.model.MapName == "Forest" ? 2 : this.model.MapName == "Field" ? 3 : 4);
			this.model.Dialog.ActualScene = "Victory";

			// this.EndFight();
			this.model.Fight.IsFightEnded = true;
		}

		/// <summary>
		/// Ending a fight.
		/// </summary>
		public void EndFight()
		{
			this.model.Fight.IsInFight = false;
			this.model.GetPlayer.Hp = this.model.Fight.IsWin ? this.model.GetPlayer.MaxHP : this.model.GetPlayer.MaxHP / 10;
			this.model.Dialog.ActualSpeeker = this.model.Fight.Enemy.Name;
			if (this.model.Dialog.ActualScene == "Speach")
			{
				this.model.Dialog.ActualScene = string.Empty;
			}

			this.FightEvent.Invoke(this, new EventArgs());
		}
	}
}
