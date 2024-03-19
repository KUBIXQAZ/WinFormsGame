using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace mdklasa
{
	public partial class MainForm : Form
	{
		public void GenerateArea() {			
			for(int i = 0; i <= 450; i+=50) {
				var label = new Label() {
					BackColor = Color.Black,
					Width = 50,
					Height =  50
				};
				Controls.Add(label);
				label.Location = new Point(0,i);
			}
			
			for(int i = 0; i <= 450; i+=50) {
				var label = new Label() {
					BackColor = Color.Black,
					Width = 50,
					Height =  50
				};
				Controls.Add(label);
				label.Location = new Point(450,i);
			}
			
			for(int i = 0; i <= 450; i+=50) {
				var label = new Label() {
					BackColor = Color.Black,
					Width = 50,
					Height =  50
				};
				Controls.Add(label);
				label.Location = new Point(i,0);
			}
			
			for(int i = 0; i <= 450; i+=50) {
				var label = new Label() {
					BackColor = Color.Black,
					Width = 50,
					Height =  50
				};
				Controls.Add(label);
				label.Location = new Point(i,450);
			}
		}
		
		public void GeneratePlayer() {
			var label = new Player() {
				Width = 50,
				Height = 50,
				BackColor = Color.Blue,
				ForeColor = Color.White,
				TextAlign = ContentAlignment.MiddleCenter,
			};

			label.canMove = true;
			
			Controls.Add(label);
			label.Location = playerPosition;
			
			player = label;
		}
		
		public void GeneratePoint() {
			var label = new Coin() {
				Width = 50,
				Height = 50,
				BackColor = Color.Red
			};
			
			label.MouseClick += CheckForPickUpEvent;
			
			Controls.Add(label);
			
			Random random = new Random();			
			Point location = playerPosition;
			
			while (location == playerPosition) {
				location.X = random.Next(1,9) * 50;
				location.Y = random.Next(1,9) * 50;
			}
			
			label.Location = location;
			
			coins.Add(label);
		}
		
		public bool CheckMove(Point point) {
			if(point.X < 50 || point.X > 400 || point.Y < 50 || point.Y > 400 || player.canMove == false) return false;
			else return true;
		}
		
		public void MovePlayer() {
			//spawn block labels//
			/*
			var label = new Label() {
				Width = 50,
				Height = 50,
				BackColor = Color.DarkGray
			};
			
			Controls.Add(label);
			label.Location = playerPosition;
			*/
			//end//
			
			player.Location = playerPosition;
			CheckForPickUp();
		}
		
		public void UpdatePoints() {
			this.Text = player.points.ToString();
		}
		
		public void CheckForPickUpEvent(object sender, EventArgs e) {
			player.Visible = false;
			
			Coin PickedCoint = (Coin)sender;
			
			player.points++;
			Controls.Remove(PickedCoint);
			coins.Remove(PickedCoint);
			GeneratePoint();
			UpdatePoints();
		}
		
		public void CheckForPickUp() {
			bool PickUp = false;
			Coin PickedCoint = null;
			foreach (var coin in coins) {
				if(player.Location == coin.Location) {
					PickUp = true;
					PickedCoint = coin;
					break;
				}
			}
			if(PickUp == true) {
				player.points++;
				Controls.Remove(PickedCoint);
				coins.Remove(PickedCoint);
				GeneratePoint();
				UpdatePoints();
			}
		}
		
		public class Player : Label {
			public int points = 0;
			public bool canMove = false;
		}
		public class Coin : Label {}
		
		Point playerPosition = new Point(200,200);
		Player player;
		
		List<Coin> coins = new List<Coin>();

        static Timer timer = new Timer();
        int secondsLeft = 30;

        public MainForm()
		{
			InitializeComponent();

			StartGame();
        }

		public void ClearGUI()
		{
			Controls.Clear();
		}

		public void EndGameGUI()
		{
            ClearGUI();

            var button = new Button();
			button.Text = "Zagraj ponownie.";
			button.Width = 100;
			button.Height = 50;
			button.Click += (object s, EventArgs e) =>
			{
				ClearGUI();
				StartGame();
			};
			button.Location = new Point(200, 200);
            Controls.Add(button);
		}

		public void StartGame()
		{
			MessageBox.Show("W tej grze musisz zebrać jak najwięcej punktów w 30 sekund. możesz poruszać się za pomocą W,A,S,D albo klikając na czerwony punkt myszką, u góry jest napisana ilość punktów a na graczu (niebieski kwadrat) wyświetla się pozostały czas.", "Game");

            GenerateArea();
            GeneratePlayer();
            UpdatePoints();

            GeneratePoint();

            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            player.Text = secondsLeft.ToString();

            secondsLeft--;

            if (secondsLeft == 0)
            {
                timer.Stop();
				player.canMove = false;

                MessageBox.Show("Czas ubiegł końcu zdobyłeś " + player.points + " punktów!", "Game");
				EndGameGUI();
            }
        }

        void MainFormKeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.W || e.KeyCode == Keys.S || e.KeyCode == Keys.A || e.KeyCode == Keys.D) {
				player.Visible = true;
			}
			
			if(e.KeyCode == Keys.W) {
				if(CheckMove(new Point(playerPosition.X, playerPosition.Y - 50))) {
					playerPosition = new Point(playerPosition.X, playerPosition.Y - 50);
					MovePlayer();
				}
			} 
			if(e.KeyCode == Keys.S) {
				if(CheckMove(new Point(playerPosition.X, playerPosition.Y + 50))) {
					playerPosition = new Point(playerPosition.X, playerPosition.Y + 50);
					MovePlayer();
				}
			}
			if(e.KeyCode == Keys.A) {
				if(CheckMove(new Point(playerPosition.X - 50, playerPosition.Y))) {
					playerPosition = new Point(playerPosition.X - 50, playerPosition.Y);
					MovePlayer();
				}
			}
			if(e.KeyCode == Keys.D) {
				if(CheckMove(new Point(playerPosition.X + 50, playerPosition.Y))) {
					playerPosition = new Point(playerPosition.X + 50, playerPosition.Y);
					MovePlayer();
				}
			}
		}
	}
}
