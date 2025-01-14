using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.Json;

namespace FolderWatcher
{
    public class MainForm : Form
    {
        
        private RichTextBox logBox;
        private Button inputButton, outputButton, startButton, stopButton, manageButton;
        private CheckBox autoStartBox;
        private Label inputPathLabel, outputPathLabel, fileTypeLabel;
        private ComboBox fileTypeComboBox;
        private FileSystemWatcher? fsWatcher;
        private string hotFolderPath = string.Empty;
        private string outputFolderPath = string.Empty;
        private string fileExtension = ".tps";
        private bool autoStart;
        private string settingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        private Config config = new Config();
        

        // Classe pour les paires de recherche et remplacement
        public class SearchReplacePair
        {
            public string? Search { get; set; }
            public string? Replace { get; set; }
        }
        

        // Classe pour la configuration
        public class Config
        {
            public string? HotFolderPath { get; set; }
            public string? OutputFolderPath { get; set; }
            public bool autoStart { get; set; }
            public List<SearchReplacePair> SearchReplacePairs { get; set; } = new List<SearchReplacePair>();
        }

        public MainForm()
        {
            // Charger la configuration
            // var config = LoadConfig();
            // hotFolderPath = config.HotFolderPath;
            // outputFolderPath = config.OutputFolderPath;
            LoadConfig();


            // Propriétés du formulaire
            string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "hotfolder.ico");
            this.Icon = new Icon(iconPath);
            this.Text = "Hotfolder RegMarks";
            this.Size = new Size(600, 450);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = true;

            // Mise en page principale
            var mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                AutoSize = true
            };
            this.Controls.Add(mainLayout);

