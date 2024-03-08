
namespace FpMatchFrom
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button_Init = new System.Windows.Forms.Button();
            this.button_Enroll = new System.Windows.Forms.Button();
            this.label_狀態 = new System.Windows.Forms.Label();
            this.button_Abort = new System.Windows.Forms.Button();
            this.button_GetFrature = new System.Windows.Forms.Button();
            this.textBox_FeatureTemplate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_FeatureValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Match = new System.Windows.Forms.Button();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.label_Score = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_Init
            // 
            this.button_Init.Location = new System.Drawing.Point(35, 189);
            this.button_Init.Name = "button_Init";
            this.button_Init.Size = new System.Drawing.Size(131, 41);
            this.button_Init.TabIndex = 0;
            this.button_Init.Text = "Init";
            this.button_Init.UseVisualStyleBackColor = true;
            // 
            // button_Enroll
            // 
            this.button_Enroll.Location = new System.Drawing.Point(172, 189);
            this.button_Enroll.Name = "button_Enroll";
            this.button_Enroll.Size = new System.Drawing.Size(131, 41);
            this.button_Enroll.TabIndex = 1;
            this.button_Enroll.Text = "Enroll";
            this.button_Enroll.UseVisualStyleBackColor = true;
            // 
            // label_狀態
            // 
            this.label_狀態.BackColor = System.Drawing.Color.Green;
            this.label_狀態.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_狀態.ForeColor = System.Drawing.Color.White;
            this.label_狀態.Location = new System.Drawing.Point(31, 27);
            this.label_狀態.Name = "label_狀態";
            this.label_狀態.Size = new System.Drawing.Size(147, 58);
            this.label_狀態.TabIndex = 2;
            this.label_狀態.Text = "閒置中";
            this.label_狀態.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Abort
            // 
            this.button_Abort.Location = new System.Drawing.Point(309, 189);
            this.button_Abort.Name = "button_Abort";
            this.button_Abort.Size = new System.Drawing.Size(131, 41);
            this.button_Abort.TabIndex = 3;
            this.button_Abort.Text = "Abort";
            this.button_Abort.UseVisualStyleBackColor = true;
            // 
            // button_GetFrature
            // 
            this.button_GetFrature.Location = new System.Drawing.Point(446, 189);
            this.button_GetFrature.Name = "button_GetFrature";
            this.button_GetFrature.Size = new System.Drawing.Size(131, 41);
            this.button_GetFrature.TabIndex = 4;
            this.button_GetFrature.Text = "GetFrature";
            this.button_GetFrature.UseVisualStyleBackColor = true;
            // 
            // textBox_FeatureTemplate
            // 
            this.textBox_FeatureTemplate.Location = new System.Drawing.Point(35, 273);
            this.textBox_FeatureTemplate.Multiline = true;
            this.textBox_FeatureTemplate.Name = "textBox_FeatureTemplate";
            this.textBox_FeatureTemplate.Size = new System.Drawing.Size(470, 322);
            this.textBox_FeatureTemplate.TabIndex = 5;
            this.textBox_FeatureTemplate.Text = resources.GetString("textBox_FeatureTemplate.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 258);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "Template";
            // 
            // textBox_FeatureValue
            // 
            this.textBox_FeatureValue.Location = new System.Drawing.Point(511, 273);
            this.textBox_FeatureValue.Multiline = true;
            this.textBox_FeatureValue.Name = "textBox_FeatureValue";
            this.textBox_FeatureValue.Size = new System.Drawing.Size(470, 322);
            this.textBox_FeatureValue.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(509, 258);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Value";
            // 
            // button_Match
            // 
            this.button_Match.Location = new System.Drawing.Point(854, 189);
            this.button_Match.Name = "button_Match";
            this.button_Match.Size = new System.Drawing.Size(131, 41);
            this.button_Match.TabIndex = 9;
            this.button_Match.Text = "Match";
            this.button_Match.UseVisualStyleBackColor = true;
            // 
            // label_Score
            // 
            this.label_Score.BackColor = System.Drawing.Color.Green;
            this.label_Score.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Score.ForeColor = System.Drawing.Color.White;
            this.label_Score.Location = new System.Drawing.Point(846, 128);
            this.label_Score.Name = "label_Score";
            this.label_Score.Size = new System.Drawing.Size(147, 58);
            this.label_Score.TabIndex = 10;
            this.label_Score.Text = "------";
            this.label_Score.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 607);
            this.Controls.Add(this.label_Score);
            this.Controls.Add(this.button_Match);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_FeatureValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_FeatureTemplate);
            this.Controls.Add(this.button_GetFrature);
            this.Controls.Add(this.button_Abort);
            this.Controls.Add(this.label_狀態);
            this.Controls.Add(this.button_Enroll);
            this.Controls.Add(this.button_Init);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Init;
        private System.Windows.Forms.Button button_Enroll;
        private System.Windows.Forms.Label label_狀態;
        private System.Windows.Forms.Button button_Abort;
        private System.Windows.Forms.Button button_GetFrature;
        private System.Windows.Forms.TextBox textBox_FeatureTemplate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_FeatureValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Match;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label label_Score;
    }
}

