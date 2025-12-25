using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ComputerLock;

public static class BreathingLightHelper
{
    public static void InitializeBreathingLight(UIElement element)
    {
        element.Visibility = Visibility.Visible;
        StartBreathingAnimation(element);
    }

    private static void StartBreathingAnimation(UIElement element)
    {
        var animation = new Storyboard();

        var opacityAnimation = new DoubleAnimation
        {
            From = 0.1,
            To = 0.5,
            Duration = TimeSpan.FromSeconds(1),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };

        Storyboard.SetTarget(opacityAnimation, element);
        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

        animation.Children.Add(opacityAnimation);
        animation.Begin();
    }
}
