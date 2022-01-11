// <copyright file="Save.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic.InternalClasses
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using DanyAndTheWizard.Logic.Interfaces;
	using DanyAndTheWizard.Repository;

	/// <summary>
	/// Savings.
	/// </summary>
	public class Save : ISave
	{
		private DatwModel model;
		private DatwRepo repo;

		/// <summary>
		/// Initializes a new instance of the <see cref="Save"/> class.
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="repo">repo.</param>
		public Save(DatwModel model, DatwRepo repo)
		{
			this.model = model;
			this.repo = repo;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Save"/> class.
		/// </summary>
		public Save()
		{
			this.repo = new DatwRepo();
		}

		/// <inheritdoc/>
		public bool GetSave(string saveName)
		{
			Dictionary<string, string> save = this.repo.Save.GetSave(saveName);
			this.model.MapName = save["Map"];
			this.model.ActualMapSegment = Point.Parse(save["ActualMapSegment"]);
			this.model.Player = Point.Parse(save["Player"]);
			this.model.BaseTime = TimeSpan.Parse(save["GameTime"]);

			this.model.IsInteractionRunning = true;
			this.model.Dialog = new Models.DialogModel()
			{
				ActualSpeeker = "System",
				ActualDialog = "Game Loaded",
			};
			return true;
		}

		/// <inheritdoc/>
		public List<string> GetSaveNames()
		{
			return this.repo.Save.GetSaveNames();
		}

		/// <inheritdoc/>
		public bool NewSave(string saveName)
		{
			Dictionary<string, string> save = new Dictionary<string, string>();
			save.Add("Map", this.model.MapName);
			save.Add("ActualMapSegment", this.model.ActualMapSegment.ToString());
			save.Add("Player", this.model.Player.ToString());
			save.Add("GameTime", this.model.GameTime.ToString());

			this.repo.Save.NewSave(saveName, save);
			this.model.IsInteractionRunning = true;
			this.model.Dialog = new Models.DialogModel()
			{
				ActualSpeeker = "System",
				ActualDialog = "Game Saved",
			};

			// return this.SaveScore(saveName);
			return true;
		}

		/// <summary>
		/// Saves the highscores.
		/// </summary>
		/// <param name="name">Saves name.</param>
		/// <returns>Saving was successfull.</returns>
		public bool SaveScore(string name)
		{
			Dictionary<string, string> score = new Dictionary<string, string>();
			score.Add("Score", ((TimeSpan.TicksPerHour / this.model.GameTime.Ticks) * this.model.Highscores.Score).ToString());
			score.Add("Kills", this.model.Highscores.EnemiesKilled.ToString());
			score.Add("Time", string.Format("{0}:{1:00}:{2:00}", this.model.GameTime.Hours, this.model.GameTime.Minutes, this.model.GameTime.Seconds));

			this.repo.Highscore.NewSave(name, score);

			return true;
		}

		/// <summary>
		/// Gets the scores that are collected.
		/// </summary>
		/// <returns>The scores that are documented.</returns>
		public List<Dictionary<string, string>> GetScores()
		{
			List<Dictionary<string, string>> ls = new List<Dictionary<string, string>>();
			foreach (string item in this.repo.Highscore.GetSaveNames())
			{
				ls.Add(this.repo.Highscore.GetSave(item));
			}

			return ls;
		}
	}
}
