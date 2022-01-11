// <copyright file="FightModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
	/// <summary>
	/// A model for fights.
	/// </summary>
	public class FightModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FightModel"/> class.
		/// </summary>
		public FightModel()
		{
			this.IsInFight = false;
		}

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets this. I guess this doesn't needs commentary.
		/// </summary>
		public bool IsInFight { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets this.
		/// </summary>
		public bool IsFightEnded { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets this.
		/// </summary>
		public bool IsWin { get; set; }

		/// <summary>
		/// Gets or sets the enemys character model.
		/// </summary>
		public CharacterModel Enemy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the enemy can attack the player.
		/// </summary>
		public bool CanEnemyAttack { get; set; }
	}
}
