using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ComputerLock;
public static class BreathingLightHelper
{
    public static void InitializeBreathingLight(Border breathingLight)
    {
        breathingLight.Visibility = Visibility.Visible;
        StartBreathingAnimation(breathingLight);
    }

    private static void StartBreathingAnimation(Border breathingLight)
    {
        var breathingAnimation = new Storyboard();

        var opacityAnimation = new DoubleAnimation
        {
            From = 0.1,
            To = 0.5,
            Duration = TimeSpan.FromSeconds(1),
            AutoReverse = true,
            RepeatBehavior = RepeatBehavior.Forever
        };

        Storyboard.SetTarget(opacityAnimation, breathingLight);
        Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Border.OpacityProperty));

        breathingAnimation.Children.Add(opacityAnimation);
        breathingAnimation.Begin();
    }
}