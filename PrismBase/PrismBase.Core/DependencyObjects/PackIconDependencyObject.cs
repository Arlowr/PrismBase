using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using MahApps.Metro.IconPacks;

namespace PrismBase.Core.DependencyObjects
{
    public class PackIconDependencyObject : DependencyObject
    {
        public static readonly DependencyProperty SpinProperty = DependencyProperty.RegisterAttached(
            "Spin", typeof(bool), typeof(PackIconDependencyObject), new PropertyMetadata(false, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is PackIconControlBase packIcon)
            {
                var startSpinning = (bool)e.NewValue;

                packIcon.Spin = startSpinning;


            }
        }
        public static void SetSpin(DependencyObject element, bool value)
        {
            element.SetValue(SpinProperty, value);
        }

        public static bool GetSpin(DependencyObject element)
        {
            return (bool)element.GetValue(SpinProperty);
        }

        public static readonly DependencyProperty BeginIfProperty = DependencyProperty.RegisterAttached(
            "BeginIf", typeof(bool), typeof(PackIconDependencyObject), new PropertyMetadata(true, BeginIfPropertyChangesCallback));

        public static void SetBeginIf(System.Windows.DependencyObject element, bool value)
        {
            element.SetValue(BeginIfProperty, value);
        }

        public static bool GetBeginIf(System.Windows.DependencyObject element)
        {
            return (bool)element.GetValue(BeginIfProperty);
        }

        private static void BeginIfPropertyChangesCallback(System.Windows.DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var storyboard = d as Storyboard;

            if (storyboard == null)
                throw new InvalidOperationException("This attached property only supports Storyboards.");

            var begin = (bool)e.NewValue;

            if (begin) storyboard.Begin();
            else storyboard.Stop();
        }
    }
}
