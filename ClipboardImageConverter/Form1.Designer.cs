namespace ClipboardImageConverter
{
    #nullable enable
    partial class Form1
    {
        private System.ComponentModel.IContainer? components;
        private Label statusValue = null!;
        private TextBox urlValue = null!;
        private TextBox resultValue = null!;
        private TextBox pathValue = null!;
        private TextBox destinationFolderValue = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopMonitoring();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            layout = new TableLayoutPanel();
            statusLabel = new Label();
            statusValue = new Label();
            urlLabel = new Label();
            urlValue = new TextBox();
            resultLabel = new Label();
            resultValue = new TextBox();
            pathLabel = new Label();
            pathValue = new TextBox();
            destinationLabel = new Label();
            destinationLayout = new TableLayoutPanel();
            destinationFolderValue = new TextBox();
            changeFolderButton = new Button();
            layout.SuspendLayout();
            destinationLayout.SuspendLayout();
            SuspendLayout();
            // 
            // layout
            // 
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            layout.Controls.Add(statusLabel, 0, 0);
            layout.Controls.Add(statusValue, 0, 1);
            layout.Controls.Add(urlLabel, 0, 2);
            layout.Controls.Add(urlValue, 0, 3);
            layout.Controls.Add(resultLabel, 0, 4);
            layout.Controls.Add(resultValue, 0, 5);
            layout.Controls.Add(pathLabel, 0, 6);
            layout.Controls.Add(pathValue, 0, 7);
            layout.Controls.Add(destinationLabel, 0, 8);
            layout.Controls.Add(destinationLayout, 0, 9);
            layout.Dock = DockStyle.Fill;
            layout.Location = new Point(0, 0);
            layout.Margin = new Padding(3, 4, 3, 4);
            layout.Name = "layout";
            layout.Padding = new Padding(18, 21, 18, 21);
            layout.RowCount = 10;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            layout.Size = new Size(823, 587);
            layout.TabIndex = 0;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(21, 21);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(179, 20);
            statusLabel.TabIndex = 0;
            statusLabel.Text = "Status do monitoramento";
            // 
            // statusValue
            // 
            statusValue.Dock = DockStyle.Fill;
            statusValue.Location = new Point(21, 53);
            statusValue.Name = "statusValue";
            statusValue.Size = new Size(781, 43);
            statusValue.TabIndex = 1;
            statusValue.Text = "Iniciando monitoramento...";
            // 
            // urlLabel
            // 
            urlLabel.AutoSize = true;
            urlLabel.Location = new Point(21, 96);
            urlLabel.Name = "urlLabel";
            urlLabel.Size = new Size(154, 20);
            urlLabel.TabIndex = 2;
            urlLabel.Text = "Última URL detectada";
            // 
            // urlValue
            // 
            urlValue.Dock = DockStyle.Fill;
            urlValue.Location = new Point(21, 132);
            urlValue.Margin = new Padding(3, 4, 3, 4);
            urlValue.Multiline = true;
            urlValue.Name = "urlValue";
            urlValue.ReadOnly = true;
            urlValue.ScrollBars = ScrollBars.Vertical;
            urlValue.Size = new Size(781, 94);
            urlValue.TabIndex = 3;
            // 
            // resultLabel
            // 
            resultLabel.AutoSize = true;
            resultLabel.Location = new Point(21, 230);
            resultLabel.Name = "resultLabel";
            resultLabel.Size = new Size(163, 20);
            resultLabel.TabIndex = 4;
            resultLabel.Text = "Resultado da operação";
            // 
            // resultValue
            // 
            resultValue.Dock = DockStyle.Fill;
            resultValue.Location = new Point(21, 266);
            resultValue.Margin = new Padding(3, 4, 3, 4);
            resultValue.Multiline = true;
            resultValue.Name = "resultValue";
            resultValue.ReadOnly = true;
            resultValue.ScrollBars = ScrollBars.Vertical;
            resultValue.Size = new Size(781, 94);
            resultValue.TabIndex = 5;
            resultValue.Text = "Aguardando uma URL de imagem.";
            // 
            // pathLabel
            // 
            pathLabel.AutoSize = true;
            pathLabel.Location = new Point(21, 364);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new Size(202, 20);
            pathLabel.TabIndex = 6;
            pathLabel.Text = "Caminho do último JPG salvo";
            // 
            // pathValue
            // 
            pathValue.Dock = DockStyle.Fill;
            pathValue.Location = new Point(21, 400);
            pathValue.Margin = new Padding(3, 4, 3, 4);
            pathValue.Multiline = true;
            pathValue.Name = "pathValue";
            pathValue.ReadOnly = true;
            pathValue.ScrollBars = ScrollBars.Vertical;
            pathValue.Size = new Size(781, 80);
            pathValue.TabIndex = 7;
            // 
            // destinationLabel
            // 
            destinationLabel.AutoSize = true;
            destinationLabel.Location = new Point(21, 484);
            destinationLabel.Name = "destinationLabel";
            destinationLabel.Size = new Size(204, 20);
            destinationLabel.TabIndex = 8;
            destinationLabel.Text = "Pasta de destino das imagens";
            // 
            // destinationLayout
            // 
            destinationLayout.ColumnCount = 2;
            destinationLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            destinationLayout.ColumnStyles.Add(new ColumnStyle());
            destinationLayout.Controls.Add(destinationFolderValue, 0, 0);
            destinationLayout.Controls.Add(changeFolderButton, 1, 0);
            destinationLayout.Dock = DockStyle.Fill;
            destinationLayout.Location = new Point(18, 516);
            destinationLayout.Margin = new Padding(0);
            destinationLayout.Name = "destinationLayout";
            destinationLayout.RowCount = 1;
            destinationLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            destinationLayout.Size = new Size(787, 50);
            destinationLayout.TabIndex = 9;
            // 
            // destinationFolderValue
            // 
            destinationFolderValue.Dock = DockStyle.Fill;
            destinationFolderValue.Location = new Point(3, 4);
            destinationFolderValue.Margin = new Padding(3, 4, 3, 4);
            destinationFolderValue.Name = "destinationFolderValue";
            destinationFolderValue.ReadOnly = true;
            destinationFolderValue.Size = new Size(646, 27);
            destinationFolderValue.TabIndex = 0;
            // 
            // changeFolderButton
            // 
            changeFolderButton.AutoSize = true;
            changeFolderButton.Dock = DockStyle.Fill;
            changeFolderButton.Location = new Point(655, 4);
            changeFolderButton.Margin = new Padding(3, 4, 3, 4);
            changeFolderButton.Name = "changeFolderButton";
            changeFolderButton.Size = new Size(129, 42);
            changeFolderButton.TabIndex = 1;
            changeFolderButton.Text = "Alterar pasta...";
            changeFolderButton.Click += OnChangeDestinationFolder;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(823, 587);
            Controls.Add(layout);
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(615, 518);
            Name = "Form1";
            Text = "ClipboardImageConverter";
            layout.ResumeLayout(false);
            layout.PerformLayout();
            destinationLayout.ResumeLayout(false);
            destinationLayout.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel layout;
        private Label statusLabel;
        private Label urlLabel;
        private Label resultLabel;
        private Label pathLabel;
        private Label destinationLabel;
        private TableLayoutPanel destinationLayout;
        private Button changeFolderButton;
    }
}
