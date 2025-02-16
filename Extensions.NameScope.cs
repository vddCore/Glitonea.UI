namespace Glitonea.UI;

using Avalonia.Controls;

internal static partial class Extensions
{
    internal static T Require<T>(this INameScope nameScope, string componentName)
        where T : Control
    {
        var component = nameScope.Find<T>(componentName);
        
        if (component == null)
        {
            throw new InvalidOperationException(
                $"Cannot find the critical template component '{componentName}'. " +
                "Forgor to include <GlitoneaUI/> in your application styles?"
            );
        }

        return component;
    }
}