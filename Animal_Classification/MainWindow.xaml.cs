using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Media;

namespace Animal_Classification
{
    public partial class MainWindow : Window
    {
        private string? imagePath;
        private bool isDarkMode = false;

        // GitHub Profile
        private const string GitHubUser = "PandaHunterX";
        private const string GitHubUrl = "https://github.com/PandaHunterX";
        private const string GitHubAvatar = "https://github.com/PandaHunterX.png?size=92";

        public MainWindow()
        {
            InitializeComponent();
            LoadGitHubAvatar();
        }
        // Sounds
        private static readonly string SoundClickPath = GetSoundPath(@"Assets\Sounds\click.wav");
        private static readonly string SoundChimePath = GetSoundPath(@"Assets\Sounds\chime.wav");
        private static readonly string SoundErrorPath = GetSoundPath(@"Assets\Sounds\error.wav");

        private static string GetSoundPath(string relativePath) =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        private static void PlaySound(string filePath)
        {
            if (!File.Exists(filePath)) return; 

            Task.Run(() =>
            {
                try
                {
                    using var player = new SoundPlayer(filePath);
                    player.PlaySync();
                }
                catch { /* audio errors are non-fatal */ }
            });
        }


        // Theme Toggle
        private void ThemeToggle_Click(object sender, RoutedEventArgs e)
        {
            PlaySound(SoundClickPath);
            isDarkMode = !isDarkMode;
            ApplyTheme(isDarkMode);
        }

        private void ApplyTheme(bool dark)
        {
            var res = this.Resources;

            if (dark)
            {
                // Dark palette 
                res["BgWindow"] = Brush("#0B0E16");
                res["BgCard"] = Brush("#12161F");
                res["BgDropZone"] = Brush("#0D1018");
                res["BgResultCard"] = Brush("#111A08");
                res["BorderCard"] = Brush("#2A3040");
                res["BorderDrop"] = Brush("#2A3040");
                res["BorderResult"] = Brush("#4A7A10");
                res["TxtTitle"] = Brush("#E8F0D8");
                res["TxtMuted"] = Brush("#7A8F6A");
                res["TxtPlaceholder"] = Brush("#3A4A30");
                res["TxtLabel"] = Brush("#8FBA5A");
                res["Accent"] = Brush("#AAEE30");
                res["AccentHover"] = Brush("#C8F858");
                res["AccentPressed"] = Brush("#88CC10");
                res["BtnOutlineFg"] = Brush("#AAEE30");
                res["BtnOutlineBg"] = Brush("Transparent");
                res["BtnOutlineHoverBg"] = Brush("#1C2E08");
                res["ProgressBg"] = Brush("#1A2030");
                res["Divider"] = Brush("#232838");
                res["GhCardBg"] = Brush("#0E1218");
                res["GhCardBorder"] = Brush("#2A3040");
                res["ToggleBg"] = Brush("#1A2810");
                res["ToggleFg"] = Brush("#AAEE30");

                ThemeToggleBtn.Content = "🌙  DARK";
            }
            else
            {
                // Light palette 
                res["BgWindow"] = Brush("#F2F5EE");
                res["BgCard"] = Brush("#FFFFFF");
                res["BgDropZone"] = Brush("#F0F4EB");
                res["BgResultCard"] = Brush("#F0FAE0");
                res["BorderCard"] = Brush("#DDE5D0");
                res["BorderDrop"] = Brush("#C8D8B8");
                res["BorderResult"] = Brush("#A8D060");
                res["TxtTitle"] = Brush("#1A2208");
                res["TxtMuted"] = Brush("#8A9A78");
                res["TxtPlaceholder"] = Brush("#AABAA0");
                res["TxtLabel"] = Brush("#6A8A58");
                res["Accent"] = Brush("#5A9E00");
                res["AccentHover"] = Brush("#6DBC00");
                res["AccentPressed"] = Brush("#447800");
                res["BtnOutlineFg"] = Brush("#5A9E00");
                res["BtnOutlineBg"] = Brush("Transparent");
                res["BtnOutlineHoverBg"] = Brush("#EAF5D0");
                res["ProgressBg"] = Brush("#DDE8CC");
                res["Divider"] = Brush("#DDE5D0");
                res["GhCardBg"] = Brush("#F7FAF2");
                res["GhCardBorder"] = Brush("#DDE5D0");
                res["ToggleBg"] = Brush("#E2EDD5");
                res["ToggleFg"] = Brush("#4A8A00");

                ThemeToggleBtn.Content = "☀  LIGHT";
            }
        }


