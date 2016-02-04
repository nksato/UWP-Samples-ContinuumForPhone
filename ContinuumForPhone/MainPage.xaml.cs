using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Popups;




// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 を参照してください

namespace ContinuumForPhone
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.SizeChanged += Current_SizeChanged;
        }


        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            switch (UIViewSettings.GetForCurrentView().UserInteractionMode)
            {
                case UserInteractionMode.Mouse:
                    // 外部ディスプレイ側
                    //VisualStateManager.GoToState(this, "MouseLayout", true);
                    break;
                case UserInteractionMode.Touch:
                    // モバイルデバイス側
                default:
                    // 規定値(モバイルデバイスを想定)
                    //VisualStateManager.GoToState(this, "TouchLayout", true);
                    break;
            }
        }

        private async void Projection_Click(object sender, RoutedEventArgs e)
        {
            // 外部ディスプレイが利用できる場合
            if (ProjectionManager.ProjectionDisplayAvailable)
            {
                // 現在のアプリケーション画面を取得する
                int thisViewId;
                thisViewId = ApplicationView.GetForCurrentView().Id;

                int newViewId = 0;

                // 新しいアプリケーション画面を作成する
                await CoreApplication.CreateNewView().Dispatcher.RunAsync(
                    CoreDispatcherPriority.Normal, () =>
                    {
                        var rootFrame = new Frame();
                        // 引数として、簡易的にアプリケーション画面の id を渡す。
                        rootFrame.Navigate(typeof(ProjectionViewPage), thisViewId);
                        Window.Current.Content = rootFrame;
                        Window.Current.Activate();
                        // 新しいアプリケーション画面を取得する
                        var newView = ApplicationView.GetForCurrentView();
                        newViewId = newView.Id;

                    });

                // StartProjectingAsync(projectionViewId, anchorViewId)
                // projectionViewId - 外部ディスプレイ側：newViewId
                // anchorViewId - アンカー側:thisViewId
                //  外部ディスプレイ側のスタートメニューからアプリを起動した場合、
                //  パラメータ projectionViewId に新しい view (ProjectionViewPage)が指定されているため、
                //  デバイス側側に MainPage,外部ディスプレイ側に ProjectionViewPage が表示される。 
                await ProjectionManager.StartProjectingAsync(newViewId, thisViewId);

            }
            else
            {
                var messageDialog = new MessageDialog("外部ディスプレイがありません");
                await messageDialog.ShowAsync();

            }

        }
    }
}
