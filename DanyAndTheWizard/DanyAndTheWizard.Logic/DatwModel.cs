// <copyright file="DatwModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic
{
	using System;
	using System.Windows;
	using System.Windows.Media;
	using DanyAndTheWizard.Logic.Models;

	/// <summary>
	/// The model of the game DATW.
	/// </summary>
	public class DatwModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DatwModel"/> class.
		/// </summary>
		/// <param name="w">Width.</param>
		/// <param name="h">Height.</param>
		public DatwModel(double w, double h)
		{
			this.GameWidth = w;
			this.GameHeight = h;
			this.TileSize = w / 16;
			this.Player = new Point(8, 4.5);
			this.ActualMapSegment = new Point(0, 0);
			this.BaseTime = default(TimeSpan);
			this.Fight = new FightModel()
			{
				IsInFight = false,
			};

			// Entities = new.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DatwModel"/> class.
		/// </summary>
		/// <param name="w">Width.</param>
		/// <param name="h">Height.</param>
		/// <param name="player">Player location. In block locations.</param>
		public DatwModel(double w, double h, Point player)
		{
			this.GameWidth = w;
			this.GameHeight = h;
			this.TileSize = w / 16;
			this.Player = player;

			// Entities = new.
		}

		/// <summary>
		/// Gets or sets the actually loaded maps name.
		/// </summary>
		public string MapName { get; set; }

		/// <summary>
		/// Gets or sets the current map and elements.
		/// </summary>
		public MapModel[,] Map { get; set; }

		/// <summary>
		/// Gets or sets the players location on the map.
		/// </summary>
		public Point Player { get; set; }

		/// <summary>
		/// Gets the width of the game.
		/// </summary>
		public double GameWidth { get; private set; } // Pixell size

		/// <summary>
		/// Gets the height of the ganme.
		/// </summary>
		public double GameHeight { get; private set; } // Pixell size

		/// <summary>
		/// Gets or sets the size of one blocks corner on the screen in pixel.
		/// </summary>
		public double TileSize { get; set; }

		/// <summary>
		/// Gets or sets the actual segment of the map.
		/// </summary>
		public Point ActualMapSegment { get; set; }

		/// <summary>
		/// Gets the actual mapModel.
		/// </summary>
		/// <returns>MapModel.</returns>
		public MapModel GetMap { get => this.Map[(int)this.ActualMapSegment.X, (int)this.ActualMapSegment.Y]; }

		/// <summary>
		/// Gets or sets the games minimap values.
		/// </summary>
		public MinimapModel Minimap { get; set; }

		/// <summary>
		/// Gets or sets the player's values.
		/// </summary>
		public CharacterModel GetPlayer { get; set; }

		/// <summary>
		/// Gets or sets the current olayer skin brush.
		/// </summary>
		public Brush CurrentPlayerBrush { get; set; }

		/// <summary>
		/// Gets or sets the current skins value.
		/// </summary>
		public int CharacterTimer { get; set; }

		/// <summary>
		/// Gets or sets the gametime.
		/// </summary>
		public TimeSpan GameTime { get; set; }

		/// <summary>
		/// Gets or sets time from a save.
		/// </summary>
		public TimeSpan BaseTime { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether an interaction is running.
		/// </summary>
		public bool IsInteractionRunning { get; set; }

		/// <summary>
		/// Gets or sets the actual dialog parameters.
		/// </summary>
		public DialogModel Dialog { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether gets or sets the inventorys state.
		/// </summary>
		public bool IsInventoryOpen { get; set; }

		/// <summary>
		/// Gets or sets the fights values.
		/// </summary>
		public FightModel Fight { get; set; }

		/// <summary>
		/// Gets or sets the highscores.
		/// </summary>
		public ScoreModel Highscores { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the player can have the easter egg.
		/// </summary>
		public bool EasterEggUnlocked { get; set; }
	}
}
