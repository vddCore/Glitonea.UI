namespace Glitonea.UI.Controls;

using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

[TemplatePart(PART_CloseButton, typeof(Button))]
[TemplatePart(PART_MaximizeButton, typeof(Button))]
[TemplatePart(PART_MinimizeButton, typeof(Button))]
[TemplatePart(PART_FullScreenButton, typeof(Button))]
[PseudoClasses(":minimized", ":normal", ":maximized", ":fullscreen")]
public class FluentCaptionButtons : TemplatedControl
{
    private const string PART_CloseButton = "PART_CloseButton";
    private const string PART_MaximizeButton = "PART_MaximizeButton";
    private const string PART_MinimizeButton = "PART_MinimizeButton";
    private const string PART_FullScreenButton = "PART_FullScreenButton";

    private IDisposable? _disposables;
    private WindowState _preFullScreenState;
    private Button? _fullScreenButton;
    private Button? _minimizeButton;
    private Button? _maximizeButton;
    private Button? _closeButton;

    protected override Type StyleKeyOverride => typeof(FluentCaptionButtons);

    public static readonly StyledProperty<IBrush?> FullScreenButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(FullScreenButtonBackground)
        );

    public static readonly StyledProperty<IBrush?> MinimizeButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(MinimizeButtonBackground)
        );

    public static readonly StyledProperty<IBrush?> MaximizeButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(MaximizeButtonBackground)
        );

    public static readonly StyledProperty<IBrush?> CloseButtonBackgroundProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(CloseButtonBackground)
        );

    public static readonly StyledProperty<bool> ShowFullScreenButtonProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, bool>(
            nameof(ShowFullScreenButton)
        );

    public static readonly StyledProperty<bool> ShowMinimizeButtonProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, bool>(
            nameof(ShowMinimizeButton), true
        );

    public static readonly StyledProperty<bool> ShowMaximizeButtonProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, bool>(
            nameof(ShowMaximizeButton), true
        );

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, bool>(
            nameof(ShowMaximizeButton), true
        );

    protected FluentWindow? HostWindow { get; set; }

    public IBrush? FullScreenButtonBackground
    {
        get => GetValue(FullScreenButtonBackgroundProperty);
        set => SetValue(FullScreenButtonBackgroundProperty, value);
    }

    public IBrush? MinimizeButtonBackground
    {
        get => GetValue(MinimizeButtonBackgroundProperty);
        set => SetValue(MinimizeButtonBackgroundProperty, value);
    }

    public IBrush? MaximizeButtonBackground
    {
        get => GetValue(MaximizeButtonBackgroundProperty);
        set => SetValue(MaximizeButtonBackgroundProperty, value);
    }

    public IBrush? CloseButtonBackground
    {
        get => GetValue(CloseButtonBackgroundProperty);
        set => SetValue(CloseButtonBackgroundProperty, value);
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

    public virtual void Attach(FluentWindow hostWindow)
    {
        if (_disposables == null)
        {
            HostWindow = hostWindow;

            _disposables = new CompositeDisposable(
            [
                HostWindow.GetObservable(FluentWindow.CanFullScreenProperty).Subscribe(x =>
                {
                    if (_fullScreenButton != null)
                        _fullScreenButton.IsEnabled = x;
                }),
                
                HostWindow.GetObservable(FluentWindow.CanMinimizeProperty).Subscribe(x =>
                {
                    if (_minimizeButton != null)
                        _minimizeButton.IsEnabled = x;
                }),
                HostWindow.GetObservable(FluentWindow.CanMaximizeProperty).Subscribe(x =>
                {
                    if (_maximizeButton != null)
                        _maximizeButton.IsEnabled = x;
                }),
                
                HostWindow.GetObservable(FluentWindow.CanCloseProperty).Subscribe(x =>
                {
                   if (_closeButton != null)
                       _closeButton.IsEnabled = x;
                }),
                
                HostWindow.GetObservable(Window.WindowStateProperty).Subscribe(x =>
                {
                    PseudoClasses.Set(":minimized", x == WindowState.Minimized);
                    PseudoClasses.Set(":normal", x == WindowState.Normal);
                    PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                    PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                }),
            ]);
        }
    }
    
    public virtual void Detach()
    {
        if (_disposables != null)
        {
            _disposables.Dispose();
            _disposables = null;

            HostWindow = null;
        }
    }

    protected virtual void OnEnterFullScreen()
    {
        if (HostWindow == null)
            return;

        if (!HostWindow.CanFullScreen || HostWindow.WindowState == WindowState.FullScreen)
            return;
        
        _preFullScreenState = HostWindow.WindowState;
        HostWindow.WindowState = WindowState.FullScreen;
    }

    protected virtual void OnExitFullScreen()
    {
        if (HostWindow == null)
            return;

        if (HostWindow.WindowState != WindowState.FullScreen)
            return;

        HostWindow.WindowState = _preFullScreenState;
    }
    
    protected virtual void OnClose()
    {
        if (HostWindow == null)
            return;
        
        if (!HostWindow.CanClose) 
            return;
        
        HostWindow?.Close();
    }

    protected virtual void OnMinimize()
    {
        if (HostWindow == null)
            return;

        if (!HostWindow.CanMinimize)
            return;
        
        HostWindow.WindowState = WindowState.Minimized;
    }

    protected virtual void OnMaximize()
    {
        if (HostWindow != null)
        {
            HostWindow.WindowState = HostWindow.WindowState == WindowState.Maximized 
                ? WindowState.Normal 
                : WindowState.Maximized;
        }
    }
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _fullScreenButton = e.NameScope.RequireInternal<Button>(PART_FullScreenButton);
        _minimizeButton = e.NameScope.RequireInternal<Button>(PART_MinimizeButton);
        _maximizeButton = e.NameScope.RequireInternal<Button>(PART_MaximizeButton);
        _closeButton = e.NameScope.RequireInternal<Button>(PART_CloseButton);

        _fullScreenButton.Click += (_, _) =>
        {
            if (HostWindow != null)
            {
                if (HostWindow.WindowState != WindowState.FullScreen)
                {
                    OnEnterFullScreen();
                }
                else
                {
                    OnExitFullScreen();
                }
            }
        };
            
        _minimizeButton.Click += (_, _) => OnMinimize();
        _maximizeButton.Click += (_, _) => OnMaximize();
        _closeButton.Click += (_, _) => OnClose();
        
        
        _fullScreenButton.IsEnabled = HostWindow?.CanFullScreen ?? false;
        _minimizeButton.IsEnabled = HostWindow?.CanMinimize ?? false;
        _maximizeButton.IsEnabled = HostWindow?.CanMaximize ?? false;
        _closeButton.IsEnabled = HostWindow?.CanClose ?? false;
    }
}