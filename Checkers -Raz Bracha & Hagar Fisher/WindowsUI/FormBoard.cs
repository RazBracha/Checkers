using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersLogic;

namespace WindowsUI
{
    public class FormBoard : Form
    {
        private const int m_ButtonSize = 50;
        private const int m_MarginTop = 70;
        private const int m_MarginLeft = 10;
        public readonly Button[,] r_Buttons;
        private Label player1Label;
        private Label player2Label;
        private Label player1Score;
        private Label player2Score;

        public delegate void BoardButtonClickedEventHandler(object button, EventArgs eventArgs);

        public event BoardButtonClickedEventHandler BoardButtonClicked;

        public string Player1Score
        {
            get
            {
                return player1Score.Text;
            }

            set
            {
                player1Score.Text = value;
            }
        }

        public string Player2Score
        {
            get
            {
                return player2Score.Text;
            }

            set
            {
                player2Score.Text = value;
            }
        }

        public string Player1Label
        {
            get
            {
                return player1Label.Text;
            }

            set
            {
                player1Label.Text = value;
            }
        }

        public string Player2Label
        {
            get
            {
                return player2Label.Text;
            }

            set
            {
                player2Label.Text = value;
            }
        }

        private void InitializeComponent()
        {
            this.player1Label = new System.Windows.Forms.Label();
            this.player2Label = new System.Windows.Forms.Label();
            this.player1Score = new System.Windows.Forms.Label();
            this.player2Score = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // player1Label
            // 
            this.player1Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.player1Label.AutoSize = true;
            this.player1Label.Location = new System.Drawing.Point(18, 13);
            this.player1Label.Name = "player1Label";
            this.player1Label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.player1Label.Size = new System.Drawing.Size(69, 20);
            this.player1Label.TabIndex = 0;
            this.player1Label.Text = "Player 1:";
            this.player1Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.player1Label.Click += new System.EventHandler(this.label1_Click);
            // 
            // player2Label
            // 
            this.player2Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.player2Label.AutoSize = true;
            this.player2Label.Location = new System.Drawing.Point(158, 13);
            this.player2Label.Name = "player2Label";
            this.player2Label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.player2Label.Size = new System.Drawing.Size(69, 20);
            this.player2Label.TabIndex = 1;
            this.player2Label.Text = "Player 2:";
            this.player2Label.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // player1Score
            // 
            this.player1Score.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.player1Score.AutoSize = true;
            this.player1Score.Location = new System.Drawing.Point(94, 13);
            this.player1Score.Name = "player1Score";
            this.player1Score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.player1Score.Size = new System.Drawing.Size(18, 20);
            this.player1Score.TabIndex = 2;
            this.player1Score.Text = "0";
            this.player1Score.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // player2Score
            // 
            this.player2Score.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.player2Score.AutoSize = true;
            this.player2Score.Location = new System.Drawing.Point(234, 12);
            this.player2Score.Name = "player2Score";
            this.player2Score.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.player2Score.Size = new System.Drawing.Size(18, 20);
            this.player2Score.TabIndex = 3;
            this.player2Score.Text = "0";
            this.player2Score.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.player2Score.Click += new System.EventHandler(this.Player2Score_Click);
            // 
            // FormBoard
            // 
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(275, 241);
            this.Controls.Add(this.player2Score);
            this.Controls.Add(this.player1Score);
            this.Controls.Add(this.player2Label);
            this.Controls.Add(this.player1Label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Name = "FormBoard";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 6, 6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Damka";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public FormBoard(Board i_Board)
        {
            int boardSize = (int)i_Board.BoardSize;
            r_Buttons = new Button[boardSize, boardSize];

            InitializeComponent();
            initializeBoardButtons(i_Board);
        }

        private void initializeBoardButtons(Board i_Board)
        {
            int boardSize = (int)i_Board.BoardSize;
            Button boardButton;
            Image blackPawn = Properties.Resources.BlackPawn;
            Image whitePawn = Properties.Resources.WhitePawn;
            Pawn pawn;

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    pawn = i_Board.BoardMatrix[i, j];
                    boardButton = new Button();
                    boardButton.Height = m_ButtonSize;
                    boardButton.Width = m_ButtonSize;
                    boardButton.BackColor = Color.White;
                    boardButton.Tag = pawn;
                    boardButton.Location = new Point((j * m_ButtonSize) + m_MarginLeft, (i * m_ButtonSize) + m_MarginTop);

                    setAlternatingButtonEnableProperty(boardButton, i, j);
                    boardButton.Click += this.BoardButton_Click;

                    if (pawn.Shape != eShapes.Empty)
                    {
                        boardButton.BackgroundImage = pawn.Shape == eShapes.PlayerO ? whitePawn : blackPawn;
                    }

                    boardButton.BackgroundImageLayout = ImageLayout.Stretch;
                    r_Buttons[i, j] = boardButton;
                    Controls.Add(boardButton);
                }
            }
        }

        private void setAlternatingButtonEnableProperty(Button i_Button, int i_Row, int i_Column)
        {
            if ((i_Row + i_Column) % 2 == 0)
            {
                i_Button.Enabled = false;
                i_Button.BackColor = Color.DarkGray;
            }
            else
            {
                i_Button.Enabled = true;
            }
        }

        private void BoardButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            Color buttonCurrentBackColor = clickedButton.BackColor;

            if (buttonCurrentBackColor == Color.SkyBlue)
            {
                clickedButton.BackColor = Color.White;
            }
            else if (buttonCurrentBackColor == Color.White)
            {
                clickedButton.BackColor = Color.SkyBlue;
            }

            BoardButtonClicked?.Invoke(sender, EventArgs.Empty);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void Player2Score_Click(object sender, EventArgs e)
        {
        }
    }
}
