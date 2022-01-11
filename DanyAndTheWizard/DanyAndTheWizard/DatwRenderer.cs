// <copyright file="DatwRenderer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard
{
	using System;
	using System.Globalization;
	using System.Windows;
	using System.Windows.Media;
	using DanyAndTheWizard.Logic;

	/// <summary>
	/// This class renders out the screen for the game.
	/// </summary>
	internal class DatwRenderer
	{
		private DatwModel model;
		private Drawing oldBackground;
		private Drawing oldEntities;
		private DrawingGroup oldEntitieNames;
		private Drawing oldPlayer;
		private Drawing oldMinimap;
		private Point oldMapPoint;

		private int fpsCount = 0;
		private int fps;
		private TimeSpan prevTime = default(TimeSpan);

		private Point oldPlayerPosition;

		/// <summary>
		/// Initializes a new instance of the <see cref="DatwRenderer"/> class.
		/// </summary>
		/// <param name="model">GameModel.</param>
		public DatwRenderer(DatwModel model)
		{
			this.model = model;
		}

		/// <summary>
		/// Gets player brush.
		/// </summary>
		private Brush PlayerBrush { get => this.model.CurrentPlayerBrush; }

		/// <summary>
		/// Gets the players fightbrush.
		/// </summary>
		private Brush PlayerFightBrush { get => this.model.GetPlayer.FightBrush; }

		/// <summary>
		/// Gets the backgrounds brush from teh actual mapsegment.
		/// </summary>
		private Brush BackgroundBrush { get => this.model.GetMap.MapBrush; }

		/// <summary>
		/// Gets the enemys brush.
		/// </summary>
		private Brush EnemyBrush { get => this.model.Fight.Enemy.FightBrush; }

		/// <summary>
		/// Resets the renderer.
		/// </summary>
		public void Reset()
		{
			this.oldBackground = null;
			this.oldEntities = null;
			this.oldEntitieNames = null;
			this.oldPlayer = null;
			this.oldMinimap = null;
			this.oldPlayerPosition = new Point(-1, -1);
		}

		/// <summary>
		/// Resets from an event.
		/// </summary>
		/// <param name="o">sender.</param>
		/// <param name="args">args.</param>
		public void Reset(object o, EventArgs args)
		{
			this.Reset();
		}

		/// <summary>
		/// Draws the game.
		/// </summary>
		/// <returns>The games drawing.</returns>
		public Drawing GetDrawing()
		{
			DrawingGroup dg = new DrawingGroup();
			dg.Children.Add(this.GetBacground());
			dg.Children.Add(this.GetEntities());
			dg.Children.Add(this.GetPlayer());
			dg.Children.Add(this.oldEntitieNames);
			dg.Children.Add(this.GetGUI());
			if (this.model.Fight.IsInFight)
			{
				dg.Children.Add(this.GetFight());
			}

			if (this.model.IsInteractionRunning)
			{
				dg.Children.Add(this.GetInteraction());
			}

			// dg.Children.Add(this.GetFight());
			if (this.oldMapPoint != this.model.ActualMapSegment)
			{
				this.oldMapPoint = this.model.ActualMapSegment;
			}

			if (this.oldPlayerPosition != this.model.Player)
			{
				this.oldPlayerPosition = this.model.Player;
			}

			// Count fps...
			if (this.model.GameTime.TotalSeconds > this.prevTime.TotalSeconds + 1)
			{
				this.prevTime = this.model.GameTime;
				this.fps = this.fpsCount;
				this.fpsCount = 0;
			}
			else
			{
				this.fpsCount++;
			}

			return dg;
		}

		private Drawing GetFight()
		{
			DrawingGroup dg = new DrawingGroup();

			// Draws the fight.
			dg.Children.Add(new GeometryDrawing(new SolidColorBrush(Colors.LightGray) { Opacity = 0.8 }, null, new RectangleGeometry(new Rect(
				this.model.TileSize / 2,
				this.model.TileSize / 2,
				Config.Width - this.model.TileSize,
				Config.Height - this.model.TileSize))));
			dg.Children.Add(new GeometryDrawing(this.PlayerFightBrush, null, new RectangleGeometry(new Rect(
				this.model.TileSize / 2,
				Config.Height - (this.model.TileSize / 2) - (this.model.TileSize * Config.FightCharacterSize),
				this.model.TileSize * Config.FightCharacterSize,
				this.model.TileSize * Config.FightCharacterSize))));
			dg.Children.Add(new GeometryDrawing(this.EnemyBrush, null, new RectangleGeometry(new Rect(
				Config.Width - (this.model.TileSize / 2) - (this.model.TileSize * Config.FightCharacterSize),
				this.model.TileSize / Config.FightCharacterSize,
				this.model.TileSize * Config.FightCharacterSize,
				this.model.TileSize * Config.FightCharacterSize))));

			// Draws the fight stats.
			Rect playerHpBorderRect = new Rect(
				(Config.Width / 2) - (this.model.TileSize / 2) - (this.model.TileSize * Config.FightCharacterSize * 0.7),
				(Config.Height / 2) - (this.model.TileSize / 2),
				this.model.TileSize * Config.FightCharacterSize * 0.7,
				this.model.TileSize * 0.7);
			Rect enemyHpBorderRect = new Rect(
				(Config.Width / 2) + (this.model.TileSize / 2),
				this.model.TileSize,
				this.model.TileSize * Config.FightCharacterSize * 0.7,
				this.model.TileSize * 0.7);
			Rect playerHpRect = new Rect(
				(Config.Width / 2) - (this.model.TileSize / 2) - (this.model.TileSize * Config.FightCharacterSize * 0.7),
				(Config.Height / 2) - (this.model.TileSize / 2),
				(this.model.TileSize * Config.FightCharacterSize * 0.7) * ((this.model.GetPlayer.Hp > 0 ? this.model.GetPlayer.Hp : 0) / this.model.GetPlayer.MaxHP),
				this.model.TileSize * 0.7);
			Rect enemyHpRect = new Rect(
				(Config.Width / 2) + (this.model.TileSize / 2),
				this.model.TileSize,
				(this.model.TileSize * Config.FightCharacterSize * 0.7) * ((this.model.Fight.Enemy.Hp > 0 ? this.model.Fight.Enemy.Hp : 0) / this.model.Fight.Enemy.MaxHP),
				this.model.TileSize * 0.7);

			dg.Children.Add(new GeometryDrawing(Brushes.Red, null, new RectangleGeometry(playerHpRect)));
			dg.Children.Add(new GeometryDrawing(Brushes.Red, null, new RectangleGeometry(enemyHpRect)));
			dg.Children.Add(new GeometryDrawing(this.model.Minimap.MinimapBorderBrush, null, new RectangleGeometry(playerHpBorderRect)));
			dg.Children.Add(new GeometryDrawing(this.model.Minimap.MinimapBorderBrush, null, new RectangleGeometry(enemyHpBorderRect)));

			// Hp texts:
			dg.Children.Add(this.DrawMyText(
				this.model.GetPlayer.Hp.ToString("F0"),
				new Point(
				(Config.Width / 2) - (this.model.TileSize / 2) - (this.model.TileSize * Config.FightCharacterSize * 0.7),
				(Config.Height / 2) - (this.model.TileSize / 2)),
				FlowDirection.LeftToRight,
				Config.TimeSize,
				Brushes.Transparent));
			dg.Children.Add(this.DrawMyText(
				this.model.Fight.Enemy.Hp.ToString("F0"),
				new Point(
				(Config.Width / 2) + (this.model.TileSize / 2),
				this.model.TileSize),
				FlowDirection.LeftToRight,
				Config.TimeSize,
				Brushes.Transparent));

			return dg;
		}

		private Drawing GetInteraction()
		{
			return this.DrawMyText(
				this.model.Dialog.ActualDialog,
				new Point(Config.Width - 600, Config.Height - 300),
				FlowDirection.LeftToRight,
				32,
				Brushes.DarkSeaGreen,
				true);
		}

		private Drawing GetGUI()
		{
			DrawingGroup drawingGroup = new DrawingGroup();
			if (this.model.Minimap.Visible)
			{
				drawingGroup.Children.Add(this.GetMinimap());
			}

			// drawingGroup.Children.Add(this.GetHpBar());
			drawingGroup.Children.Add(this.Stats());

			// drawingGroup.Children.Add(this.Tooltips());
			return drawingGroup;
		}

		private Drawing Tooltips()
		{
			throw new NotImplementedException();
		}

		private Drawing GetMinimap()
		{
			if (this.oldMinimap == null || this.oldPlayerPosition != this.model.Player)
			{
				DrawingGroup dgp = new DrawingGroup();
				Geometry g = new RectangleGeometry(new Rect(0, this.model.GameHeight - this.model.Minimap.Height, this.model.Minimap.Width, this.model.Minimap.Height));
				dgp.Children.Add(new GeometryDrawing(Brushes.Black, null, g));
				dgp.Children.Add(new GeometryDrawing(this.model.Minimap.MinimapBrush, null, g));
				dgp.Children.Add(new GeometryDrawing(this.model.Minimap.MinimapBorderBrush, null, g));
				g = new RectangleGeometry(
					new Rect(
					    (this.model.ActualMapSegment.Y * this.model.Minimap.Width / 5) + (this.model.Player.X * this.model.TileSize / 15) - (this.model.TileSize / 8),
					    this.model.GameHeight - this.model.Minimap.Height + (this.model.ActualMapSegment.X * this.model.Minimap.Height / 5) + (this.model.Player.Y * this.model.TileSize / 15) - (this.model.TileSize / 8),
					    this.model.TileSize / 4,
					    this.model.TileSize / 4));
				dgp.Children.Add(new GeometryDrawing(this.PlayerBrush, null, g));
				dgp.Opacity = Config.MapOpacity;

				this.oldMinimap = dgp;
			}

			return this.oldMinimap;
		}

		private Drawing Stats()
		{
			DrawingGroup dg = new DrawingGroup();

			// Clock.
			dg.Children.Add(this.DrawMyText(
				string.Format(
					"{0}:{1:00}",
					(int)this.model.GameTime.TotalMinutes,
					this.model.GameTime.Seconds),
				new Point(this.model.GameWidth - 25, 0),
				FlowDirection.RightToLeft,
				Config.TimeSize,
				Brushes.Transparent));

			// Hp.
			dg.Children.Add(this.DrawMyText(
				$"Hp: {this.model.GetPlayer.Hp.ToString("F0")}/{this.model.GetPlayer.MaxHP}\t{this.fps} fps",
				new Point(25, 0),
				FlowDirection.LeftToRight,
				Config.TimeSize,
				Brushes.Transparent));

			return dg;
		}

		private Drawing DrawMyText(string textString, Point where, FlowDirection fdr, int fontSize, Brush background, bool setWidth = false, double opi = 1, bool middle = false)
		{
			// Create a new DrawingGroup of the control.
			DrawingGroup drawingGroup = new DrawingGroup();

			// Open the DrawingGroup in order to access the DrawingContext.
			using (DrawingContext drawingContext = drawingGroup.Open())
			{
				// Create the formatted text based on the properties set.
				FormattedText formattedText = new FormattedText(
					textString,
					CultureInfo.GetCultureInfo("en-us"),
					fdr,
					new Typeface("Comic Sans MS Bold"),
					fontSize,
					System.Windows.Media.Brushes.Black); // This brush does not matter since we use the geometry of the text.

				if (setWidth)
				{
					formattedText.MaxTextWidth = this.model.GameWidth - where.X;
					formattedText.MaxTextHeight = 300;
				}

				Geometry textGeometry;
				if (middle)
				{
					textGeometry = formattedText.BuildGeometry(new Point(where.X - (formattedText.Width / 2), where.Y));
					if (fdr == FlowDirection.LeftToRight)
					{
						drawingContext.DrawRoundedRectangle(background, null, new Rect(where.X - (formattedText.Width / 2) - 25, where.Y, formattedText.Width + 50, formattedText.Height + 5), 7.5, 7.5);
					}
				}
				else
				{
					// Build the geometry object that represents the text.
					textGeometry = formattedText.BuildGeometry(where);
					if (fdr == FlowDirection.LeftToRight)
					{
						drawingContext.DrawRoundedRectangle(background, null, new Rect(where.X - 25, where.Y, formattedText.Width + 50, formattedText.Height + 5), 7.5, 7.5);
					}
				}

				// Draw the outline based on the properties that are set.
				drawingContext.DrawGeometry(System.Windows.Media.Brushes.Gold, new System.Windows.Media.Pen(System.Windows.Media.Brushes.Maroon, 1.5), textGeometry);
			}

			drawingGroup.Opacity = opi;

			// Return the updated DrawingGroup content to be used by the control.
			return drawingGroup;
		}

		private Drawing GetBacground()
		{
			if (this.oldBackground == null || this.oldMapPoint != this.model.ActualMapSegment)
			{
				Geometry g = new RectangleGeometry(new Rect(0, 0, this.model.GameWidth, this.model.GameHeight));
				this.oldBackground = new GeometryDrawing(this.BackgroundBrush, null, g);
			}

			return this.oldBackground;
		}

		private Drawing GetEntities()
		{
			if (this.oldEntities == null || this.oldMapPoint != this.model.ActualMapSegment)
			{
				DrawingGroup dg = new DrawingGroup();
				this.oldEntitieNames = new DrawingGroup();
				for (int i = 0; i < this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities.GetLength(0); i++)
				{
					for (int j = 0; j < this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities.GetLength(1); j++)
					{
						Geometry g = new RectangleGeometry(new Rect(j * this.model.TileSize, i * this.model.TileSize, this.model.TileSize, this.model.TileSize));
						dg.Children.Add(new GeometryDrawing(this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[i, j].EntityBrush, null, g));
						if (this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[i, j].IsCharacter &&
							this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[i, j].Name.Split('|')[0] != "NextMap" &&
							this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[i, j].Name.Split('|')[0] != "EndGame" &&
							this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[i, j].Name.Split('|')[0] != "EasterEgg")
						{
							this.oldEntitieNames.Children.Add(this.DrawMyText(
								this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[i, j].Name.Split('|')[0],
								new Point((j * this.model.TileSize) + (this.model.TileSize / 2), (i - 0.5) * this.model.TileSize),
								FlowDirection.LeftToRight,
								22,
								Brushes.Black,
								false,
								0.7,
								true));
						}
					}
				}

				this.oldEntities = dg;
			}

			return this.oldEntities;
		}

		private Drawing GetPlayer()
		{
			if (this.oldPlayer == null || this.oldPlayerPosition != this.model.Player)
			{
				Geometry g = new RectangleGeometry(new Rect(
				    this.model.Player.X * this.model.TileSize,
				    this.model.Player.Y * this.model.TileSize,
				    this.model.TileSize,
				    this.model.TileSize));
				this.oldPlayer = new GeometryDrawing(this.PlayerBrush, null, g);
			}

			return this.oldPlayer;
		}
	}
}
