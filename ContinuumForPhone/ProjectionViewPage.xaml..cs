using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ContinuumForPhone
{
    public sealed partial class ProjectionViewPage : Page
    {
        // デバイス側の view id
        int mainViewId;

        public ProjectionViewPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // (簡易的な方法)引数に入っている、アプリケーション画面のid を取得する
            mainViewId = (int)e.Parameter;
        }

        private async void StopProjection_Click(object sender, RoutedEventArgs e)
        {
            await ProjectionManager.StopProjectingAsync(
                 ApplicationView.GetForCurrentView().Id,
                 mainViewId);
        }
    }
}
