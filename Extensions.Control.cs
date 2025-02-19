namespace Glitonea.UI;

using Avalonia.Controls;

public static partial class Extensions
{
    public static T Require<T>(this Control control, string componentName)
        where T : Control
    {
        var component = control.Find<T>(componentName);

        if (component == null)
        {
            throw new InvalidOperationException(
                $"Cannot find the critical visual child '{componentName}'. "
            );
        }

        return component;
    }
}