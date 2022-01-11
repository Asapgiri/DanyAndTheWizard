// <copyright file="DatwRepo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DanyAndTheWizard.Repository.Interfaces;
    using DanyAndTheWizard.Repository.InternalClass;

	/// <summary>
	/// DatwRepo.
	/// </summary>
    public class DatwRepo : IDatwRepo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DatwRepo"/> class.
		/// </summary>
		/// <param name="saveLocation">Save path next to exe name.</param>
		/// <param name="settingLocation">Settings path next to exe name.</param>
		/// <param name="highscoreLocation">Highscores path next to exe name.</param>
		public DatwRepo(string saveLocation = "DATW.save", string settingLocation = "DATW.sett", string highscoreLocation = "DATW.hsc")
		{
			this.Save = new Save(saveLocation);
			this.Highscore = new Save(highscoreLocation);
			this.Settings = new Settings(settingLocation);
			this.Map = new Map();
			this.Character = new Character();
		}

		/// <summary>
		/// Gets save elements.
		/// </summary>
		public ISave Save { get; }

		/// <summary>
		/// Gets the highscore load save elements.
		/// </summary>
		public ISave Highscore { get; }

		/// <summary>
		/// Gets save elements.
		/// </summary>
		public ISettings Settings { get; }

		/// <summary>
		/// Gets the map elements.
		/// </summary>
		public IMap Map { get; }

		/// <summary>
		/// Gets character skins.
		/// </summary>
		public ICharacter Character { get; }
	}
}
