using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersLogic;

namespace WindowsUI
{
    public class FormSettings : Form
    {
        private Label boradSize;
        private Label playersLabel;
        private Label playerOne;
        private CheckBox playerType;
        private RadioButton small;
        private RadioButton medium;
        private TextBox playerOneName;
        private TextBox playerTwoName;
        private Button doneButton;
        private RadioButton large;

        public Button DoneButton
        {
            get
            {
                return doneButton;
            }
        }

        public eBoardSizeOption BoardSize
        {
            get
            {
                if (small.Checked)
                {
                    return eBoardSizeOption.Small;
                }
                else if (medium.Checked)
                {
                    return eBoardSizeOption.Medium;
                }

                return eBoardSizeOption.Large;
            }
        }

        public ePlayerType PlayerType
        {
            get
            {
                return playerType.Checked ? ePlayerType.User : ePlayerType.Computer;
            }
        }

        public string PlayerOneName
        {
            get
            {
                return playerOneName.Text;
            }
        }

        public string PlayerTwoName
        {
            get
            {
                return playerTwoName.Text;
            }
        }

        public FormSettings()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.boradSize = new System.Windows.Forms.Label();
            this.playersLabel = new System.Windows.Forms.Label();
            this.playerOne = new System.Windows.Forms.Label();
            this.playerType = new System.Windows.Forms.CheckBox();
            this.small = new System.Windows.Forms.RadioButton();
            this.medium = new System.Windows.Forms.RadioButton();
            this.large = new System.Windows.Forms.RadioButton();
            this.playerOneName = new System.Windows.Forms.TextBox();
            this.playerTwoName = new System.Windows.Forms.TextBox();
            this.doneButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // boradSize
            // 
            this.boradSize.AutoSize = true;
            this.boradSize.Location = new System.Drawing.Point(33, 37);
            this.boradSize.Name = "boradSize";
            this.boradSize.Size = new System.Drawing.Size(91, 20);
            this.boradSize.TabIndex = 0;
            this.boradSize.Text = "Borad Size:";
            this.boradSize.Click += new System.EventHandler(this.label1_Click);
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.Location = new System.Drawing.Point(36, 193);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(64, 20);
            this.playersLabel.TabIndex = 1;
            this.playersLabel.Text = "Players:";
            this.playersLabel.Click += new System.EventHandler(this.playersLabel_Click);
            // 
            // playerOne
            // 
            this.playerOne.AutoSize = true;
            this.playerOne.Location = new System.Drawing.Point(74, 245);
            this.playerOne.Name = "playerOne";
            this.playerOne.Size = new System.Drawing.Size(69, 20);
            this.playerOne.TabIndex = 2;
            this.playerOne.Text = "Player 1:";
            // 
            // playerType
            // 
            this.playerType.AutoSize = true;
            this.playerType.Location = new System.Drawing.Point(78, 289);
            this.playerType.Name = "playerType";
            this.playerType.Size = new System.Drawing.Size(95, 24);
            this.playerType.TabIndex = 3;
            this.playerType.Text = "Player 2:";
            this.playerType.UseVisualStyleBackColor = true;
            this.playerType.CheckedChanged += new System.EventHandler(this.playerType_Checked);
            // 
            // small
            // 
            this.small.AutoSize = true;
            this.small.Checked = true;
            this.small.Location = new System.Drawing.Point(58, 78);
            this.small.Name = "small";
            this.small.Size = new System.Drawing.Size(67, 24);
            this.small.TabIndex = 4;
            this.small.TabStop = true;
            this.small.Text = "6 x 6";
            this.small.UseVisualStyleBackColor = true;
            // 
            // medium
            // 
            this.medium.AutoSize = true;
            this.medium.Location = new System.Drawing.Point(166, 78);
            this.medium.Name = "medium";
            this.medium.Size = new System.Drawing.Size(67, 24);
            this.medium.TabIndex = 5;
            this.medium.TabStop = true;
            this.medium.Text = "8 x 8";
            this.medium.UseVisualStyleBackColor = true;
            // 
            // large
            // 
            this.large.AutoSize = true;
            this.large.Location = new System.Drawing.Point(268, 78);
            this.large.Name = "large";
            this.large.Size = new System.Drawing.Size(85, 24);
            this.large.TabIndex = 6;
            this.large.TabStop = true;
            this.large.Text = "10 x 10";
            this.large.UseVisualStyleBackColor = true;
            // 
            // playerOneName
            // 
            this.playerOneName.Location = new System.Drawing.Point(239, 239);
            this.playerOneName.MaxLength = 20;
            this.playerOneName.Name = "playerOneName";
            this.playerOneName.Size = new System.Drawing.Size(113, 26);
            this.playerOneName.TabIndex = 7;
            // 
            // playerTwoName
            // 
            this.playerTwoName.Enabled = false;
            this.playerTwoName.Location = new System.Drawing.Point(239, 287);
            this.playerTwoName.MaxLength = 20;
            this.playerTwoName.Name = "playerTwoName";
            this.playerTwoName.Size = new System.Drawing.Size(111, 26);
            this.playerTwoName.TabIndex = 8;
            this.playerTwoName.Text = "[Computer]";
            // 
            // doneButton
            // 
            this.doneButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.doneButton.Location = new System.Drawing.Point(267, 348);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(83, 29);
            this.doneButton.TabIndex = 9;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.buttonDone_Click);
            // 
            // FormSettings
            // 
            this.ClientSize = new System.Drawing.Size(412, 433);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.playerTwoName);
            this.Controls.Add(this.playerOneName);
            this.Controls.Add(this.large);
            this.Controls.Add(this.medium);
            this.Controls.Add(this.small);
            this.Controls.Add(this.playerType);
            this.Controls.Add(this.playerOne);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.boradSize);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.Text = "Game Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void playersLabel_Click(object sender, EventArgs e)
        {
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PlayerOneName) || string.IsNullOrEmpty(PlayerTwoName))
            {
                MessageBox.Show("Players names cannot be empty");
                this.DialogResult = DialogResult.None;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void playerType_Checked(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            if(checkBox.Checked)
            {
                playerTwoName.Enabled = true;
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
        }
    }
}
