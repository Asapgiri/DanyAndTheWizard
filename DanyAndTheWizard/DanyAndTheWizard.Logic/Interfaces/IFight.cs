// <copyright file="IFight.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Interfaces
{
	using System;

	/// <summary>
	/// The interface for a fight.
	/// </summary>
	public interface IFight
	{
		/// <summary>
		/// An event to know if something happened in the fight.
		/// </summary>
		event EventHandler FightEvent;

		/// <summary>
		/// Starts a fight.
		/// </summary>
		void StartFight();

		/// <summary>
		/// Attacks the enemy.
		/// </summary>
		/// <param name="attackName">the attaks name. "" = equipped weapon.</param>
		void Attack(string attackName);

		/// <summary>
		/// The enemy attacks the character.
		/// </summary>
		/// <param name="attackName">The enemys attack's name.</param>
		void EnemyAttack(string attackName);

		/// <summary>
		/// Sets the status to Victory.
		/// </summary>
		void Win();

		/// <summary>
		/// Setr the results to Defeate.
		/// </summary>
		void Lose();

		/// <summary>
		/// Ends the fiht.
		/// </summary>
		void EndFight();
	}
}
