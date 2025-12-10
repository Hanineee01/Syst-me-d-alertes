using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using TTronAlert.Desktop.ViewModels;

namespace TTronAlert.Desktop.Views;

public partial class AlertToastWindow : Window
{
    private readonly DispatcherTimer _autoCloseTimer;
    private readonly DispatcherTimer _animationTimer;
    private const int AutoCloseDurationSeconds = 5;
    private const int AnimationSteps = 20;
    private const int AnimationInterval = 20;
    private bool _isAnimating;

    public AlertToastWindow()
    {
        InitializeComponent();

        _autoCloseTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(AutoCloseDurationSeconds)
        };
        _autoCloseTimer.Tick += (s, e) => BeginClose();

        _animationTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(AnimationInterval)
        };
    }

    public void SetAlert(AlertItemViewModel alert)
    {
        DataContext = alert;

        var border = this.FindControl<Border>("ToastBorder");
        if (border != null)
        {
            border.Classes.Clear();
            border.Classes.Add(alert.LevelClass);
        }
    }

    public void PositionToast(int index)
    {
        var screens = Screens.All;
        if (screens.Count == 0) return;

        var screen = screens[0];
        var workingArea = screen.WorkingArea;

        const int margin = 20;
        const int spacing = 12;
        const int toastHeight = 160;

        Position = new PixelPoint(
            workingArea.Right - (int)Width - margin,
            workingArea.Bottom - (toastHeight + margin) - (index * (toastHeight + spacing))
        );
    }

    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        await AnimateIn();
        _autoCloseTimer.Start();
    }

    private async Task AnimateIn()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        var border = this.FindControl<Border>("ToastBorder");
        if (border == null)
        {
            _isAnimating = false;
            return;
        }

        var translateTransform = border.RenderTransform as TranslateTransform;
        if (translateTransform == null)
        {
            _isAnimating = false;
            return;
        }

        var tcs = new TaskCompletionSource<bool>();
        int step = 0;

        _animationTimer.Tick += AnimationTick;
        _animationTimer.Start();

        void AnimationTick(object? s, EventArgs args)
        {
            step++;
            double progress = (double)step / AnimationSteps;
            progress = EaseOutCubic(progress);

            translateTransform.X = 400 * (1 - progress);
            border.Opacity = progress;

            if (step >= AnimationSteps)
            {
                _animationTimer.Stop();
                _animationTimer.Tick -= AnimationTick;
                _isAnimating = false;
                tcs.SetResult(true);
            }
        }

        await tcs.Task;
    }

    private async Task AnimateOut()
    {
        if (_isAnimating) return;
        _isAnimating = true;

        var border = this.FindControl<Border>("ToastBorder");
        if (border == null)
        {
            _isAnimating = false;
            return;
        }

        var translateTransform = border.RenderTransform as TranslateTransform;
        if (translateTransform == null)
        {
            _isAnimating = false;
            return;
        }

        var tcs = new TaskCompletionSource<bool>();
        int step = 0;

        _animationTimer.Tick += AnimationTick;
        _animationTimer.Start();

        void AnimationTick(object? s, EventArgs args)
        {
            step++;
            double progress = (double)step / AnimationSteps;
            progress = EaseInCubic(progress);

            translateTransform.X = 400 * progress;
            border.Opacity = 1 - progress;

            if (step >= AnimationSteps)
            {
                _animationTimer.Stop();
                _animationTimer.Tick -= AnimationTick;
                _isAnimating = false;
                tcs.SetResult(true);
            }
        }

        await tcs.Task;
    }

    private static double EaseOutCubic(double t)
    {
        return 1 - Math.Pow(1 - t, 3);
    }

    private static double EaseInCubic(double t)
    {
        return t * t * t;
    }

    private async void BeginClose()
    {
        _autoCloseTimer.Stop();
        await AnimateOut();
        Close();
    }

    private void CloseButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BeginClose();
    }
}
