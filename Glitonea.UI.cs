namespace Glitonea.UI;

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

public class GlitoneaUI : ResourceDictionary
{
    public GlitoneaUI()
    {
        AvaloniaXamlLoader.Load(this);
    }
}