namespace Glitonea.UI;

using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform;

public class FluentWindow : Window
{
    private Control? _captionBar;

    private Button? _maximizeButton;
    private Button? _fullScreenButton;
    private Button? _minimizeButton;
    private Button? _closeButton;

    private CompositeDisposable _disposables;

    protected override Type StyleKeyOverride => typeof(FluentWindow);

    public static readonly StyledProperty<bool> ShowMinimizeButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowMinimizeButton), true
        );

    public static readonly StyledProperty<bool> ShowMaximizeButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowMaximizeButton), true
        );

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowMaximizeButton), true
        );

    public static readonly StyledProperty<IBrush> CaptionBarBackgroundProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(CaptionBarBackground)
        );

    public static readonly StyledProperty<object> CaptionBarProperty =
        AvaloniaProperty.Register<FluentWindow, object>(
            nameof(CaptionBar)
        );
    
    public static readonly StyledProperty<double> CaptionBarHeightProperty =
        AvaloniaProperty.Register<FluentWindow, double>(
            nameof(CaptionBarHeight), 31
        );

    public bool ShowMaximizeButton
    {
        get => GetValue(ShowMaximizeButtonProperty);
        set => SetValue(ShowMaximizeButtonProperty, value);
    }

    public bool ShowMinimizeButton
    {
        get => GetValue(ShowMinimizeButtonProperty);
        set => SetValue(ShowMinimizeButtonProperty, value);
    }

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public IBrush CaptionBarBackground
    {
        get => GetValue(CaptionBarBackgroundProperty);
        set => SetValue(CaptionBarBackgroundProperty, value);
    }

    public object CaptionBar
    {
        get => GetValue(CaptionBarProperty);
        set => SetValue(CaptionBarProperty, value);
    }
    
    public double CaptionBarHeight
    {
        get => GetValue(CaptionBarHeightProperty);
        set => SetValue(CaptionBarHeightProperty, value);
    }

    public FluentWindow()
    {
        TransparencyLevelHint = new[]
        {
            WindowTransparencyLevel.AcrylicBlur,
            WindowTransparencyLevel.Mica,
            WindowTransparencyLevel.Blur
        };

        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = CaptionBarHeight;

        _disposables = new CompositeDisposable()
        {
            this.GetObservable(WindowStateProperty)
                .Subscribe(x =>
                {
                    PseudoClasses.Set(":normal", x == WindowState.Normal);
                    PseudoClasses.Set(":minimized", x == WindowState.Minimized);
                    PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                    PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                }),

            this.GetObservable(ShowMaximizeButtonProperty)
                .Subscribe(x =>
                {
                    if (_maximizeButton != null)
                        _maximizeButton.IsVisible = x;
                }),

            this.GetObservable(ShowMinimizeButtonProperty)
                .Subscribe(x =>
                {
                    if (_minimizeButton != null)
                        _minimizeButton.IsVisible = x;
                }),

            this.GetObservable(ShowCloseButtonProperty)
                .Subscribe(x =>
                {
                    if (_closeButton != null)
                        _closeButton.IsVisible = x;
                })
        };
    }

    protected override void OnUnloaded(RoutedEventArgs e)
        => _disposables.Dispose();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _captionBar = e.NameScope.Find<Border>("PART_CaptionBar");

        if (_captionBar == null)
        {
            throw new InvalidOperationException(
                "Cannot find a critical template element. " +
                "Forgor to include <GlitoneaUI/> in your application styles?"
            );
        }
        
        _captionBar.PointerPressed += OnCaptionBarPointerPressed;

        var captionButtons = e.NameScope.Find<CaptionButtons>(
            "PART_CaptionButtons"
        );

        if (captionButtons != null)
        {
            captionButtons.TemplateApplied += CaptionButtonsOnTemplateApplied;
            captionButtons.Attach(this);
        }
    }

    private void CaptionButtonsOnTemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        _closeButton = e.NameScope.Find<Button>("PART_CloseButton");
        _fullScreenButton = e.NameScope.Find<Button>("PART_FullScreenButton");
        _maximizeButton = e.NameScope.Find<Button>("PART_RestoreButton");
        _minimizeButton = e.NameScope.Find<Button>("PART_MinimizeButton");

        if (_maximizeButton != null)
            _maximizeButton.IsVisible = ShowMaximizeButton;

        if (_minimizeButton != null)
            _minimizeButton.IsVisible = ShowMinimizeButton;

        if (_closeButton != null)
            _closeButton.IsVisible = ShowCloseButton;
    }

    private void OnCaptionBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(null).Properties;

        if (properties.IsLeftButtonPressed)
        {
            if (e.ClickCount == 2 && ShowMaximizeButton)
            {
                WindowState = WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;
            }
            else
            {
                BeginMoveDrag(e);
            }
        }
    }
}