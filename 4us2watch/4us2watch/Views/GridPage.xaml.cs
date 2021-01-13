﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace _4us2watch.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GridPage : ContentPage
    {
        ProfilePage profile = null;
        GridPage grid = null;
        public string email;
        public GridPage(string text)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
            _menuItemsView = new[] { (View)LabelSlikaTest, LabelTest, LabelSlikaDvaTest, LabelDvaTest };
            CreateAndFillGrid(MovieGrid);
            email = text;
        }
        private const string ExpandAnimationName = "ExpandAnimation";
        private const string CollapseAnimationName = "CollapseAnimation";
        private const double SlideAnimationDuration = 0.25;
        private const int AnimationDuration = 600;
        private const double PageScale = 0.9;
        private const double PageTranslation = 0.35;
        private readonly IEnumerable<View> _menuItemsView;
        private bool _isAnimationRun;
        private double _safeInsetsTop;

        private void CreateAndFillGrid(Grid grid) //Fix dis plis
        {

            grid.RowDefinitions.Add(new RowDefinition { Height = 200 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 200 });

            Frame frame = new Frame
            {
                BorderColor = Color.Black,
                HasShadow = true,
                Content = new Label { Text = "x", HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center }
            };
            grid.Children.Add(frame, 0, 0);
            //grid.Children.Add(frame, 0, 1);

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MenuTopRow.Height = MenuBottomRow.Height = Device.Info.ScaledScreenSize.Height * (1 - PageScale) / 2;
        }
        void HelpCommand(object sender, EventArgs args)
        {
            DisplayAlert("Dela", "Dela", "OK");
        }
        void LogOutCommand(object sender, EventArgs args)
        {
            DisplayAlert("Dela", "Dela", "OK");
        }
        void HomeCommand(object sender, EventArgs args)
        {
            if (grid == null)
            {
                grid = new GridPage(email);
            }
            App.Current.MainPage = new NavigationPage(grid);
        }
        void FriendsCommand(object sender, EventArgs args)
        {
            OnShowMenu();
        }
        void ProfileCommand(object sender, EventArgs args)
        {
            if (profile == null)
            {
                profile = new ProfilePage(email);
            }
            App.Current.MainPage = new NavigationPage(profile);
        }
        void MoviesCommand(object sender, EventArgs args)
        {
            DisplayAlert("Dela", "Dela", "OK");
        }
        void TVSeriesCommand(object sender, EventArgs args)
        {
            DisplayAlert("Dela", "Dela", "OK");
        }
        void DocumentariesCommand(object sender, EventArgs args)
        {
            DisplayAlert("Dela", "Dela", "OK");
        }
        void AnimeCommand(object sender, EventArgs args)
        {
            DisplayAlert("Dela", "Dela", "OK");
        }

        public async void OnShowMenu()
        {
            if (_isAnimationRun)
                return;

            _isAnimationRun = true;
            var animationDuration = AnimationDuration;
            if (Page.Scale < 1)
            {
                animationDuration = (int)(AnimationDuration * SlideAnimationDuration);
                GetCollapseAnimation().Commit(this, CollapseAnimationName, 16,
                    (uint)(AnimationDuration * SlideAnimationDuration),
                    Easing.Linear,
                    null, () => false);
            }
            else
            {
                GetExpandAnimation().Commit(this, ExpandAnimationName, 16,
                    AnimationDuration,
                    Easing.Linear,
                    null, () => false);
            }

            await Task.Delay(animationDuration);
            _isAnimationRun = false;
        }

        private Animation GetExpandAnimation()
        {
            var iconAnimationTime = (1 - SlideAnimationDuration) / _menuItemsView.Count();
            var animation = new Animation
            {
                {0, SlideAnimationDuration, new Animation(v => ToolbarSafeAreaRow.Height = v, _safeInsetsTop, 0)},
                {
                    0, SlideAnimationDuration,
                    new Animation(v => Page.TranslationX = v, 0, Device.Info.ScaledScreenSize.Width * PageTranslation)
                },
                {0, SlideAnimationDuration, new Animation(v => Page.Scale = v, 1, PageScale)},
                {
                    0, SlideAnimationDuration,
                    new Animation(v => Page.Margin = new Thickness(0, v, 0, 0), 0, _safeInsetsTop)
                },
                {0, SlideAnimationDuration, new Animation(v => Page.CornerRadius = (float) v, 0, 5)}
            };

            foreach (var view in _menuItemsView)
            {
                var index = _menuItemsView.IndexOf(view);
                animation.Add(SlideAnimationDuration + iconAnimationTime * index - 0.05,
                    SlideAnimationDuration + iconAnimationTime * (index + 1) - 0.05, new Animation(
                        v => view.Opacity = (float)v, 0, 1));
                animation.Add(SlideAnimationDuration + iconAnimationTime * index,
                    SlideAnimationDuration + iconAnimationTime * (index + 1), new Animation(
                        v => view.TranslationY = (float)v, -10, 0));
            }

            return animation;
        }

        private Animation GetCollapseAnimation()
        {
            var animation = new Animation
            {
                {0, 1, new Animation(v => ToolbarSafeAreaRow.Height = v, 0, _safeInsetsTop)},
                {0, 1, new Animation(v => Page.TranslationX = v, Device.Info.ScaledScreenSize.Width * PageTranslation, 0)},
                {0, 1, new Animation(v => Page.Scale = v, PageScale, 1)},
                {0, 1, new Animation(v => Page.Margin = new Thickness(0, v, 0, 0), _safeInsetsTop, 0)},
                {0, 1, new Animation(v => Page.CornerRadius = (float) v, 5, 0)}
            };

            foreach (var view in _menuItemsView)
            {
                animation.Add(0, 1, new Animation(
                    v => view.Opacity = (float)v, 1, 0));
                animation.Add(0, 1, new Animation(
                    v => view.TranslationY = (float)v, 0, -10));
            }

            return animation;
        }
    }
}