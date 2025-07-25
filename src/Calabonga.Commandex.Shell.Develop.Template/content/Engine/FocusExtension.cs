﻿using System.Windows;

namespace Calabonga.Commandex.Shell.Develop.Engine;
/// <summary>
/// Focus for MVVM
/// </summary>
public static class FocusExtension
{
    public static bool GetIsFocused(DependencyObject obj)
    {
        return (bool)obj.GetValue(IsFocusedProperty);
    }

    public static void SetIsFocused(DependencyObject obj, bool value)
    {
        obj.SetValue(IsFocusedProperty, value);
    }

    public static readonly DependencyProperty IsFocusedProperty =
        DependencyProperty.RegisterAttached(
            "IsFocused", typeof(bool), typeof(FocusExtension),
            new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

    private static void OnIsFocusedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var uiElement = (UIElement)d;
        if ((bool)e.NewValue)
        {
            uiElement.Focus();
        }
    }
}
