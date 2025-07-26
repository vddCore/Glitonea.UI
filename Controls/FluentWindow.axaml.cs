namespace Glitonea.UI.Controls;

using System.Reactive.Disposables;
using System.Runtime.Versioning;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.Immutable;
using Avalonia.Platform;
using static Glitonea.UI.Platform.Win32;

[TemplatePart(PART_CaptionBarWrapper, typeof(Border))]
[TemplatePart(PART_CaptionBar, typeof(Grid))]
[TemplatePart(PART_CaptionButtons, typeof(FluentCaptionButtons))]
[TemplatePart(PART_WindowBorder, typeof(Border))]
public class FluentWindow : Window
{
    private const string PART_CaptionBarWrapper = "PART_CaptionBarWrapper";
    private const string PART_CaptionBar = "PART_CaptionBar";
    private const string PART_CaptionButtons = "PART_CaptionButtons";
    private const string PART_WindowBorder = "PART_WindowBorder";
    
    private CompositeDisposable? _disposables;
    private Grid? _captionBar;
    private FluentCaptionButtons? _captionButtons;
    private Border? _windowBorder;
    
    private WindowState? _preMaximizeWindowState;
    private Thickness? _preMaximizeWindowBorderThickness;

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

    public static readonly StyledProperty<IBrush> MinimizeButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(MinimizeButtonBackground)
        );

    public static readonly StyledProperty<IBrush> MaximizeButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(MaximizeButtonBackground)
        );

    public static readonly StyledProperty<IBrush> FullScreenButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(FullScreenButtonBackground)
        );

    public static readonly StyledProperty<IBrush> CloseButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(CloseButtonBackground)
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
    
    public static readonly StyledProperty<IBrush> ActiveWindowBorderBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(ActiveWindowBorderBrush)
        );
    
    public static readonly StyledProperty<IBrush> InactiveWindowBorderBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush>(
            nameof(InactiveWindowBorderBrush)
        );

    public static readonly StyledProperty<bool> EnableCustomWindowBorderProperty =
        AvaloniaProperty.Register<FluentWindow, bool>(
            nameof(EnableCustomWindowBorder)
        );

    public static readonly StyledProperty<Thickness?> CaptionContentSafeAreaMarginProperty =
        AvaloniaProperty.Register<FluentWindow, Thickness?>(
            nameof(CaptionContentSafeAreaMargin)
        );
    
    public static readonly StyledProperty<Thickness?> WindowBorderThicknessProperty =
        AvaloniaProperty.Register<FluentWindow, Thickness?>(
            nameof(WindowBorderThickness)
        );
    
    internal static readonly StyledProperty<Thickness> InternalCaptionContentSafeAreaMarginProperty =
        AvaloniaProperty.Register<FluentWindow, Thickness>(
            nameof(CaptionContentSafeAreaMargin)
        );

    internal static readonly StyledProperty<IBrush?> CurrentWindowBorderBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush?>(
            nameof(CurrentWindowBorderBrush)
        );



    internal static readonly StyledProperty<IBrush?> CurrentCaptionBorderBrushProperty =
        AvaloniaProperty.Register<FluentWindow, IBrush?>(
            nameof(CurrentCaptionBorderBrush)
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

    public IBrush FullScreenButtonBackground
    {
        get => GetValue(FullScreenButtonBackgroundProperty);
        set => SetValue(FullScreenButtonBackgroundProperty, value);
    }

    public IBrush MinimizeButtonBackground
    {
        get => GetValue(MinimizeButtonBackgroundProperty);
        set => SetValue(MinimizeButtonBackgroundProperty, value);
    }

    public IBrush MaximizeButtonBackground
    {
        get => GetValue(MaximizeButtonBackgroundProperty);
        set => SetValue(MaximizeButtonBackgroundProperty, value);
    }

    public IBrush CloseButtonBackground
    {
        get => GetValue(CloseButtonBackgroundProperty);
        set => SetValue(CloseButtonBackgroundProperty, value);
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

    public IBrush ActiveWindowBorderBrush
    {
        get => GetValue(ActiveWindowBorderBrushProperty);
        set => SetValue(ActiveWindowBorderBrushProperty, value);
    }

    public IBrush InactiveWindowBorderBrush
    {
        get => GetValue(InactiveWindowBorderBrushProperty);
        set => SetValue(InactiveWindowBorderBrushProperty, value);
    }

    public bool EnableCustomWindowBorder
    {
        get => GetValue(EnableCustomWindowBorderProperty);
        set => SetValue(EnableCustomWindowBorderProperty, value);
    }
    
    public IBrush? CurrentCaptionBorderBrush
    {
        get => GetValue(CurrentCaptionBorderBrushProperty);
        private set => SetValue(CurrentCaptionBorderBrushProperty, value);
    }
    
    public Thickness? CaptionContentSafeAreaMargin
    {
        get => GetValue(CaptionContentSafeAreaMarginProperty);
        set => SetValue(CaptionContentSafeAreaMarginProperty, value);
    }
    
    public Thickness? WindowBorderThickness
    {
        get => GetValue(WindowBorderThicknessProperty);
        set => SetValue(WindowBorderThicknessProperty, value);
    }

    internal Thickness InternalCaptionContentSafeAreaMargin
    {
        get => GetValue(InternalCaptionContentSafeAreaMarginProperty);
        set => SetValue(InternalCaptionContentSafeAreaMarginProperty, value);
    }
    
    internal IBrush? CurrentWindowBorderBrush
    {
        get => GetValue(CurrentWindowBorderBrushProperty);
        set  => SetValue(CurrentWindowBorderBrushProperty, value);
    }
    
    public FluentWindow()
    {
        TransparencyLevelHint =
        [
            WindowTransparencyLevel.AcrylicBlur,
            WindowTransparencyLevel.Mica,
            WindowTransparencyLevel.Blur,
        ];
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _windowBorder = e.NameScope.RequireInternal<Border>(PART_WindowBorder);
        _captionBar = e.NameScope.RequireInternal<Grid>(PART_CaptionBar);
        _captionButtons = e.NameScope.RequireInternal<FluentCaptionButtons>(PART_CaptionButtons);        
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

        _preMaximizeWindowBorderThickness = WindowBorderThickness;
        _preMaximizeWindowState = WindowState;

        if (EnableCustomWindowBorder && OperatingSystem.IsWindows())
        {
            if (IsDwmCompositionEnabled())
            {
                var handle = TryGetPlatformHandle()?.Handle;

                if (handle != null)
                {
                    DwmApi.DwmSetWindowAttribute(
                        handle.Value,
                        DwmApi.DWMWINDOWATTRIBUTE.DWMWA_BORDER_COLOR,
                        DwmApi.DWMWA_COLOR_NONE
                    );
                }
            }
        }
        
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

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        switch (change.Property.Name)
        {
            case nameof(EnableCustomWindowBorder):
            {
                var validBorderThickness = new Thickness(1);
                if (Application.Current?.TryGetResource("WindowBorderThickness", out var defaultThickness) ?? false)
                {
                    validBorderThickness = (defaultThickness as Thickness?) ?? new Thickness(1);
                }
                
                WindowBorderThickness = EnableCustomWindowBorder ? validBorderThickness : new Thickness(0);
                break;
            }

            case nameof(IsActive):
            {
                if (EnableCustomWindowBorder)
                {
                    CurrentWindowBorderBrush = IsActive
                        ? ActiveWindowBorderBrush
                        : InactiveWindowBorderBrush;

                    CurrentCaptionBorderBrush = ConstructCaptionBorderBrush();
                }

                break;
            }

            case nameof(WindowState):
            {
                if (WindowState is WindowState.FullScreen or WindowState.Maximized)
                {
                    _preMaximizeWindowState = ((WindowState)change.OldValue!);
                    _preMaximizeWindowBorderThickness = WindowBorderThickness;
                    
                    if (EnableCustomWindowBorder)
                    {
                        WindowBorderThickness = new Thickness(0);
                    }
                }
                else if (WindowState is WindowState.Normal)
                {
                    if (_preMaximizeWindowBorderThickness != null)
                    {
                        WindowBorderThickness = _preMaximizeWindowBorderThickness;
                        _preMaximizeWindowBorderThickness = null;
                    }
                }

                break;
            }
        }

        base.OnPropertyChanged(change);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (!CanClose)
        {
            e.Cancel = true;
        }
        
        base.OnClosing(e);
    }

    protected virtual IBrush ConstructCaptionBorderBrush()
    {
        var newCaptionBorderBrush = new LinearGradientBrush();
        
        if (CurrentWindowBorderBrush is ISolidColorBrush brush)
        {
            var accent = Color.FromArgb(
                0,
                brush.Color.R,
                brush.Color.G,
                brush.Color.B
            );
            
            newCaptionBorderBrush.GradientStops.Add(new(accent, 0.0));
            newCaptionBorderBrush.GradientStops.Add(new(brush.Color, 0.5) );
            newCaptionBorderBrush.GradientStops.Add(new(accent, 0.998));
        }
        else
        {
            newCaptionBorderBrush.GradientStops.Add(new(Colors.Transparent, 0.5));
            newCaptionBorderBrush.GradientStops.Add(new(Color.FromArgb(85, 255, 255, 255), 0.5));
            newCaptionBorderBrush.GradientStops.Add(new(Colors.Transparent, 0.998));
        }
        
        return newCaptionBorderBrush;
    }

    protected virtual Thickness? ProvideCaptionContentSafeAreaMargin()
        => CaptionContentSafeAreaMargin;

    [SupportedOSPlatform("windows")]
    protected bool IsDwmCompositionEnabled()
        => DwmApi.DwmIsCompositionEnabled();

    private void CaptionButtons_LayoutUpdated(object? sender, EventArgs e)
    {
        InternalCaptionContentSafeAreaMargin = ProvideCaptionContentSafeAreaMargin() ?? new Thickness(
            0, 0, ((FluentCaptionButtons)sender!).Bounds.Width, 0
        );
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