using System;
using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace ElementVisuaElementHotReload.Platforms
{
    //TODO
    public class MyVisualElementParentControlHandler: ViewHandler<MyVisualElementParentControl, UIView> {
        public static IPropertyMapper<MyVisualElementParentControl, MyVisualElementParentControlHandler> PropertyMapper = new PropertyMapper<MyVisualElementParentControl, MyVisualElementParentControlHandler>(ViewHandler.ViewMapper) {
            [nameof(MyVisualElementParentControl.Items)] = MapItems
        };

        public static void MapItems(MyVisualElementParentControlHandler handler, MyVisualElementParentControl myVisualElementParentControl) {
            handler.UpdateItems(myVisualElementParentControl.Items);
        }

        UICollectionView collectionView;

        public MyVisualElementParentControlHandler() : base(PropertyMapper, null) { }

        void UpdateItems(IList<MyNestedVisualElementItem> items) {
            foreach (var subview in collectionView.Subviews)
                subview.RemoveFromSuperview();

            if (items == null)
                return;

            foreach(var item in items) {
                AddTextItem(item);
            }
        }

        void AddTextItem(MyNestedVisualElementItem item) {
            MauiTextView textView = new MauiTextView();
            textView.Text = item.Text;
            collectionView.Add(textView);
        }

        protected override UIView CreatePlatformView() {
            return collectionView = new UICollectionView(new CGRect(0, 0, this.VirtualView.WidthRequest,
                                                                          this.VirtualView.HeightRequest),
                                                                          new UICollectionViewLayout());
        }

        protected override void ConnectHandler(UIView platformView) {
            base.ConnectHandler(platformView);
            UpdateItems(VirtualView.Items);
        }

        protected override void DisconnectHandler(UIView platformView) {
            base.DisconnectHandler(platformView);
        }
	}

}

