// <copyright file="EquippedModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
	using DanyAndTheWizard.Logic.InternalClasses;

	/// <summary>
	/// A model for equipped items.
	/// </summary>
	public class EquippedModel
	{
		/// <summary>
		/// Gets or sets the equipped armor.
		/// </summary>
		public GameItem Armor { get; set; }

		/// <summary>
		/// Gets or sets the equipped weapon.
		/// </summary>
		public GameItem Weapon { get; set; }

		/// <summary>
		/// Gets or sets the equipped accessory.
		/// </summary>
		public GameItem Accessory { get; set; }
	}
}
