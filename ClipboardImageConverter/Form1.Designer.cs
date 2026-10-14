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
            components = new System.ComponentModel.Container();
            var layout = new TableLayoutPanel();
            var statusLabel = new Label();
            var urlLabel = new Label();
            var resultLabel = new Label();
            var pathLabel = new Label();
            statusValue = new Label();
            urlValue = new TextBox();
            resultValue = new TextBox();
            pathValue = new TextBox();
            SuspendLayout();
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(16);
            layout.ColumnCount = 1;
            layout.RowCount = 8;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 35));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
            statusLabel.Text = "Status do monitoramento";
            urlLabel.Text = "Última URL detectada";
            resultLabel.Text = "Resultado da operação";
            pathLabel.Text = "Caminho do último JPG salvo";
            statusValue.Text = "Iniciando monitoramento...";
            resultValue.Text = "Aguardando uma URL de imagem.";
            statusLabel.AutoSize = true;
            urlLabel.AutoSize = true;
            resultLabel.AutoSize = true;
            pathLabel.AutoSize = true;
            statusValue.Dock = DockStyle.Fill;
            urlValue.Dock = DockStyle.Fill;
            resultValue.Dock = DockStyle.Fill;
            pathValue.Dock = DockStyle.Fill;
            urlValue.ReadOnly = true;
            resultValue.ReadOnly = true;
            pathValue.ReadOnly = true;
            urlValue.Multiline = true;
            resultValue.Multiline = true;
            pathValue.Multiline = true;
            urlValue.ScrollBars = ScrollBars.Vertical;
            resultValue.ScrollBars = ScrollBars.Vertical;
            pathValue.ScrollBars = ScrollBars.Vertical;
            layout.Controls.Add(statusLabel, 0, 0);
            layout.Controls.Add(statusValue, 0, 1);
            layout.Controls.Add(urlLabel, 0, 2);
            layout.Controls.Add(urlValue, 0, 3);
            layout.Controls.Add(resultLabel, 0, 4);
            layout.Controls.Add(resultValue, 0, 5);
            layout.Controls.Add(pathLabel, 0, 6);
            layout.Controls.Add(pathValue, 0, 7);
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(720, 380);
            MinimumSize = new Size(540, 340);
            Controls.Add(layout);
            Text = "ClipboardImageConverter";
            ResumeLayout(false);
        }
    }
}
