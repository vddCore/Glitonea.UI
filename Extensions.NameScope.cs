namespace Glitonea.UI;

using Avalonia.Controls;

public static partial class Extensions
{
    internal static T RequireInternal<T>(this INameScope nameScope, string componentName, bool internalCall = true)
        where T : Control
    {
        var component = nameScope.Find<T>(componentName);

        if (component == null)
        {
            throw new InvalidOperationException(
                $"Cannot find the critical template component '{componentName}'. " +
                (internalCall ? "Forgor to add <GlitoneaUI /> to your application styles?" : string.Empty)
            );
        }

        return component;
    }

    public static T Require<T>(this INameScope nameScope, string componentName)
        where T : Control
    {
        return RequireInternal<T>(nameScope, componentName, false);
    }
}