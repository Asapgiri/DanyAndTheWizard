// <copyright file="DatwControl.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DanyAndTheWizard
{
	using System;
	using System.Diagnostics;
	using System.Windows;
	using System.Windows.Controls;
	using System.Windows.Input;
	using System.Windows.Media;
	using System.Windows.Threading;
	using DanyAndTheWizard.Logic;
	using DanyAndTheWizard.Logic.Events;
	using DanyAndTheWizard.Logic.InternalClasses;
	using DanyAndTheWizard.ViewModels;

	/// <summary>
	/// The controller class of the game.
	/// </summary>
	public class DatwControl : FrameworkElement
	{
		private bool isPaused = false;
		private bool isLoadedOnce = false;
		private bool isSprinting = false;
		private bool isFightStarted = false;

		private DatwRenderer renderer;
		private DispatcherTimer mainTimer;
		private DispatcherTimer skinTimer;
		private Stopwatch stw;

		/// <summary>
		/// Initializes a new instance of the <see cref="DatwControl"/> class.
		/// </summary>
		public DatwControl()
		{
			this.Loaded += this.DatwControl_Loaded;
		}

		/// <summary>
		/// Invoke when the game is ended.
		/// </summary>
		public event EventHandler EscapeScreenEvent;

		private enum MoveDirection
		{
			Left,
			Right,
			Up,
			Down,
			LeftDown,
			LeftUp,
			RightDown,
			RightUp,
			None,
		}

		/// <summary>
		/// Gets or sets the mgames logic.
		/// </summary>
		public DatwLogic Logic { get; set; }

		/// <summary>
		/// Gets or sets the ganmes model.
		/// </summary>
		public DatwModel Model { get; set; }

		/// <summary>
		/// Gets or sets ticks for the enemys round.
		/// </summary>
		public DispatcherTimer EnemyAttackTimer { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the game is loaded from a savefile.
		/// </summary>
		public bool LoadFromSave { get; set; }

		/// <summary>
		/// Gets or sets the saves name.
		/// </summary>
		public string SaveName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether gets if the game is paused.
		/// </summary>
		public bool IsPaused { get => this.isPaused; set => this.isPaused = value; }

		private MoveDirection Direction { get; set; } = MoveDirection.None;

		/// <summary>
		/// Gets the maps Brush.
		/// </summary>
		/// <returns>a brush.</returns>
		public Brush GetMapBrush()
		{
			return this.Model.GetMap.MapBrush;
		}

		/// <summary>
		/// Rsumes the game after a greak or pause.
		/// </summary>
		public void Resume()
		{
			if (this.IsPaused)
			{
				this.stw.Start();
				this.mainTimer.Start();
				this.IsPaused = false;
			}
			else if (this.isFightStarted)
			{
				this.mainTimer.Start();
			}
		}

		/// <summary>
		/// Saves the game.
		/// </summary>
		/// <param name="saveName">The name of the save.</param>
		public void SaveGame(string saveName)
		{
			this.Logic.Save.NewSave(saveName);
		}

		/// <summary>
		/// Drops the user to the escape screen.
		/// </summary>
		public void EscapeScreen()
		{
			if (this.Model.Fight.IsInFight)
			{
				this.Logic.Fight.EndFight();
				this.Resume();
			}
			else if (!this.Model.IsInteractionRunning)
			{
				this.stw.Stop();
				this.mainTimer.Stop();
				this.IsPaused = true;
				this.EscapeScreenEvent.Invoke(this, new UserSwitchingEventArgs("InGameMenu"));
			}
			else
			{
				this.Model.IsInteractionRunning = false;
			}
		}

		/// <inheritdoc/>
		protected override void OnRender(DrawingContext drawingContext)
		{
			if (this.renderer != null)
			{
				drawingContext.DrawDrawing(this.renderer.GetDrawing());
			}
		}

		private void DatwControl_Loaded(object sender, RoutedEventArgs e)
		{
			if (!this.isLoadedOnce)
			{
				Window win = Window.GetWindow(this);
				this.stw = new Stopwatch();

				win.Width = Config.Width + 14.4;
				win.Height = Config.Height + 37.6;
				this.Model = new DatwModel(Config.Width, Config.Height)
				{
					ActualMapSegment = new Point(0, 0),
					BaseTime = default(TimeSpan),
				};
				this.Logic = new DatwLogic(this.Model);
				this.renderer = new DatwRenderer(this.Model);
				this.Logic.ModelChanged += this.renderer.Reset;
				this.Logic.Fight.FightEvent += (win.DataContext as GameWindow).FightEventDropped;
				this.Logic.EndGameEvent += this.Logic_EndGameEvent;
				((win as MainWindow).UcDic["InGameMenu"] as InGameMenu).Exit += this.Logic_EndGameEvent;

				if (win != null)
				{
					win.KeyDown += this.Win_KeyDown;
					win.KeyUp += this.Win_KeyUp;

					// MouseDown += DatwControl_MouseDown;
					this.mainTimer = new DispatcherTimer();
					this.mainTimer.Interval = TimeSpan.FromMilliseconds(1);
					this.mainTimer.Tick += this.MainTimer_Tick;

					this.skinTimer = new DispatcherTimer();
					this.skinTimer.Interval = TimeSpan.FromMilliseconds(100);
					this.skinTimer.Tick += this.SkinTimer_Tick;

					this.EnemyAttackTimer = new DispatcherTimer();
					this.EnemyAttackTimer.Interval = TimeSpan.FromSeconds(2);
					this.EnemyAttackTimer.Tick += this.EnemyAttackTimer_Tick;

					this.mainTimer.Start();
					this.stw.Start();
					this.isLoadedOnce = true;

					Window.GetWindow(this).SizeToContent = SizeToContent.Manual;
				}
			}

			this.Resume();
			this.InvalidateVisual();
		}

		private void EnemyAttackTimer_Tick(object sender, EventArgs e)
		{
			if (this.Model.Fight.CanEnemyAttack && this.Model.Fight.IsInFight && !this.Model.Fight.IsFightEnded)
			{
				this.Logic.EnemyRandomArrack();
				this.Model.Fight.CanEnemyAttack = false;
				if (!this.Model.Fight.IsFightEnded)
				{
					(Window.GetWindow(this).DataContext as GameWindow).FightActions.Visibility = Visibility.Visible;
					this.EnemyAttackTimer.Stop();
				}
			}
			else if (this.Model.Fight.IsFightEnded)
			{
				this.Logic.Fight.EndFight();
				this.EnemyAttackTimer.Stop();
			}

			this.renderer.Reset();
		}

		private void Logic_EndGameEvent(object sender, EventArgs e)
		{
			Window win;
			try
			{
				win = Window.GetWindow(sender as UserControl);
			}
			catch (Exception)
			{
				win = Window.GetWindow(this);
			}

			win.KeyDown -= this.Win_KeyDown;
			win.KeyUp -= this.Win_KeyUp;
			this.Logic.ModelChanged -= this.renderer.Reset;
			this.Logic.Fight.FightEvent -= ((win as MainWindow).UcDic["Game"] as GameWindow).FightEventDropped;
			this.Logic.EndGameEvent -= this.Logic_EndGameEvent;
			((win as MainWindow).UcDic["InGameMenu"] as InGameMenu).Exit -= this.Logic_EndGameEvent;
			this.stw.Stop();
			this.mainTimer.Stop();
			this.skinTimer.Stop();
			this.EnemyAttackTimer.Stop();
			this.EscapeScreenEvent.Invoke(this, new UserSwitchingEventArgs("ScoreSave"));
		}

		private void SkinTimer_Tick(object sender, EventArgs e)
		{
			if (this.Direction != MoveDirection.None)
			{
				switch (this.Model.CharacterTimer)
				{
					case 0:
						this.Model.CharacterTimer++;
						break;
					case 1:
						this.Model.CharacterTimer++;
						break;
					case 2:
						this.Model.CharacterTimer = 0;
						break;
					default:
						this.Model.CharacterTimer = 0;
						break;
				}
			}
			else
			{
				this.Model.CharacterTimer = 0;
			}
		}

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			this.Movement();
			if (this.isSprinting)
			{
				this.Movement();
			}

			this.CharacterDirection();

			this.Model.GameTime = this.Model.BaseTime + this.stw.Elapsed;
		}

		private void Win_KeyDown(object sender, KeyEventArgs e)
		{
			if (Keyboard.IsKeyDown(Config.MoveDown) && Keyboard.IsKeyDown(Config.MoveLeft))
			{
				this.Direction = MoveDirection.LeftDown;
			}
			else if (Keyboard.IsKeyDown(Config.MoveUp) && Keyboard.IsKeyDown(Config.MoveLeft))
			{
				this.Direction = MoveDirection.LeftUp;
			}
			else if (Keyboard.IsKeyDown(Config.MoveDown) && Keyboard.IsKeyDown(Config.MoveRight))
			{
				this.Direction = MoveDirection.RightDown;
			}
			else if (Keyboard.IsKeyDown(Config.MoveUp) && Keyboard.IsKeyDown(Config.MoveRight))
			{
				this.Direction = MoveDirection.RightUp;
			}
			else if (Keyboard.IsKeyDown(Config.MoveDown))
			{
				this.Direction = MoveDirection.Down;
			}
			else if (Keyboard.IsKeyDown(Config.MoveUp))
			{
				this.Direction = MoveDirection.Up;
			}
			else if (Keyboard.IsKeyDown(Config.MoveLeft))
			{
				this.Direction = MoveDirection.Left;
			}
			else if (Keyboard.IsKeyDown(Config.MoveRight))
			{
				this.Direction = MoveDirection.Right;
			}
			else if (Keyboard.IsKeyDown(Config.Inventory))
			{
				if (!this.Model.IsInventoryOpen)
				{
					this.InventoryOpen();
				}
				else
				{
					this.InventoryClose();
				}
			}
			else if (Keyboard.IsKeyDown(Key.Space))
			{
				this.Dialog();
			}
			else if (Keyboard.IsKeyDown(Key.M))
			{
				if (this.Model.Minimap.Visible)
				{
					this.Model.Minimap.Visible = false;
				}
				else
				{
					this.Model.Minimap.Visible = true;
				}
			}
			else if (Keyboard.IsKeyDown(Config.Escape))
			{
				if (this.IsPaused)
				{
					this.EscapeScreenEvent.Invoke(this, new UserSwitchingEventArgs("Game"));
					this.Resume();
				}
				else if (this.Model.IsInventoryOpen)
				{
					this.InventoryClose();
				}
				else
				{
					this.EscapeScreen();
				}
			}

			if (!this.isSprinting && Keyboard.IsKeyDown(Key.LeftShift))
			{
				this.isSprinting = true;
			}
		}

		private void Dialog()
		{
			if (this.Model.IsInteractionRunning)
			{
				if (this.Model.Dialog.ActualDialogQueue != null && this.Model.Dialog.ActualDialogQueue.Count != 0)
				{
					this.Model.Dialog.ActualDialog = $"[{this.Model.Dialog.ActualSpeeker}]:\n{this.Model.Dialog.ActualDialogQueue.Dequeue()}";
				}
				else
				{
					this.Model.IsInteractionRunning = false;
					this.Logic.CharacterInteraction();
					if (this.Model.Dialog.ActualDialogQueue != null && this.Model.Dialog.ActualDialogQueue.Count != 0)
					{
						this.Model.Dialog.ActualDialog = $"[{this.Model.Dialog.ActualSpeeker}]:\n{this.Model.Dialog.ActualDialogQueue.Dequeue()}";
					}

					// if (!this.model.Fight.IsInFight)
					// {
					// (Window.GetWindow(this).DataContext as GameWindow).FightActions.Visibility = Visibility.Hidden;
					// this.isFightStarted = false;
					// }
				}
			}
		}

		private void InventoryOpen()
		{
			if ((Window.GetWindow(this).DataContext as GameWindow).Inventory != null)
			{
				StackPanel inventory = (Window.GetWindow(this).DataContext as GameWindow).Inventory;
				inventory.Children.Clear();
				(Window.GetWindow(this).DataContext as GameWindow).SCViewer.Visibility = Visibility.Visible;
				foreach (GameItem item in this.Model.GetPlayer.Inventory)
				{
					Grid grid = new Grid();
					ColumnDefinition col = new ColumnDefinition();
					col.Width = new GridLength(90);
					ColumnDefinition col2 = new ColumnDefinition();
					ColumnDefinition col3 = new ColumnDefinition();
					col3.Width = new GridLength(120);

					grid.ColumnDefinitions.Add(col);
					grid.ColumnDefinitions.Add(col2);
					grid.ColumnDefinitions.Add(col3);

					Image img = new Image();
					img.Source = item.Image;
					img.Margin = new Thickness(5);
					grid.Children.Add(img);

					TextBlock tb = new TextBlock();
					tb.Text = item.ToString();
					tb.FontSize = 18;
					tb.FontWeight = FontWeights.Bold;
					tb.Margin = new Thickness(10, 0, 5, 0);
					Grid.SetColumn(tb, 1);
					grid.Children.Add(tb);

					Button btn = new Button();
					if (item.Type == ItemType.Neutral)
					{
						btn.Content = "Use";
						btn.Click += (sender, e) =>
						{
							this.Logic.UseItem(item);
							this.InventoryOpen();
						};
					}
					else if (item.IsEquipped)
					{
						btn.Content = "Unequip";
						btn.Click += (sender, e) =>
						{
							this.Logic.UnEquipItem(item);
							this.InventoryOpen();
						};
						grid.Background = Brushes.Red;
					}
					else
					{
						btn.Content = "Equip";
						btn.Click += (sender, e) =>
						{
							this.Logic.EquipItem(item);
							this.InventoryOpen();
						};
					}

					btn.Margin = new Thickness(5);
					Grid.SetColumn(btn, 2);
					grid.Children.Add(btn);

					grid.Margin = new Thickness(10);
					inventory.Children.Add(grid);
				}

				this.Model.IsInventoryOpen = true;
			}
		}

		private void InventoryClose()
		{
			(Window.GetWindow(this).DataContext as GameWindow).SCViewer.Visibility = Visibility.Hidden;
			this.Model.IsInventoryOpen = false;
		}

		private void Win_KeyUp(object sender, KeyEventArgs e)
		{
			if (Keyboard.IsKeyDown(Config.MoveDown))
			{
				this.Direction = MoveDirection.Down;
			}
			else if (Keyboard.IsKeyDown(Config.MoveUp))
			{
				this.Direction = MoveDirection.Up;
			}
			else if (Keyboard.IsKeyDown(Config.MoveLeft))
			{
				this.Direction = MoveDirection.Left;
			}
			else if (Keyboard.IsKeyDown(Config.MoveRight))
			{
				this.Direction = MoveDirection.Right;
			}
			else
			{
				this.Direction = MoveDirection.None;
			}

			if (this.isSprinting && !Keyboard.IsKeyDown(Key.LeftShift))
			{
				this.isSprinting = false;
			}
		}

		private void Movement()
		{
			switch (this.Direction)
			{
				case MoveDirection.Left:
					this.Logic.Move(-Config.MovementSpeed, 0);

					break;
				case MoveDirection.Right:
					this.Logic.Move(Config.MovementSpeed, 0);
					break;
				case MoveDirection.Up:
					this.Logic.Move(0, -Config.MovementSpeed);
					break;
				case MoveDirection.Down:
					this.Logic.Move(0, Config.MovementSpeed);
					break;
				case MoveDirection.LeftDown:
					if (!this.Logic.Move(-Config.MovementSpeed, Config.MovementSpeed))
					{
						if (!this.Logic.Move(-Config.MovementSpeed / 2, 0))
						{
							this.Logic.Move(-0, Config.MovementSpeed / 2);
						}
					}

					break;
				case MoveDirection.LeftUp:
					if (!this.Logic.Move(-Config.MovementSpeed, -Config.MovementSpeed))
					{
						if (!this.Logic.Move(-Config.MovementSpeed / 2, 0))
						{
							this.Logic.Move(0, -Config.MovementSpeed / 2);
						}
					}

					break;
				case MoveDirection.RightDown:
					if (!this.Logic.Move(Config.MovementSpeed, Config.MovementSpeed))
					{
						if (!this.Logic.Move(Config.MovementSpeed / 2, 0))
						{
							this.Logic.Move(0, Config.MovementSpeed / 2);
						}
					}

					break;
				case MoveDirection.RightUp:
					if (!this.Logic.Move(Config.MovementSpeed, -Config.MovementSpeed))
					{
						if (!this.Logic.Move(Config.MovementSpeed / 2, 0))
						{
							this.Logic.Move(0, -Config.MovementSpeed / 2);
						}
					}

					break;
			}

			this.InvalidateVisual();
		}

		private void CharacterDirection()
		{
			switch (this.Direction)
			{
				case MoveDirection.Left:
				case MoveDirection.LeftDown:
				case MoveDirection.LeftUp:
					this.Logic.CharacterDirection(DatwLogic.Direction.Left);
					this.skinTimer.Start();
					break;
				case MoveDirection.Right:
				case MoveDirection.RightDown:
				case MoveDirection.RightUp:
					this.Logic.CharacterDirection(DatwLogic.Direction.Right);
					this.skinTimer.Start();
					break;
				case MoveDirection.Up:
					this.Logic.CharacterDirection(DatwLogic.Direction.Up);
					this.skinTimer.Start();
					break;
				case MoveDirection.Down:
					this.Logic.CharacterDirection(DatwLogic.Direction.Down);
					this.skinTimer.Start();
					break;
				case MoveDirection.None:
					this.skinTimer.Stop();
					this.Model.CharacterTimer = 0;
					break;
				default:
					break;
			}
		}
	}
}
