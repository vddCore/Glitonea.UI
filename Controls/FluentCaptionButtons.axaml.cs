namespace Glitonea.UI;

using Avalonia;
using Avalonia.Controls.Chrome;
using Avalonia.Media;

public class FluentCaptionButtons : CaptionButtons
{
    protected override Type StyleKeyOverride => typeof(FluentCaptionButtons);

    public static readonly StyledProperty<IBrush?> FullScreenButtonBackgroundBrushProperty = 
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(FullScreenButtonBackgroundBrush)
        );
    
    public static readonly StyledProperty<IBrush?> MinimizeButtonBackgroundBrushProperty = 
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(MinimizeButtonBackgroundBrush)
        );

    public static readonly StyledProperty<IBrush?> MaximizeButtonBackgroundBrushProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(MaximizeButtonBackgroundBrush)
        );

    public static readonly StyledProperty<IBrush?> CloseButtonBackgroundBrushProperty =
        AvaloniaProperty.Register<FluentCaptionButtons, IBrush?>(
            nameof(CloseButtonBackgroundBrush)
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

    public IBrush? FullScreenButtonBackgroundBrush
    {
        get => GetValue(FullScreenButtonBackgroundBrushProperty);
        set => SetValue(FullScreenButtonBackgroundBrushProperty, value);
    }

    public IBrush? MinimizeButtonBackgroundBrush
    {
        get => GetValue(MinimizeButtonBackgroundBrushProperty);
        set => SetValue(MinimizeButtonBackgroundBrushProperty, value);
    }

    public IBrush? MaximizeButtonBackgroundBrush
    {
        get => GetValue(MaximizeButtonBackgroundBrushProperty);
        set => SetValue(MaximizeButtonBackgroundBrushProperty, value);
    }

    public IBrush? CloseButtonBackgroundBrush
    {
        get => GetValue(CloseButtonBackgroundBrushProperty);
        set => SetValue(CloseButtonBackgroundBrushProperty, value);
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
}