namespace Atlas_of_innovation
{
    partial class StartForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.ActiveForm = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // ActiveForm
            // 
            this.ActiveForm.BackColor = System.Drawing.SystemColors.Control;
            this.ActiveForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ActiveForm.Location = new System.Drawing.Point(0, 0);
            this.ActiveForm.Margin = new System.Windows.Forms.Padding(0);
            this.ActiveForm.Name = "ActiveForm";
            this.ActiveForm.Size = new System.Drawing.Size(932, 703);
            this.ActiveForm.TabIndex = 1;
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(932, 703);
            this.Controls.Add(this.ActiveForm);
            this.MinimumSize = new System.Drawing.Size(950, 750);
            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.StartForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel ActiveForm;
    }
}

