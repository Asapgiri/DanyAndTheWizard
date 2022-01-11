// <copyright file="GameItem.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.InternalClasses
{
	using System.Windows.Media.Imaging;

	/// <summary>
	/// A model for a game item.
	/// </summary>
	public class GameItem
	{
		/// <summary>
		/// Gets or sets the Items Image.
		/// </summary>
		public BitmapImage Image { get; set; }

		/// <summary>
		/// Gets or sets the items Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the items name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the items Descreption.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the items damage.
		/// </summary>
		public double Damage { get; set; }

		/// <summary>
		/// Gets or sets the items defence power.
		/// </summary>
		public double Defense { get; set; }

		/// <summary>
		/// Gets or sets the items critical chance.
		/// </summary>
		public double CriticalChance { get; set; }

		/// <summary>
		/// Gets or sets a neutral items healing capablelity.
		/// </summary>
		public double Health { get; set; }

		/// <summary>
		/// Gets or sets the items rarity.
		/// </summary>
		public string Rarity { get; set; }

		/// <summary>
		/// Gets or sets the items type.
		/// </summary>
		public ItemType Type { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the item is equipped.
		/// </summary>
		public bool IsEquipped { get; set; }

		/// <summary>
		/// Generates a text to write out the items stats.
		/// </summary>
		/// <returns>A text with the items stats.</returns>
		public override string ToString()
		{
			if (this.Type != ItemType.Neutral)
			{
				return $"[{this.Name.Replace('_', ' ')}]\nDescription: {this.Description}\nDefense: {this.Defense}\tDamage: {this.Damage}\nRarity: {this.Rarity}\tCritChanse: {this.CriticalChance * 100}%";
			}
			else
			{
				return $"[{this.Name.Replace('_', ' ')}]\nDescription: {this.Description}\nHealth Restore: +{this.Health} Hp\nDamage: {this.Damage}\tRarity: {this.Rarity}\tCritChanse: {this.CriticalChance * 100}%";
			}
		}
	}
}