            // Panel pour la sélection des dossiers
            var folderPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(10),
                AutoSize = true
            };
            mainLayout.Controls.Add(folderPanel);

            // Bouton et label pour le dossier d'entrée
            inputButton = new Button { Text = "Choisir le dossier d'entrée", Width = 180 };
            inputButton.Click += ChooseInputFolder;
            folderPanel.Controls.Add(inputButton, 0, 0);

            inputPathLabel = new Label
            {
                Text = string.IsNullOrEmpty(hotFolderPath) ? "Dossier d'entrée : Non sélectionné" : $"Dossier d'entrée : {hotFolderPath}",
                AutoSize = true,
                Padding = new Padding(0, 6, 0, 0)
            };
            folderPanel.Controls.Add(inputPathLabel, 1, 0);

            // Bouton et label pour le dossier de sortie
            outputButton = new Button { Text = "Choisir le dossier de sortie", Width = 180 };
            outputButton.Click += ChooseOutputFolder;
            folderPanel.Controls.Add(outputButton, 0, 1);

            outputPathLabel = new Label
            {
                Text = string.IsNullOrEmpty(outputFolderPath) ? "Dossier de sortie : Non sélectionné" : $"Dossier de sortie : {outputFolderPath}",
                AutoSize = true,
                Padding = new Padding(0, 6, 0, 0)
            };
            folderPanel.Controls.Add(outputPathLabel, 1, 1);

            // Panel pour la sélection du type de fichier
            var fileTypePanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(10),
                AutoSize = true
            };
            mainLayout.Controls.Add(fileTypePanel);

            fileTypeLabel = new Label { Text = "Type de fichier à surveiller :", Width = 180, Padding = new Padding(0, 6, 0, 0) };
            fileTypePanel.Controls.Add(fileTypeLabel, 0, 0);

            fileTypeComboBox = new ComboBox
            {
                Width = 180,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            fileTypeComboBox.Items.AddRange(new[] { ".tps", ".dxf", ".txt" });
            fileTypeComboBox.SelectedIndex = 0;
            fileTypeComboBox.SelectedIndexChanged += (s, e) =>
            {
                fileExtension = fileTypeComboBox.SelectedItem?.ToString() ?? string.Empty;
                LogMessage($"Type de fichier sélectionné : {fileExtension}");
            };
            fileTypePanel.Controls.Add(fileTypeComboBox, 1, 0);

            manageButton = new Button { Text = "Gérer les remplacements", Width = 180 };
            manageButton.Click += OpenManageWindow;
            fileTypePanel.Controls.Add(manageButton, 2, 0);

            // Panel pour les boutons de démarrage/arrêt
            var actionPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(10),
                AutoSize = true
            };
            mainLayout.Controls.Add(actionPanel);

            startButton = new Button { Text = "Démarrer la surveillance", Width = 180, Enabled = true };
            startButton.Click += StartSurveillance;
            actionPanel.Controls.Add(startButton, 0, 0);

            stopButton = new Button { Text = "Arrêter la surveillance", Width = 180, Enabled = false };
            stopButton.Click += StopSurveillance;
            actionPanel.Controls.Add(stopButton, 1, 0);

            autoStartBox = new CheckBox { Text = "Surveillance au lancement", Width = 180, Padding = new Padding(0, 4, 0, 0), Checked = autoStart };
            autoStartBox.CheckedChanged += (s, e) =>
            {
                autoStart = autoStartBox.Checked;
                LogMessage($"Surveillance automatique {(autoStart ? "activée" : "désactivée")}");
                if (autoStart){
                    autoStart = true;
                }
                else {
                    autoStart = false;
                }
                SaveConfig();
            };
            actionPanel.Controls.Add(autoStartBox, 2, 0);

            // Zone de log
            logBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true
            };
            mainLayout.Controls.Add(logBox);

            this.Load += MainForm_Load;
        }
        private void MainForm_Load(object? sender, EventArgs e)
        {
            LogMessage("L'application a été chargée avec succès.", Color.Blue);
            try
            {
                throw new InvalidOperationException("Test d'exception.");   
            }
            catch (Exception ex)
            {
                LogMessage($"Message de l'exception : {ex.Message}", Color.Red);
            }
            if (autoStart)
            {
                StartSurveillance(null, EventArgs.Empty);
            }
        }

        private void LoadConfig()
        {
            if (File.Exists(settingsPath))
            {
                string json = File.ReadAllText(settingsPath);
                config = JsonSerializer.Deserialize<Config>(json) ?? new Config();
            }
            else
            {
                config = new Config();
            }
            // Mettre à jour les chemins d'entrée et de sortie
            autoStart = config.autoStart;
            hotFolderPath = config.HotFolderPath ?? string.Empty;
            outputFolderPath = config.OutputFolderPath ?? string.Empty;
        }
         private void SaveConfig()
        {
            config.autoStart = autoStart;
            config.HotFolderPath = hotFolderPath;
            config.OutputFolderPath = outputFolderPath;
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(settingsPath, json);
        }


        private void OpenManageWindow(object? sender, EventArgs e)
        {
            using var manageWindow = new ManageWindow(config);
             // S'abonner à l'événement SaveCompleted
                manageWindow.SaveCompleted += (s, args) =>
                {
                    LogMessage("Les chaînes de remplacement ont été sauvegardées.", Color.Green);
                    LogMessage($"Sauvegarde terminée : {config.SearchReplacePairs.Count} paires enregistrées.", Color.Green);
                };
            if (manageWindow.ShowDialog() == DialogResult.OK)
            {
                config = manageWindow.GetConfig();
                SaveConfig();
            }
        }

        private void ChooseInputFolder(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog { Description = "Sélectionnez le dossier d'entrée" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                hotFolderPath = dialog.SelectedPath;
                inputPathLabel.Text = $"Dossier d'entrée : {hotFolderPath}";
                LogMessage($"Dossier d'entrée sélectionné : {hotFolderPath}", Color.Blue);
                SaveConfig();
            }
        }

        private void ChooseOutputFolder(object? sender, EventArgs e)
        {
            using var dialog = new FolderBrowserDialog { Description = "Sélectionnez le dossier de sortie" };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                outputFolderPath = dialog.SelectedPath;
                outputPathLabel.Text = $"Dossier de sortie : {outputFolderPath}";
                LogMessage($"Dossier de sortie sélectionné : {outputFolderPath}", Color.Blue);
                SaveConfig();
            }
        }

        private void LogMessage(string message, Color? color = null)
        {
            logBox.SelectionStart = logBox.TextLength;
            logBox.SelectionColor = color ?? Color.Black;
            logBox.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}] {message}\n");
            logBox.SelectionStart = logBox.Text.Length;
            logBox.ScrollToCaret();
        }

        private void StartSurveillance(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hotFolderPath))
            {
                LogMessage("Le dossier d'entrée doit être sélectionné avant de démarrer la surveillance.", Color.Red);
                return;
            }

            if (string.IsNullOrEmpty(outputFolderPath))
            {
                LogMessage("Le dossier de sortie doit être sélectionné avant de démarrer la surveillance.", Color.Red);
                return;
            }

            if (!Directory.Exists(hotFolderPath))
            {
                LogMessage("Le dossier d'entrée n'existe pas. Veuillez sélectionner un dossier valide.", Color.Red);
                return;
            }
            if (!Directory.Exists(outputFolderPath))
            {
                LogMessage("Le dossier de sortie n'existe pas. Veuillez sélectionner un dossier valide.", Color.Red);
                return;
            }

            fsWatcher = new FileSystemWatcher
            {
                Path = hotFolderPath,
                Filter = $"*{fileExtension}",
                EnableRaisingEvents = true
            };
            fsWatcher.Created += (s, e) =>
            {
                FileCreated(e.FullPath);
            };

            if (autoStart)
            {
                LogMessage("Surveillance démarrée automatiquement.", Color.Green);
            }
            else
            {
                LogMessage("Surveillance démarrée manuellement.", Color.Green);
            }
            startButton.Enabled = false;
            stopButton.Enabled = true;
        }

        private void StopSurveillance(object? sender, EventArgs e)
        {
            if (fsWatcher != null)
            {
                fsWatcher.EnableRaisingEvents = false;
                fsWatcher.Dispose();
                fsWatcher = null;
            }

            LogMessage("Surveillance arrêtée.", Color.Red);
            startButton.Enabled = true;
            stopButton.Enabled = false;
        }


        private void FileCreated(string filePath)
        {
            const int maxRetries = 5;
            int attempt = 0;
            bool fileLocked = true;

            while (fileLocked && attempt < maxRetries)
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var outputFilePath = Path.Combine(outputFolderPath, $"{fileName}_processed{fileExtension}");

                    // Lire le contenu du fichier
                    var content = File.ReadAllText(filePath);

                    // Appliquer tous les remplacements à partir de la configuration déjà chargée
                    foreach (var pair in config.SearchReplacePairs)
                    {
                        if (!string.IsNullOrEmpty(pair.Search))
                        {
                            content = content.Replace(pair.Search, pair.Replace ?? string.Empty);
                        }
                    }

                    // Sauvegarder le fichier modifié
                    File.WriteAllText(outputFilePath, content);

                    LogMessage($"Fichier traité et sauvegardé dans: {outputFilePath}", Color.Green);
                    fileLocked = false; // Succès, sortir de la boucle
                }
                catch (IOException ex) when (attempt < maxRetries)
                {
                    // Si le fichier est utilisé par un autre processus, réessayer après un délai
                    attempt++;
                    LogMessage($"Le fichier est utilisé, tentative {attempt}/{maxRetries} : {ex.Message}", Color.Orange);
                    Thread.Sleep(500); // Attendre 500 ms avant de réessayer
                }
                catch (Exception ex)
                {
                    // Autres erreurs
                    LogMessage($"Erreur lors du traitement du fichier : {ex.Message}", Color.Red);
                    fileLocked = false; // En cas d'erreur non liée à un fichier verrouillé
                }
            }
        }
    }

    public class ManageWindow : Form
    {
        private MainForm.Config config = new MainForm.Config();
        public event EventHandler SaveCompleted = delegate { };

        public MainForm.Config GetConfig()
        {
            return config;
        }

        private void SetConfig(MainForm.Config value)
        {
            config = value;
        }

        private DataGridView dgv;
        private Button saveButton, cancelButton;

        public ManageWindow(MainForm.Config config)
        {
            this.SetConfig(config);
            this.Text = "Gérer les chaînes de remplacement";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 1,
                Padding = new Padding(10)
            };

            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 80)); // 80% pour le tableau
            layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));    // Auto pour les boutons

            // Tableau des chaînes de remplacement
            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = true,
                AllowUserToDeleteRows = true,
                AllowUserToResizeRows = false, // Verrouiller la modification de la hauteur des lignes
                ColumnCount = 2
            };
            dgv.Columns[0].HeaderText = "Chaîne à rechercher";
            dgv.Columns[1].HeaderText = "Chaîne de remplacement";

            // Charger les données dans le tableau
            foreach (var pair in config.SearchReplacePairs)
            {
                dgv.Rows.Add(pair.Search ?? string.Empty, pair.Replace ?? string.Empty);
            }

            // Boutons
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true
            };

            saveButton = new Button { Text = "Enregistrer", Width = 100 };
            saveButton.Click += SaveChanges;

            cancelButton = new Button { Text = "Annuler", Width = 100 };
            cancelButton.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            buttonPanel.Controls.Add(saveButton);
            buttonPanel.Controls.Add(cancelButton);

            layout.Controls.Add(dgv);
            layout.Controls.Add(buttonPanel);
            this.Controls.Add(layout);
        }

        private void SaveChanges(object? sender, EventArgs e)
        {
            GetConfig().SearchReplacePairs.Clear();
            
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[1].Value != null)
                {
                    GetConfig().SearchReplacePairs.Add(new MainForm.SearchReplacePair
                    {
                        Search = row.Cells[0].Value?.ToString() ?? string.Empty,
                        Replace = row.Cells[1].Value?.ToString() ?? string.Empty
                    });

                }
            }
                
            // Déclencher l'événement SaveCompleted
            SaveCompleted?.Invoke(this, EventArgs.Empty);
            this.DialogResult = DialogResult.OK;
        }
        [STAThread]
        static void Main()
        {    
            // Démarrer l'application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
        }
    }
}