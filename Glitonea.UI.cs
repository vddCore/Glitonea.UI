namespace Glitonea.UI;

using Avalonia.Markup.Xaml;
using Avalonia.Styling;

public class GlitoneaUI : Styles
{
    public GlitoneaUI()
    {
        AvaloniaXamlLoader.Load(this);
    }
}