﻿using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Phone.Shell;

namespace Phone7.Fx.Preview
{
    public class BindableApplicationBarIconButton : FrameworkElement, IApplicationBarIconButton, IApplicationBarMenuItem
    {

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(BindableApplicationBarIconButton), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(BindableApplicationBarIconButton), null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly DependencyProperty CommandParameterValueProperty =
          DependencyProperty.RegisterAttached("CommandParameterValue", typeof(object), typeof(BindableApplicationBarMenuItem), null);

        public object CommandParameterValue
        {
            get { return GetValue(CommandParameterValueProperty); }
            set { SetValue(CommandParameterValueProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(BindableApplicationBarIconButton), new PropertyMetadata(true, OnEnabledChanged));

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableApplicationBarIconButton)d).Button.IsEnabled = (bool)e.NewValue;
            }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string), typeof(BindableApplicationBarIconButton), new PropertyMetadata(OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != e.OldValue)
            {
                ((BindableApplicationBarIconButton)d).Button.Text = e.NewValue.ToString();
            }
        }

        public ApplicationBarIconButton Button { get; set; }

        public BindableApplicationBarIconButton()
        {
            Button = new ApplicationBarIconButton();
            Button.Text = "Text";
            Button.Click += ApplicationBarIconButtonClick;
        }

        void ApplicationBarIconButtonClick(object sender, EventArgs e)
        {
            if (Command != null && CommandParameter != null)
                Command.Execute(CommandParameter);
            else if (Command != null)
                Command.Execute(CommandParameterValue);
        }

        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public event EventHandler Click;

        public Uri IconUri
        {
            get { return Button.IconUri; }
            set { Button.IconUri = value; }
        }
    }
}