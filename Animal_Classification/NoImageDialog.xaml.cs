using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Animal_Classification
{
    public partial class NoImageDialog : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // ── Bindable theme colors ────────────────────────────
        private Brush _cardBg     = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        private Brush _cardBorder = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDE5D0"));
        private Brush _labelColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6A8A58"));
        private Brush _bodyColor  = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A4A28"));
        private Brush _mutedColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8A9A78"));

        public Brush CardBg     { get => _cardBg;     set { _cardBg     = value; OnProp(nameof(CardBg));     } }
        public Brush CardBorder { get => _cardBorder; set { _cardBorder = value; OnProp(nameof(CardBorder)); } }
        public Brush LabelColor { get => _labelColor; set { _labelColor = value; OnProp(nameof(LabelColor)); } }
        public Brush BodyColor  { get => _bodyColor;  set { _bodyColor  = value; OnProp(nameof(BodyColor));  } }
        public Brush MutedColor { get => _mutedColor; set { _mutedColor = value; OnProp(nameof(MutedColor)); } }

        public NoImageDialog(Window owner, bool isDarkMode)
        {
            InitializeComponent();
            DataContext = this;
            Owner = owner;

            ApplyTheme(isDarkMode);

            // Play fade-in animation once loaded
            Loaded += (_, _) =>
            {
                var sb = (System.Windows.Media.Animation.Storyboard)FindResource("FadeIn");
                sb.Begin();
            };
        }

        private void ApplyTheme(bool dark)
        {
            if (dark)
            {
                CardBg     = C("#12161F");
                CardBorder = C("#2A3040");
                LabelColor = C("#8FBA5A");
                BodyColor  = C("#C8D8B8");
                MutedColor = C("#5A6A48");
            }
            else
            {
                CardBg     = C("#FFFFFF");
                CardBorder = C("#DDE5D0");
                LabelColor = C("#6A8A58");
                BodyColor  = C("#3A4A28");
                MutedColor = C("#8A9A78");
            }
        }

        private static SolidColorBrush C(string hex) =>
            new SolidColorBrush((Color)ColorConverter.ConvertFromString(hex));

        private void OnProp(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        // Allow dragging the dialog by clicking anywhere on the card
        private void DialogCard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