        private static SolidColorBrush Brush(string hex) =>
            hex == "Transparent"
                ? new SolidColorBrush(Colors.Transparent)
                : new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));


        //  GITHUB Loader
        private async void LoadGitHubAvatar()
        {
            try
            {
                using var http = new HttpClient();
                http.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");
                var bytes = await http.GetByteArrayAsync(GitHubAvatar);

                var bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(bytes);
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                bmp.Freeze();


                AvatarBrush.ImageSource = bmp;
            }
            catch
            {
                // Avatar failed to load — silently ignore; card still shows username + link
            }
        }

        private void GitHubLink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(GitHubUrl) { UseShellExecute = true });
        }

        //  IMAGE UPLOAD
        private void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            PlaySound(SoundClickPath);
            var dlg = new OpenFileDialog
            {
                Filter = "Image files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            if (dlg.ShowDialog() == true)
            {
                imagePath = dlg.FileName;
                UploadedImage.Source = new BitmapImage(new Uri(imagePath));
                FileNameLabel.Text = Path.GetFileName(imagePath);
                PlaceholderPanel.Visibility = Visibility.Collapsed;
                ResultCard.Visibility = Visibility.Collapsed;
                PredictionResult.Opacity = 0;
            }
        }

        //  Classification
        private async void Predict_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                PlaySound(SoundErrorPath);
                var dialog = new NoImageDialog(this, isDarkMode);
                dialog.ShowDialog();
                return;
            }

            PlaySound(SoundClickPath);

            try
            {
                LoadingPanel.Visibility = Visibility.Visible;
                ResultCard.Visibility = Visibility.Collapsed;
                PredictionResult.Text = "";
                ConfidenceValue.Text = "";
                UploadBtn.IsEnabled = false;
                PredictBtn.IsEnabled = false;

                string predLabel = "";
                float bestScore = 0f;

                await Task.WhenAll(
                    Task.Run(() =>
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath);
                        var input = new Animal_Classifier.ModelInput { ImageSource = imageBytes };
                        var result = Animal_Classifier.Predict(input);
                        predLabel = result.PredictedLabel;
                        bestScore = result.Score.Max();
                    }),
                    Task.Delay(3000) 
                );

                PredictionResult.Text = $"🐾  {predLabel.ToUpper()}";

                ConfidenceValue.Text = $"{bestScore:P2}";
                ConfidenceValue.Foreground = bestScore >= 0.75
                    ? new SolidColorBrush((Color)ColorConverter.ConvertFromString(isDarkMode ? "#AAEE30" : "#4A9E00"))
                    : bestScore >= 0.45
                        ? new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"))
                        : new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));

                ResultCard.Visibility = Visibility.Visible;

                var sb = (Storyboard)FindResource("PredictionAnimation");
                sb.Begin(PredictionResult);

                PlaySound(SoundChimePath); 
            }
            catch (Exception ex)
            {
                PlaySound(SoundErrorPath);
                MessageBox.Show(
                    "Prediction error: " + ex.Message +
                    "\n\nINNER:\n" + ex.InnerException?.Message +
                    "\n\nSTACK:\n" + ex.InnerException?.StackTrace,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LoadingPanel.Visibility = Visibility.Collapsed;
                UploadBtn.IsEnabled = true;
                PredictBtn.IsEnabled = true;
            }
        }
    }
}
