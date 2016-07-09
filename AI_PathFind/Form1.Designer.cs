namespace AI_PathFind
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panel = new System.Windows.Forms.Panel();
            this.mouseCoordLabel = new System.Windows.Forms.Label();
            this.startTB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.endTB = new System.Windows.Forms.TextBox();
            this.pfkButt = new System.Windows.Forms.Button();
            this.sDistanceNumber = new System.Windows.Forms.Label();
            this.sDistanceLabel1 = new System.Windows.Forms.Label();
            this.foundInLabel = new System.Windows.Forms.Label();
            this.randCoordButt = new System.Windows.Forms.Button();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.pfkTimer = new System.Windows.Forms.Timer(this.components);
            this.aboutLabel = new System.Windows.Forms.Label();
            this.replyLabel = new System.Windows.Forms.Label();
            this.replyTimer = new System.Windows.Forms.Timer(this.components);
            this.rndObButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Location = new System.Drawing.Point(13, 13);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(600, 600);
            this.panel.TabIndex = 0;
            this.panel.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Paint);
            this.panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_MouseClick);
            this.panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel_MouseMove);
            // 
            // mouseCoordLabel
            // 
            this.mouseCoordLabel.AutoSize = true;
            this.mouseCoordLabel.Location = new System.Drawing.Point(619, 13);
            this.mouseCoordLabel.Name = "mouseCoordLabel";
            this.mouseCoordLabel.Size = new System.Drawing.Size(92, 13);
            this.mouseCoordLabel.TabIndex = 1;
            this.mouseCoordLabel.Text = "mouseCoordLabel";
            // 
            // startTB
            // 
            this.startTB.Location = new System.Drawing.Point(619, 51);
            this.startTB.Name = "startTB";
            this.startTB.Size = new System.Drawing.Size(100, 20);
            this.startTB.TabIndex = 2;
            this.startTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.startTB_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(619, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Start at (X,Y)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(619, 77);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Enter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.StartButt);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(758, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Enter";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.EndButt);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(758, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "End at (X,Y)";
            // 
            // endTB
            // 
            this.endTB.Location = new System.Drawing.Point(758, 51);
            this.endTB.Name = "endTB";
            this.endTB.Size = new System.Drawing.Size(100, 20);
            this.endTB.TabIndex = 5;
            this.endTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.endTB_Enter);
            // 
            // pfkButt
            // 
            this.pfkButt.Location = new System.Drawing.Point(619, 189);
            this.pfkButt.Name = "pfkButt";
            this.pfkButt.Size = new System.Drawing.Size(239, 23);
            this.pfkButt.TabIndex = 8;
            this.pfkButt.Text = "Find Path";
            this.pfkButt.UseVisualStyleBackColor = true;
            this.pfkButt.Click += new System.EventHandler(this.pfkButt_Click);
            // 
            // sDistanceNumber
            // 
            this.sDistanceNumber.Location = new System.Drawing.Point(619, 239);
            this.sDistanceNumber.Name = "sDistanceNumber";
            this.sDistanceNumber.Size = new System.Drawing.Size(100, 13);
            this.sDistanceNumber.TabIndex = 10;
            this.sDistanceNumber.Text = "sDistanceLabel";
            // 
            // sDistanceLabel1
            // 
            this.sDistanceLabel1.Location = new System.Drawing.Point(619, 226);
            this.sDistanceLabel1.Name = "sDistanceLabel1";
            this.sDistanceLabel1.Size = new System.Drawing.Size(100, 13);
            this.sDistanceLabel1.TabIndex = 11;
            this.sDistanceLabel1.Text = "Straight Distance:";
            // 
            // foundInLabel
            // 
            this.foundInLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.foundInLabel.Location = new System.Drawing.Point(758, 226);
            this.foundInLabel.Name = "foundInLabel";
            this.foundInLabel.Size = new System.Drawing.Size(100, 26);
            this.foundInLabel.TabIndex = 12;
            this.foundInLabel.Text = "Found in:";
            this.foundInLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // randCoordButt
            // 
            this.randCoordButt.Location = new System.Drawing.Point(619, 131);
            this.randCoordButt.Name = "randCoordButt";
            this.randCoordButt.Size = new System.Drawing.Size(239, 23);
            this.randCoordButt.TabIndex = 13;
            this.randCoordButt.Text = "Random Start/End";
            this.randCoordButt.UseVisualStyleBackColor = true;
            this.randCoordButt.Click += new System.EventHandler(this.randCoordButt_Click);
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.Location = new System.Drawing.Point(619, 343);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(157, 91);
            this.instructionsLabel.TabIndex = 14;
            this.instructionsLabel.Text = "Start = Blue\nEnd = Green\nClick a tile to place an obstacle.\nClick an obstacle to " +
    "remove it.\nEnter start and end points\nin X,Y format.\nExample: 3,5";
            // 
            // pfkTimer
            // 
            this.pfkTimer.Tick += new System.EventHandler(this.pfkTimer_Tick);
            // 
            // aboutLabel
            // 
            this.aboutLabel.AutoSize = true;
            this.aboutLabel.Location = new System.Drawing.Point(619, 519);
            this.aboutLabel.Name = "aboutLabel";
            this.aboutLabel.Size = new System.Drawing.Size(232, 78);
            this.aboutLabel.TabIndex = 15;
            this.aboutLabel.Text = resources.GetString("aboutLabel.Text");
            // 
            // replyLabel
            // 
            this.replyLabel.Location = new System.Drawing.Point(619, 103);
            this.replyLabel.Name = "replyLabel";
            this.replyLabel.Size = new System.Drawing.Size(239, 13);
            this.replyLabel.TabIndex = 16;
            this.replyLabel.Text = "replyLabel";
            // 
            // replyTimer
            // 
            this.replyTimer.Tick += new System.EventHandler(this.replyTimer_Tick);
            // 
            // rndObButt
            // 
            this.rndObButt.Location = new System.Drawing.Point(619, 160);
            this.rndObButt.Name = "rndObButt";
            this.rndObButt.Size = new System.Drawing.Size(239, 23);
            this.rndObButt.TabIndex = 17;
            this.rndObButt.Text = "Random Obstacles";
            this.rndObButt.UseVisualStyleBackColor = true;
            this.rndObButt.Click += new System.EventHandler(this.rndObButt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 619);
            this.Controls.Add(this.rndObButt);
            this.Controls.Add(this.replyLabel);
            this.Controls.Add(this.aboutLabel);
            this.Controls.Add(this.instructionsLabel);
            this.Controls.Add(this.randCoordButt);
            this.Controls.Add(this.foundInLabel);
            this.Controls.Add(this.sDistanceLabel1);
            this.Controls.Add(this.sDistanceNumber);
            this.Controls.Add(this.pfkButt);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.endTB);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startTB);
            this.Controls.Add(this.mouseCoordLabel);
            this.Controls.Add(this.panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label mouseCoordLabel;
        private System.Windows.Forms.TextBox startTB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox endTB;
        private System.Windows.Forms.Button pfkButt;
        private System.Windows.Forms.Label sDistanceNumber;
        private System.Windows.Forms.Label sDistanceLabel1;
        private System.Windows.Forms.Label foundInLabel;
        private System.Windows.Forms.Button randCoordButt;
        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Timer pfkTimer;
        private System.Windows.Forms.Label aboutLabel;
        private System.Windows.Forms.Label replyLabel;
        private System.Windows.Forms.Timer replyTimer;
        private System.Windows.Forms.Button rndObButt;
    }
}

