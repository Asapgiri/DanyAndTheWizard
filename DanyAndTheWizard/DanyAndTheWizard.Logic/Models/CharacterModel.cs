// <copyright file="CharacterModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.Models
{
	using System.Collections.Generic;
	using System.Windows.Media;
	using DanyAndTheWizard.Logic.InternalClasses;

	/// <summary>
	/// CharacterModel.
	/// </summary>
	public class CharacterModel
	{
		private double hp;
		private double maxHp;

		/// <summary>
		/// Gets or sets a list to select character brushes.
		/// 0 -> standing still.
		/// 1 -> moving 1.
		/// 2 -> moving 2.
		/// </summary>
		public List<ImageBrush> CharacterBrush { get; set; }

		/// <summary>
		/// Gets or sets. This brush is only for fighting screen.
		/// </summary>
		public Brush FightBrush { get; set; }

		/// <summary>
		/// Gets or sets the character name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the characters age.
		/// The older the character, the stronger he gets.
		/// </summary>
		public double Age { get; set; }

		/// <summary>
		/// Gets or sets the character discription.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets a poorly configured character type. Hopes it doesn't have effects. It was implemented early in the project and nearly never used yet.
		/// </summary>
		public string CharacterType { get; set; }

		/// <summary>
		/// Gets or sets gets or sets the characters actual Hp.
		/// </summary>
		public double Hp
		{
			get
			{
				return this.hp;
			}

			set
			{
				if (this.MaxHP == 0)
				{
					this.maxHp = value;
					this.Hp = value;
				}
				else
				{
					this.hp = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the characters maximum HP.
		/// </summary>
		public double MaxHP
		{
			get
			{
				return this.maxHp;
			}

			set
			{
				this.maxHp = value;
				this.hp = value;
			}
		}

		/// <summary>
		/// Gets or sets the characters inventory.
		/// </summary>
		public List<GameItem> Inventory { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of items in the inventory.
		/// </summary>
		public int InventorySize { get; set; }

		/// <summary>
		/// Gets or sets the actual equipped items.
		/// </summary>
		public EquippedModel Equipped { get; set; }
	}
}
