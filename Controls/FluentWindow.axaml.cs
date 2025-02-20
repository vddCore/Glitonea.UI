namespace Glitonea.UI;

using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;

[TemplatePart(PART_CaptionBarWrapper, typeof(Border))]
[TemplatePart(PART_CaptionBar, typeof(Grid))]
[TemplatePart(PART_CaptionButtons, typeof(FluentCaptionButtons))]
public class FluentWindow : Window
{
    private const string PART_CaptionBarWrapper = "PART_CaptionBarWrapper";
    private const string PART_CaptionBar = "PART_CaptionBar";
    private const string PART_CaptionButtons = "PART_CaptionButtons";
    
    private CompositeDisposable? _disposables;
    private Grid? _captionBar;
    private FluentCaptionButtons? _captionButtons;

    protected override Type StyleKeyOverride => typeof(FluentWindow);

    public new static readonly StyledProperty<Bitmap> IconProperty =
        AvaloniaProperty.Register<FluentWindow, Bitmap>(nameof(Icon));

    public static readonly StyledProperty<bool> ShowIconProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowIcon), true
        );
    
    public static readonly StyledProperty<bool> CanFullScreenProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(CanFullScreen)
        );
    
    public static readonly StyledProperty<bool> CanMinimizeProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(CanMinimize), true
        );
    
    public static readonly StyledProperty<bool> CanMaximizeProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(CanMaximize), true
        );
    
    public static readonly StyledProperty<bool> CanCloseProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(CanClose), true
        );

    public static readonly StyledProperty<bool> CanMoveProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(CanMove), true
        );    
    
    public static readonly StyledProperty<bool> ShowFullScreenButtonProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(ShowFullScreenButton)
        );
    
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

    public static readonly StyledProperty<IBrush> MinimizeButtonBackgroundBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(MinimizeButtonBackgroundBrush)
        );

    public static readonly StyledProperty<IBrush> MaximizeButtonBackgroundBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(MaximizeButtonBackgroundBrush)
        );

    public static readonly StyledProperty<IBrush> FullScreenButtonBackgroundBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(FullScreenButtonBackgroundBrush)
        );

    public static readonly StyledProperty<IBrush> CloseButtonBackgroundBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(CloseButtonBackgroundBrush)
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
            nameof(CaptionBarHeight), 30
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

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }
    
    public bool CanFullScreen
    {
        get => GetValue(CanFullScreenProperty);
        set => SetValue(CanFullScreenProperty, value);
    }

    public bool CanMinimize
    {
        get => GetValue(CanMinimizeProperty);
        set => SetValue(CanMinimizeProperty, value);
    }

    public bool CanMaximize
    {
        get => GetValue(CanMaximizeProperty);
        set => SetValue(CanMaximizeProperty, value);
    }

    public bool CanClose
    {
        get => GetValue(CanCloseProperty);
        set => SetValue(CanCloseProperty, value);
    }

    public bool CanMove
    {
        get => GetValue(CanMoveProperty);
        set => SetValue(CanMoveProperty, value);
    }

    public bool ShowFullScreenButton
    {
        get => GetValue(ShowFullScreenButtonProperty);
        set => SetValue(ShowFullScreenButtonProperty, value);
    }

    public bool ShowMinimizeButton
    {
        get => GetValue(ShowMinimizeButtonProperty);
        set => SetValue(ShowMinimizeButtonProperty, value);
    }
    
    public bool ShowMaximizeButton
    {
        get => GetValue(ShowMaximizeButtonProperty);
        set => SetValue(ShowMaximizeButtonProperty, value);
    }

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public IBrush FullScreenButtonBackgroundBrush
    {
        get => GetValue(FullScreenButtonBackgroundBrushProperty);
        set => SetValue(FullScreenButtonBackgroundBrushProperty, value);
    }

    public IBrush MinimizeButtonBackgroundBrush
    {
        get => GetValue(MinimizeButtonBackgroundBrushProperty);
        set => SetValue(MinimizeButtonBackgroundBrushProperty, value);
    }

    public IBrush MaximizeButtonBackgroundBrush
    {
        get => GetValue(MaximizeButtonBackgroundBrushProperty);
        set => SetValue(MaximizeButtonBackgroundBrushProperty, value);
    }

    public IBrush CloseButtonBackgroundBrush
    {
        get => GetValue(CloseButtonBackgroundBrushProperty);
        set => SetValue(CloseButtonBackgroundBrushProperty, value);
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
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _captionBar = e.NameScope.RequireInternal<Grid>(PART_CaptionBar);
        _captionButtons = e.NameScope.RequireInternal<FluentCaptionButtons>("PART_CaptionButtons");
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        _captionBar!.PointerPressed += OnCaptionBarPointerPressed;
        _captionButtons!.LayoutUpdated += CaptionButtons_LayoutUpdated;
        _captionButtons!.Resources.Add("CaptionButtonHeight", CaptionBarHeight);
        _captionButtons!.Attach(this);

        _disposables = new CompositeDisposable
        {
            this.GetObservable(WindowStateProperty).Subscribe(x =>
            {
                PseudoClasses.Set(":normal", x == WindowState.Normal);
                PseudoClasses.Set(":minimized", x == WindowState.Minimized);
                PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
            })
        };

        base.OnLoaded(e);
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        _captionBar!.PointerPressed -= OnCaptionBarPointerPressed;
        _captionButtons!.LayoutUpdated -= CaptionButtons_LayoutUpdated;
        _captionButtons!.Detach();
        _disposables?.Dispose();

        base.OnUnloaded(e);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (!CanClose)
        {
            e.Cancel = true;
        }
        
        base.OnClosing(e);
    }

    private void CaptionButtons_LayoutUpdated(object? sender, EventArgs e)
    {
        CaptionContentSafeAreaMargin = new Thickness(0, 0, ((FluentCaptionButtons)sender!).Bounds.Width, 0);
    }

    private void OnCaptionBarPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!CanMove)
            return;
        
        if (WindowState == WindowState.FullScreen)
            return;

        if (_captionBar!.IsPointerOver)
        {
            var properties = e.GetCurrentPoint(this).Properties;

            if (properties.IsLeftButtonPressed)
            {
                if (e.ClickCount == 2 && CanMaximize)
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
}