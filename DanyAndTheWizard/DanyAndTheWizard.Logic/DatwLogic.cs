// <copyright file="DatwLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Windows;
	using System.Windows.Media;
	using System.Windows.Media.Imaging;
	using DanyAndTheWizard.Logic.Interfaces;
	using DanyAndTheWizard.Logic.InternalClasses;
	using DanyAndTheWizard.Logic.Models;
	using DanyAndTheWizard.Repository;

	/// <summary>
	/// DatwLogic.
	/// </summary>
	public class DatwLogic : IDatwLogic
	{
		private DatwRepo repo;
		private DatwModel model;
		private Dictionary<string, Dictionary<string, List<string>>> npcSpeaches;
		private Random rnd = new Random();

		/// <summary>
		/// Initializes a new instance of the <see cref="DatwLogic"/> class.
		/// Generates logic.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="fromSave">Load from save.</param>
		/// <param name="saveName">The saves name.</param>
		public DatwLogic(DatwModel model, bool fromSave = false, string saveName = "Bela")
		{
			this.model = model;
			this.repo = new DatwRepo("DATW.save", "DATW.sett", "DATW.hsc");
			this.npcSpeaches = this.repo.Map.GetNpcSpeach("Npc_Texts");

			this.InitModel("City");
			this.Save = new Save(this.model, this.repo);
			this.Fight = new Fight(this.model);

			// this.Save.NewSave("Test2");
		}

		/// <summary>
		/// An event to handle if the model changed.
		/// </summary>
		public event EventHandler ModelChanged;

		/// <summary>
		/// Dropped when the game ends.
		/// </summary>
		public event EventHandler EndGameEvent;

		/// <summary>
		/// An enum for the current direction to set character skin status.
		/// </summary>
		public enum Direction
		{
			/// <summary>
			/// Right.
			/// </summary>
			Right,

			/// <summary>
			/// Left.
			/// </summary>
			Left,

			/// <summary>
			/// Up.
			/// </summary>
			Up,

			/// <summary>
			/// Down.
			/// </summary>
			Down,
		}

		private enum MapSegment
		{
			Right,
			Left,
			Up,
			Down,
		}

		/// <summary>
		/// Gets or sets save.
		/// </summary>
		public ISave Save { get; set; }

		/// <summary>
		/// Gets or sets fight.
		/// </summary>
		public IFight Fight { get; set; }

		/// <summary>
		/// To initialize the model first time.
		/// </summary>
		/// <param name="lvlName">Map name.</param>
		public void InitModel(string lvlName)
		{
			this.model.Map = new MapModel[5, 5];
			this.model.MapName = lvlName;
			if (this.model.Highscores == null)
			{
				this.model.Highscores = new ScoreModel();
				this.model.Highscores.EnemiesKilled = 0;
			}

			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.StreamSource = this.repo.Map.GetMap(lvlName);
			bmp.EndInit();

			this.InitMap(bmp);
			this.InitMinimap(bmp);
			if (this.model.GetPlayer == null)
			{
				this.InitPlayer();
			}
		}

		/// <summary>
		/// To equip an item from your inventory.
		/// ~
		/// ERRORNS:
		/// ~ Maybe the item wasn't in the inventorí?!?
		/// ~ At least i didn't handle that.
		/// </summary>
		/// <param name="gi">Item to equip. In theory from the inventory.</param>
		public void EquipItem(GameItem gi)
		{
			if (!this.model.GetPlayer.Inventory.Find(x => x.Id == gi.Id).IsEquipped)
			{
				foreach (GameItem item in this.model.GetPlayer.Inventory.FindAll(x => x.Type == gi.Type))
				{
					item.IsEquipped = false;
				}

				if (this.model.GetPlayer.Equipped == null)
				{
					this.model.GetPlayer.Equipped = new EquippedModel();
				}

				switch (gi.Type)
				{
					case ItemType.None:
						break;
					case ItemType.Weapon:
						this.model.GetPlayer.Equipped.Weapon = gi;
						break;
					case ItemType.Armor:
						this.model.GetPlayer.Equipped.Armor = gi;
						break;
					case ItemType.Neutral:
						break;
					case ItemType.Accessory:
						this.model.GetPlayer.Equipped.Accessory = gi;
						break;
					default:
						break;
				}

				this.model.GetPlayer.Inventory.Find(x => x.Id == gi.Id).IsEquipped = true;
			}
		}

		/// <summary>
		/// To unequip an item in your inventorí.
		/// Most likly it's in the inventory.
		/// </summary>
		/// <param name="gi">An item in the inventory.</param>
		public void UnEquipItem(GameItem gi)
		{
			switch (gi.Type)
			{
				case ItemType.None:
					break;
				case ItemType.Weapon:
					this.model.GetPlayer.Equipped.Weapon = null;
					break;
				case ItemType.Armor:
					this.model.GetPlayer.Equipped.Armor = null;
					break;
				case ItemType.Neutral:
					break;
				case ItemType.Accessory:
					this.model.GetPlayer.Equipped.Accessory = null;
					break;
				default:
					break;
			}

			this.model.GetPlayer.Inventory.Find(x => x.Id == gi.Id).IsEquipped = false;
		}

		/// <summary>
		/// Using an item from your inventory.
		/// </summary>
		/// <param name="gi">Item to use.</param>
		public void UseItem(GameItem gi)
		{
			if (this.model.GetPlayer.Hp + gi.Health > this.model.GetPlayer.MaxHP)
			{
				this.model.GetPlayer.Hp = this.model.GetPlayer.MaxHP;
			}
			else
			{
				this.model.GetPlayer.Hp += gi.Health;
			}

			this.model.IsInteractionRunning = true;
			this.model.Dialog.ActualSpeeker = "System";
			this.model.Dialog.ActualDialog = $"You have been gained: +{gi.Health} Hp";
			this.model.GetPlayer.Inventory.Remove(gi);
		}

		/// <summary>
		/// Adding an item to the players inventory.
		/// </summary>
		/// <param name="item">The items name.</param>
		public void AddItemToInventory(string item)
		{
			if (this.model.GetPlayer.Inventory.Count <= this.model.GetPlayer.InventorySize)
			{
				GameItem gi = this.GetItem(item);
				this.model.GetPlayer.Inventory.Add(gi);
				this.model.IsInteractionRunning = true;
				this.model.Dialog.ActualSpeeker = "System";
				this.model.Dialog.ActualDialogQueue.Enqueue("An item has been gained:\n" + gi);
			}
			else
			{
				GameItem gi = this.GetItem(item);
				this.model.GetPlayer.Inventory.Add(gi);
				this.model.IsInteractionRunning = true;
				this.model.Dialog.ActualSpeeker = "System";
				this.model.Dialog.ActualDialogQueue.Enqueue("Your inventory is full but we added an item anyway, because there is no system to get it later...\nAn item has been gained:\n" + gi);
			}
		}

		/// <summary>
		/// An interaction after the characters "speach".
		/// </summary>
		public void CharacterInteraction()
		{
			switch (this.model.Dialog.ActualSpeeker)
			{
				case "Wizard":
					switch (this.model.Dialog.ActualScene)
					{
						case "Intro":
						case "Forest":
						case "Field":
							this.DeleteObjectFromMap((int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y);
							this.ModelChanged.Invoke(this, new EventArgs());
							break;
						case "Frazer":
							this.EndGame();
							break;
						default:
							break;
					}

					break;

				case "Zoe":
					switch (this.model.Dialog.ActualScene)
					{
						case "Forest":
							this.DeleteObjectFromMap((int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y);
							this.model.EasterEggUnlocked = true;
							this.ModelChanged.Invoke(this, new EventArgs());
							break;
						default:
							break;
					}

					break;
				case "EasterEgg":
					if (this.model.EasterEggUnlocked)
					{
						this.CharacterInteraction(new EntityModel()
						{
							Name = this.model.Dialog.ActualSpeeker + "|CanHave",
						});
						this.AddItemToInventory("Masterpiece");
					}
					else
					{
						if (!this.model.Dialog.CantSayTwice)
						{
							this.CharacterInteraction(new EntityModel()
							{
								Name = this.model.Dialog.ActualSpeeker + "|DontHave",
							});
							this.model.Dialog.CantSayTwice = true;
						}
					}

					this.DeleteObjectFromMap((int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y);
					this.ModelChanged.Invoke(this, new EventArgs());

					break;

				case "Bandit":
				case "Goblin":
				case "WizardE":
				case "Robot":
				case "OrcKnight":
				case "Knight":
				case "Frazer":
					if (!this.model.Dialog.CantSayTwice)
					{
						this.CharacterInteraction(new EntityModel()
						{
							Name = this.model.Dialog.ActualSpeeker + "|" + this.model.Dialog.ActualScene,
						});
					}

					switch (this.model.Dialog.ActualScene)
					{
						case "Speach":
							this.model.Fight.Enemy = this.GetEnemy(this.model.Dialog.ActualSpeeker);
							this.Fight.StartFight();
							this.model.Dialog.ActualScene = string.Empty;

							break;
						case "Victory":
							this.DeleteObjectFromMap((int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y);
							this.ModelChanged.Invoke(this, new EventArgs());
							this.model.Dialog.CantSayTwice = true;
							if (!this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[(int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y].IsInteracted)
							{
								this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[(int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y].IsInteracted = true;
								switch (this.model.Dialog.ActualSpeeker)
								{
									case "Bandit": this.AddItemToInventory("Apple"); break;
									case "WizardE": case "Goblin": this.AddItemToInventory("Nyans_Bread"); break;
									case "Robot":
										this.AddItemToInventory("Maples_Blood");
										this.AddItemToInventory("Mapels_Log");
										this.AddItemToInventory("Mapels_Chest");
										this.AddItemToInventory("Mapple");
										break;
									case "OrcKnight":
										this.AddItemToInventory("Nyan_Rod");
										this.AddItemToInventory("Nyan_chest");
										this.AddItemToInventory("Nyans_Bread");
										break;
									case "Knight":
										this.AddItemToInventory("Kerrys_Axe");
										this.AddItemToInventory("Kerrys_chest");
										this.AddItemToInventory("Better_Bread");
										break;
									case "Frazer":
										for (int i = 1; i < 8; i++)
										{
											for (int j = 0; j < 14; j++)
											{
												this.DeleteObjectFromMap(i, j);
											}
										}

										this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[6, 10] = new EntityModel()
										{
											EntityBrush = this.GetEntities("Wizard|Frazer"),
											Name = "Wizard|Frazer",
											IsCharacter = true,
										};
										this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[6, 11] = new EntityModel()
										{
											EntityBrush = this.GetEntities("Queen|Sexy"),
											Name = "Queen|Sexy",
											IsCharacter = true,
										};
										break;
									default:
										break;
								}
							}

							break;
						case "Defeat":
							this.model.Dialog.CantSayTwice = true;
							this.ModelChanged.Invoke(this, new EventArgs());
							break;
						default:
							break;
					}

					break;

				case "TownNPC":
					if (!this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[(int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y].IsInteracted)
					{
						this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[(int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y].IsInteracted = true;
						switch (this.model.Dialog.ActualScene)
						{
							case "City": this.AddItemToInventory("Apple"); break;
							case "Field": this.AddItemToInventory("Apple"); break;
							default:
								break;
						}
					}

					break;

				case "NPC1":
					if (!this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[(int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y].IsInteracted)
					{
						this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[(int)this.model.Dialog.Location.X, (int)this.model.Dialog.Location.Y].IsInteracted = true;
						switch (this.model.Dialog.ActualScene)
						{
							case "City": this.AddItemToInventory("Bread"); break;
							case "Field": this.AddItemToInventory("Mapple"); break;
							case "Extra": this.AddItemToInventory("Mapels_Log"); break;
							default:
								break;
						}
					}

					break;

				case "NextMap":
					switch (this.model.Dialog.ActualScene)
					{
						case "First": this.LoadNextMap("Forest"); this.model.Player = new Point(10.5, 1); break;
						case "Second": this.LoadNextMap("Field"); this.model.Player = new Point(1, 3.5); break;
						case "Third": this.LoadNextMap("Frazer"); this.model.ActualMapSegment = new Point(1, 2); this.model.Player = new Point(8, 1); this.model.EasterEggUnlocked = true; break;
						case "Test": this.LoadNextMap(this.model.MapName); break;
						default:
							break;
					}

					break;

				case "EndGame":
					this.EndGame();
					this.Save.SaveScore((this.repo.Highscore.GetSaveNames().Count + 1).ToString());
					break;
				default:
					break;
			}

			// if (this.model.Fight != null && !this.model.Fight.IsInFight)
			// {

			// }
			if (!this.model.Fight.IsInFight && !this.model.IsInteractionRunning && this.model.Dialog.ActualSpeeker != "Dany")
			{
				this.CharacterInteraction(new EntityModel()
				{
					Name = $"Dany|{this.model.Dialog.ActualScene}",
				});
			}
		}

		/// <summary>
		/// Sets the acrual brushes for the character according to moving.
		/// </summary>
		/// <param name="direction">Enum with the direction.</param>
		public void CharacterDirection(Direction direction)
		{
			BitmapImage player = new BitmapImage();
			BitmapImage player1 = new BitmapImage();
			BitmapImage player2 = new BitmapImage();
			player.BeginInit();
			player1.BeginInit();
			player2.BeginInit();
			switch (direction)
			{
				case Direction.Right:
					player.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Right");
					player1.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Right1");
					player2.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Right2");
					break;
				case Direction.Left:
					player.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Left");
					player1.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Left1");
					player2.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Left2");
					break;
				case Direction.Up:
					player.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Back");
					player1.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Back1");
					player2.StreamSource = this.repo.Character.GetCharacterBrush("Dany_Back2");
					break;
				case Direction.Down:
					player.StreamSource = this.repo.Character.GetCharacterBrush("Dany");
					player1.StreamSource = this.repo.Character.GetCharacterBrush("Dany1");
					player2.StreamSource = this.repo.Character.GetCharacterBrush("Dany2");
					break;
				default:
					break;
			}

			player.EndInit();
			player1.EndInit();
			player2.EndInit();

			/*this.chModel.CharacterBrush = new List<ImageBrush>()
			{
					new ImageBrush(player) { Stretch = Stretch.Uniform },
					new ImageBrush(player1) { Stretch = Stretch.Uniform },
					new ImageBrush(player2) { Stretch = Stretch.Uniform },
			};*/

			// this.model.GetPlayer = this.chModel;
			switch (this.model.CharacterTimer)
			{
				case 0:
					this.model.CurrentPlayerBrush = new ImageBrush(player) { Stretch = Stretch.Uniform };
					break;
				case 1:
					this.model.CurrentPlayerBrush = new ImageBrush(player1) { Stretch = Stretch.Uniform };
					break;
				case 2:
					this.model.CurrentPlayerBrush = new ImageBrush(player2) { Stretch = Stretch.Uniform };
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// To move your caracter on the screen.
		/// </summary>
		/// <param name="dx">The noving differency x.</param>
		/// <param name="dy">The noving differency y.</param>
		/// <returns>Returns the player could move.</returns>
		public bool Move(double dx, double dy)
		{
			double newX = this.model.Player.X + dx;
			double newY = this.model.Player.Y + dy;

			if (this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y]
				.Entities[(int)Math.Ceiling(newY), (int)Math.Round(newX)].EntityBrush == null &&
				!this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y]
				.Entities[(int)Math.Ceiling(newY), (int)Math.Round(newX)].IsCharacter &&
				!this.model.IsInteractionRunning &&
				!this.model.Fight.IsInFight)
			{
				if (newX < Config.InvFrameSize)
				{
					return this.ChangeMapSegment(MapSegment.Left, this.model.Player);
				}
				else if (newY < Config.InvFrameSize)
				{
					return this.ChangeMapSegment(MapSegment.Up, this.model.Player);
				}
				else if (newX > 15 - Config.InvFrameSize)
				{
					return this.ChangeMapSegment(MapSegment.Right, this.model.Player);
				}
				else if (newY > 7.6 - Config.InvFrameSize)
				{
					return this.ChangeMapSegment(MapSegment.Down, this.model.Player);
				}
				else
				{
					this.model.Player = new Point(newX, newY);
				}

				return true;
			}
			else if (this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y]
				.Entities[(int)Math.Ceiling(newY), (int)Math.Round(newX)].IsCharacter &&
				!this.model.IsInteractionRunning &&
				!this.model.Fight.IsInFight)
			{
				this.model.Dialog = null;
				this.CharacterInteraction(this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y]
				.Entities[(int)Math.Ceiling(newY), (int)Math.Round(newX)]);
				this.model.Dialog.Location = new Point((int)Math.Ceiling(newY), (int)Math.Round(newX));
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// To begin a fight.
		/// </summary>
		public void BeginFight()
		{
			this.model.Fight = new FightModel()
			{
				Enemy = this.GetEnemy(this.model.Dialog.ActualSpeeker),
			};

			this.Fight.StartFight();
		}

		/// <summary>
		/// Your enemy will attack randomly.
		/// </summary>
		public void EnemyRandomArrack()
		{
			string s = string.Empty;

			switch (this.rnd.Next(0, 4))
			{
				case 0:
					s = "FireBall";
					break;
				case 1:
					s = "RiceBall";
					break;
				case 2:
					s = "Push";
					break;
				case 3:
					s = "equipped";
					break;
				default:
					break;
			}

			if (s != string.Empty)
			{
				this.Fight.EnemyAttack(s);
			}
		}

		private CharacterModel GetEnemy(string enemy)
		{
			switch (enemy)
			{
				case "Bandit":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy + "Fight"),
						Name = enemy,
						Age = 24,
						Description = "Someone who is in the wrong path.",
						CharacterType = "Enemy",
						Hp = 100,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Arduins_rod"),
							Armor = this.GetItem("Allahs_chest"),
						},
					};
				case "Goblin":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy),
						Name = enemy,
						Age = 36,
						Description = "They are \"just\" GOBLINS.",
						CharacterType = "Enemy",
						Hp = 150,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Arduins_rod"),
							Armor = this.GetItem("Allahs_chest"),
						},
					};
				case "WizardE":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy + "Fight"),
						Name = enemy,
						Age = 40,
						Description = "A black wizard.",
						CharacterType = "Enemy",
						Hp = 200,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Nyan_Rod"),
							Armor = this.GetItem("Nyan_chest"),
						},
					};
				case "Robot":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy + "Fight"),
						Name = enemy,
						Age = 50,
						Description = "A robot.",
						CharacterType = "Boss",
						Hp = 400,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Kerrys_Axe"),
							Armor = this.GetItem("Kerrys_chest"),
						},
					};

				// Bosses:
				case "OrcKnight":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy + "Fight"),
						Name = enemy,
						Age = 40,
						Description = "Just a big Orc.",
						CharacterType = "Boss",
						Hp = 200,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Arduins_rod"),
							Armor = this.GetItem("Allahs_chest"),
						},
					};
				case "Knight":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy + "Fight"),
						Name = enemy,
						Age = 40,
						Description = "A knight.",
						CharacterType = "Boss",
						Hp = 300,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Arduins_rod"),
							Armor = this.GetItem("Allahs_chest"),
						},
					};
				case "Frazer":
					return new CharacterModel()
					{
						FightBrush = this.GetEntities(enemy),
						Name = enemy,
						Age = 25,
						Description = "A knight.",
						CharacterType = "Boss",
						Hp = 500,
						Equipped = new EquippedModel()
						{
							Weapon = this.GetItem("Kerrys_Axe"),
							Armor = this.GetItem("Kerrys_chest"),
						},
					};

				default:
					throw new NotImplementedException();
			}
		}

		private void InitMap(BitmapImage bmp)
		{
			string[,] str = this.repo.Map.GetMapElements(this.model.MapName);
			ImageBrush ib;
			for (int i = 0; i < this.model.Map.GetLength(0); i++)
			{
				for (int j = 0; j < this.model.Map.GetLength(1); j++)
				{
					ib = new ImageBrush(bmp)
					{
						Viewbox = new Rect(j * 0.2, i * 0.2, 0.2, 0.2),
						ViewboxUnits = BrushMappingMode.RelativeToBoundingBox,
					};
					this.model.Map[i, j].MapBrush = ib;
					this.model.Map[i, j].Entities = new EntityModel[str.GetLength(0) / 5, str.GetLength(1) / 5];
					for (int k = 0; k < str.GetLength(0) / 5; k++)
					{
						for (int l = 0; l < str.GetLength(1) / 5; l++)
						{
							this.model.Map[i, j].Entities[k, l] = new EntityModel()
							{
								EntityBrush = this.GetEntities(str[(i * str.GetLength(0) / 5) + k, (j * str.GetLength(1) / 5) + l]),
								Name = str[(i * str.GetLength(0) / 5) + k, (j * str.GetLength(1) / 5) + l],
							};

							this.model.Map[i, j].Entities[k, l].
								IsCharacter = this.model.Map[i, j].Entities[k, l].Name != null ? this.npcSpeaches.ContainsKey(str[(i * str.GetLength(0) / 5) + k, (j * str.GetLength(1) / 5) + l].Split('|')[0]) : false;
						}
					}
				}
			}
		}

		private void InitMinimap(BitmapImage bmp)
		{
			BitmapImage minMapCorner = new BitmapImage();
			minMapCorner.BeginInit();
			minMapCorner.StreamSource = this.repo.Map.GetElement("MinimapBorder");
			minMapCorner.EndInit();

			this.model.Minimap = new MinimapModel()
			{
				MinimapBrush = new ImageBrush(bmp),
				MinimapBorderBrush = new ImageBrush(minMapCorner),
				Width = this.model.GameWidth / 3,
				Height = this.model.GameHeight / 3,
				Visible = this.model.Minimap != null ? this.model.Minimap.Visible : true,
			};
		}

		private void LoadNextMap(string v)
		{
			this.model.ActualMapSegment = new Point(0, 0);
			this.model.GetPlayer.MaxHP += 50;
			this.InitModel(v);
			this.ModelChanged.Invoke(this, new EventArgs());
		}

		private void InitPlayer()
		{
			BitmapImage player = new BitmapImage();
			player.BeginInit();
			player.StreamSource = this.repo.Character.GetCharacterBrush("Dany");
			player.EndInit();

			BitmapImage player1 = new BitmapImage();
			player1.BeginInit();
			player1.StreamSource = this.repo.Character.GetCharacterBrush("Dany1");
			player1.EndInit();

			BitmapImage player2 = new BitmapImage();
			player2.BeginInit();
			player2.StreamSource = this.repo.Character.GetCharacterBrush("Dany2");
			player2.EndInit();

			BitmapImage playerFight = new BitmapImage();
			playerFight.BeginInit();
			playerFight.StreamSource = this.repo.Character.GetCharacterBrush("DanyFight");
			playerFight.EndInit();

			this.model.GetPlayer = new CharacterModel()
			{
				CharacterBrush = new List<ImageBrush>()
				{
					new ImageBrush(player) { Stretch = Stretch.Uniform },
					new ImageBrush(player1) { Stretch = Stretch.Uniform },
					new ImageBrush(player2) { Stretch = Stretch.Uniform },
				},
				FightBrush = new ImageBrush(playerFight) { Stretch = Stretch.Uniform },
				CharacterType = "Player",
				Name = "Dany",
				Age = 25,
				Description = "Currently not defined",
				MaxHP = 100,
				Hp = 100,
				Inventory = this.InitInventory(),
				InventorySize = 40,
			};

			foreach (GameItem item in this.model.GetPlayer.Inventory)
			{
				this.EquipItem(item);
			}

			this.model.CurrentPlayerBrush = new ImageBrush(player) { Stretch = Stretch.Uniform };
			this.model.CharacterTimer = 0;
		}

		private List<GameItem> InitInventory()
		{
			List<GameItem> li = new List<GameItem>()
			{
				this.GetItem("Arduins_rod"),
				this.GetItem("Allahs_chest"),

				// this.GetItem("Triangle"),
				// this.GetItem("Masterpiece"),
				this.GetItem("Bread"),
				this.GetItem("Apple"),
			};

			return li;
		}

		private GameItem GetItem(string v)
		{
			switch (v)
			{
				// Tier 1
				case "Arduins_rod":
					return new GameItem()
					{
						Id = 10,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 30,
						Defense = 0,
						Description = "The great rod of Arduin",
						Type = ItemType.Weapon,
						CriticalChance = 0.1,
						Rarity = "Common",
					};

				case "Allahs_chest":
					return new GameItem()
					{
						Id = 11,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 0,
						Defense = 10,
						Description = "The great chest of Allah",
						Type = ItemType.Armor,
						CriticalChance = 0,
						Rarity = "Common",
					};

				case "Bread":
					return new GameItem()
					{
						Id = 12,
						Name = v,
						Image = this.GetItemImage(v),
						Health = 20,
						Damage = int.MaxValue,
						Defense = 0,
						Description = "Your Grandmothers bakery.",
						Type = ItemType.Neutral,
						CriticalChance = 1,
						Rarity = "Common",
					};
				case "Apple":
					return new GameItem()
					{
						Id = 13,
						Name = v,
						Image = this.GetItemImage(v),
						Health = 10,
						Damage = 0,
						Defense = 0,
						Description = "Every day an apple.. Hmm I hate apples.",
						Type = ItemType.Neutral,
						CriticalChance = 1,
						Rarity = "Common",
					};

				// Tier 2
				case "Nyan_Rod":
					return new GameItem()
					{
						Id = 24,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 50,
						Defense = 0,
						Description = "The great rod of Nyanners",
						Type = ItemType.Weapon,
						CriticalChance = 0.3,
						Rarity = "Rare",
					};

				case "Nyan_chest":
					return new GameItem()
					{
						Id = 23,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 0,
						Defense = 40,
						Description = "The great chest of Nyanners",
						Type = ItemType.Armor,
						CriticalChance = 0,
						Rarity = "Rare",
					};

				case "Nyans_Bread":
					return new GameItem()
					{
						Id = 22,
						Name = v,
						Image = this.GetItemImage(v),
						Health = 30,
						Damage = int.MaxValue,
						Defense = 0,
						Description = "Nyanners bakery. It looks like a pie pss. Hwo knows what thy put in.",
						Type = ItemType.Neutral,
						CriticalChance = 1,
						Rarity = "Rare",
					};

				// Tier 3
				case "Kerrys_Axe":
					return new GameItem()
					{
						Id = 21,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 75,
						Defense = 0,
						Description = "The great rod of Arduin",
						Type = ItemType.Weapon,
						CriticalChance = 0.5,
						Rarity = "Epic",
					};

				case "Kerrys_chest":
					return new GameItem()
					{
						Id = 20,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 0,
						Defense = 60,
						Description = "The great chest of Kerry",
						Type = ItemType.Armor,
						CriticalChance = 0,
						Rarity = "Epic",
					};

				case "Better_Bread":
					return new GameItem()
					{
						Id = 18,
						Name = v,
						Image = this.GetItemImage(v),
						Health = 50,
						Damage = int.MaxValue,
						Defense = 0,
						Description = "Defenetly not your Grandmothers bakery.",
						Type = ItemType.Neutral,
						CriticalChance = 1,
						Rarity = "Epic",
					};

				// Tier 4
				case "Mapels_Log":
					return new GameItem()
					{
						Id = 14,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 100,
						Defense = 0,
						Description = "The great rod of Maple the Great.",
						Type = ItemType.Weapon,
						CriticalChance = 1,
						Rarity = "Legendary",
					};

				case "Mapels_Chest":
					return new GameItem()
					{
						Id = 15,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 0,
						Defense = 120,
						Description = "The great chest of Maple..\nYou think her real one is better don't you ;)",
						Type = ItemType.Armor,
						CriticalChance = 0,
						Rarity = "Legendary",
					};

				case "Maples_Blood":
					return new GameItem()
					{
						Id = 16,
						Name = v,
						Image = this.GetItemImage(v),
						Health = 200,
						Damage = int.MaxValue,
						Defense = 0,
						Description = "Maples bakery.",
						Type = ItemType.Neutral,
						CriticalChance = 1,
						Rarity = "Legendary",
					};
				case "Mapple":
					return new GameItem()
					{
						Id = 17,
						Name = v,
						Image = this.GetItemImage(v),
						Health = 100,
						Damage = 0,
						Defense = 0,
						Description = "Every day a Mapple.. Hmm Mapples <3",
						Type = ItemType.Neutral,
						CriticalChance = 1,
						Rarity = "Legendary",
					};

				// For testing cases
				case "Triangle":
					return new GameItem()
					{
						Id = 13,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = 0,
						Defense = int.MaxValue,
						Description = "The great chest of Allah",
						Type = ItemType.Armor,
						CriticalChance = 0,
						Rarity = "Common",
					};
				case "Thot":
					return new GameItem()
					{
						Id = 25,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = int.MaxValue,
						Defense = 0,
						Description = "[...]",
						Type = ItemType.Weapon,
						CriticalChance = 1,
						Rarity = "Common",
					};
				case "Masterpiece":
					return new GameItem()
					{
						Id = 256,
						Name = v,
						Image = this.GetItemImage(v),
						Damage = int.MaxValue,
						Defense = 0,
						Description = "Soo rare that you couldnt evan have that... You are so lucky to find it [...]",
						Type = ItemType.Weapon,
						CriticalChance = 1,
						Rarity = "UltraRareLegendary",
					};

				default:
					break;
			}

			throw new KeyNotFoundException();
		}

		private BitmapImage GetItemImage(string v)
		{
			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.StreamSource = this.repo.Map.GetElement(v);
			if (bmp.StreamSource != null)
			{
				bmp.EndInit();
				return bmp;
			}
			else
			{
				return null;
			}
		}

		private void CharacterInteraction(EntityModel entity)
		{
			string[] a = entity.Name.Split('|');
			if (this.model.Dialog == null || (this.npcSpeaches[a[0]].ContainsKey(a[1]) && entity.Name != $"{this.model.Dialog.ActualSpeeker}|Speach"))
			{
				this.model.IsInteractionRunning = true;
				this.model.Dialog = new DialogModel()
				{
					ActualSpeeker = a[0],
					ActualScene = a[1],
					ActualDialogQueue = new Queue<string>(this.npcSpeaches[a[0]][a[1]]),
					Location = this.model.Dialog != null ? this.model.Dialog.Location : new Point(0, 0),
				};
				this.model.Dialog.ActualDialog = $"[{this.model.Dialog.ActualSpeeker}]:\n{this.model.Dialog.ActualDialogQueue.Dequeue()}";
			}
		}

		private bool ChangeMapSegment(MapSegment mapDirection, Point player)
		{
			switch (mapDirection)
			{
				case MapSegment.Right:
					if (this.model.ActualMapSegment.Y + 1 < this.model.Map.GetLength(1))
					{
						this.model.ActualMapSegment = new Point(this.model.ActualMapSegment.X, this.model.ActualMapSegment.Y + 1);
						this.model.Player = new Point(Config.InvFrameSize, player.Y);
					}

					break;
				case MapSegment.Left:
					if (this.model.ActualMapSegment.Y - 1 >= 0)
					{
						this.model.ActualMapSegment = new Point(this.model.ActualMapSegment.X, this.model.ActualMapSegment.Y - 1);
						this.model.Player = new Point(15 - Config.InvFrameSize, player.Y);
					}

					break;
				case MapSegment.Up:
					if (this.model.ActualMapSegment.X - 1 >= 0)
					{
						this.model.ActualMapSegment = new Point(this.model.ActualMapSegment.X - 1, this.model.ActualMapSegment.Y);
						this.model.Player = new Point(player.X, 7.6 - Config.InvFrameSize);
					}

					break;
				case MapSegment.Down:
					if (this.model.ActualMapSegment.X + 1 < this.model.Map.GetLength(0))
					{
						this.model.ActualMapSegment = new Point(this.model.ActualMapSegment.X + 1, this.model.ActualMapSegment.Y);
						this.model.Player = new Point(player.X, Config.InvFrameSize);
					}

					break;
				default:
					return false;
			}

			return false;
		}

		private Brush GetEntities(string elementName)
		{
			BitmapImage bmp = new BitmapImage();
			bmp.BeginInit();
			bmp.StreamSource = this.repo.Map.GetElement("Slates_v2");
			bmp.EndInit();
			ImageBrush ib;
			switch (elementName)
			{
				case "Cactus":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(320, 223);
					return ib;
				case "TPWall": // Láthatatlan fal.
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(944, 63);
					return ib;
				case "Bush":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(448, 607);
					return ib;
				case "Tussko":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(415, 576);
					return ib;
				case "Ftussko":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(383, 576);
					return ib;
				case "Kut0":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(832, 544);
					return ib;
				case "Kut1":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(832, 576);
					return ib;
				case "Kut2":
				case "EasterEgg|TheEgg":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(864, 544);
					return ib;
				case "Kut3":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(864, 576);
					return ib;
				case "NagyFa0":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(384, 608);
					return ib;
				case "NagyFa1":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(416, 608);
					return ib;
				case "NagyFa2":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(384, 640);
					return ib;
				case "NagyFa3":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(416, 640);
					return ib;
				case "NagyFa4":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(384, 672);
					return ib;
				case "NagyFa5":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(416, 672);
					return ib;
				case "Pad":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(1728, 634);
					return ib;
				case "Szikla":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(256, 320);
					return ib;
				case "Varrom":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(1280, 416);
					return ib;

				// Fenyő erdő
				case "ErdoFVeg1":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(256, 416);
					return ib;
				case "ErdoFVeg2":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(288, 416);
					return ib;
				case "Erdo1":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(256, 448);
					return ib;
				case "Erdo2":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(288, 448);
					return ib;
				case "Erdo3":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(256, 480);
					return ib;
				case "Erdo4":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(288, 480);
					return ib;
				case "ErdoAVeg1":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(256, 512);
					return ib;
				case "ErdoAVeg2":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(288, 512);
					return ib;

				case "ErdoFVeg3":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(320, 416);
					return ib;
				case "ErdoFVeg4":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(352, 416);
					return ib;
				case "Erdo5":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(320, 448);
					return ib;
				case "Erdo6":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(352, 448);
					return ib;
				case "Erdo7":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(320, 480);
					return ib;
				case "Erdo8":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(352, 480);
					return ib;
				case "ErdoAVeg3":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(320, 512);
					return ib;
				case "ErdoAVeg4":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(352, 512);
					return ib;
				case "Kishenyo":
					ib = new ImageBrush(bmp);
					ib.Viewbox = this.GetVBRect(320, 576);
					return ib;

				// Characters:
				case "NextMap|Test":
					BitmapImage nextMap = new BitmapImage();
					nextMap.BeginInit();
					nextMap.StreamSource = this.repo.Character.GetCharacterBrush("Wizard");
					nextMap.EndInit();
					ib = new ImageBrush(nextMap)
					{
						Stretch = Stretch.Uniform,
					};
					return ib;

				case "Wizard|Intro":
				case "Wizard|Forest":
				case "Wizard|Field":
				case "Zoe|Forest":
				case "Bandit|Speach":
				case "WizardE|Speach":
				case "Goblin|Speach":
				case "OrcKnight|Speach":
				case "Knight|Speach":
				case "Robot|Speach":
				case "Frazer|Speach":
				case "TownNPC|City":
				case "NPC1|City":
				case "NPC1|Field":
				case "NPC1|Frazer1":
				case "NPC1|Frazer2":
				case "Queen|Sexy":
					BitmapImage lmnt = new BitmapImage();
					lmnt.BeginInit();
					lmnt.StreamSource = this.repo.Character.GetCharacterBrush(elementName.Split('|')[0]);
					lmnt.EndInit();
					ib = new ImageBrush(lmnt)
					{
						Stretch = Stretch.Uniform,
					};
					return ib;
				case "Wizard|Frazer":
					BitmapImage w = new BitmapImage();
					w.BeginInit();
					w.StreamSource = this.repo.Character.GetCharacterBrush("WizardF");
					w.EndInit();
					ib = new ImageBrush(w)
					{
						Stretch = Stretch.Uniform,
					};
					return ib;
				case "BanditFight":
				case "WizardEFight":
				case "GoblinFight":
				case "OrcKnightFight":
				case "KnightFight":
				case "RobotFight":
				case "Frazer":
					BitmapImage fgh = new BitmapImage();
					fgh.BeginInit();
					fgh.StreamSource = this.repo.Character.GetCharacterBrush(elementName);
					fgh.EndInit();
					ib = new ImageBrush(fgh)
					{
						Stretch = Stretch.Uniform,
					};
					return ib;
				default:
					if (this.repo.Character.GetCharacterBrush(elementName) != null)
					{
						BitmapImage def = new BitmapImage();
						def.BeginInit();
						def.StreamSource = this.repo.Character.GetCharacterBrush(elementName);
						def.EndInit();
						ib = new ImageBrush(def)
						{
							Stretch = Stretch.Uniform,
						};
						return ib;
					}

					break;
			}

			return null;
		}

		private Rect GetVBRect(double widthInPx, double heightInPx)
		{
			widthInPx = widthInPx / 1792;
			heightInPx = heightInPx / 736;

			return new Rect(widthInPx, heightInPx, 0.01782729805, 0.0434726087);
		}

		private void DeleteObjectFromMap(int y, int x)
		{
			this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[y, x].EntityBrush = null;
			this.model.Map[(int)this.model.ActualMapSegment.X, (int)this.model.ActualMapSegment.Y].Entities[y, x].IsCharacter = false;
		}

		private void EndGame()
		{
			this.EndGameEvent.Invoke(this, new EventArgs());
		}
	}
}
