namespace Glitonea.UI;

using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
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

    public new static readonly StyledProperty<Bitmap> IconProperty =
        AvaloniaProperty.Register<FluentWindow, Bitmap>(nameof(Icon));

    public static readonly StyledProperty<bool> ShowMinimizeButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowMinimizeButton), true
        );

    public static readonly StyledProperty<bool> ShowMaximizeButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowMaximizeButton), true
        );

    public static readonly StyledProperty<bool> ShowFullScreenButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowFullScreenButton)
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

    internal static readonly StyledProperty<Thickness> CaptionContentSafeAreaMarginProperty =
        AvaloniaProperty.Register<FluentWindow, Thickness>(
            nameof(CaptionContentSafeAreaMargin)
        );

    public new Bitmap Icon
    {
        get => GetValue(IconProperty);
        set
        {
            SetValue(IconProperty, value);
            base.Icon = new WindowIcon(Icon);
        }
    }

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

    public bool ShowFullScreenButton
    {
        get => GetValue(ShowFullScreenButtonProperty);
        set => SetValue(ShowFullScreenButtonProperty, value);
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

    internal Thickness CaptionContentSafeAreaMargin
    {
        get => GetValue(CaptionContentSafeAreaMarginProperty);
        set => SetValue(CaptionContentSafeAreaMarginProperty, value);
    }

    public FluentWindow()
    {
        TransparencyLevelHint =
        [
            WindowTransparencyLevel.AcrylicBlur,
            WindowTransparencyLevel.Mica,
            WindowTransparencyLevel.Blur
        ];

        ExtendClientAreaToDecorationsHint = true;
        ExtendClientAreaChromeHints = ExtendClientAreaChromeHints.NoChrome;
        ExtendClientAreaTitleBarHeightHint = CaptionBarHeight;

        _disposables = new CompositeDisposable
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

        _captionBar = e.NameScope.Find<Control>("PART_CaptionBar");

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
            captionButtons.Resources.Add("CaptionButtonHeight", CaptionBarHeight);
            captionButtons.TemplateApplied += CaptionButtons_TemplateApplied;
            captionButtons.LayoutUpdated += CaptionButtons_LayoutUpdated;
            captionButtons.Attach(this);
        }
    }

    private void CaptionButtons_TemplateApplied(object? sender, TemplateAppliedEventArgs e)
    {
        _minimizeButton = e.NameScope.Find<Button>("PART_MinimizeButton");
        _maximizeButton = e.NameScope.Find<Button>("PART_RestoreButton");
        _fullScreenButton = e.NameScope.Find<Button>("PART_FullScreenButton");
        _closeButton = e.NameScope.Find<Button>("PART_CloseButton");

        if (_minimizeButton != null)
            _minimizeButton.IsVisible = ShowMinimizeButton;
        
        if (_maximizeButton != null)
            _maximizeButton.IsVisible = ShowMaximizeButton;

        if (_fullScreenButton != null)
            _fullScreenButton.IsVisible = ShowFullScreenButton;

        if (_closeButton != null)
            _closeButton.IsVisible = ShowCloseButton;
    }

    private void CaptionButtons_LayoutUpdated(object? sender, EventArgs e)
    {
        CaptionContentSafeAreaMargin = new Thickness(0, 0, ((CaptionButtons)sender!).Bounds.Width, 0);
    }

    private void OnCaptionBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (WindowState == WindowState.FullScreen)
            return;
        
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