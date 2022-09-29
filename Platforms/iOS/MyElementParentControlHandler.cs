using System;
using CoreGraphics;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace ElementVisuaElementHotReload.Platforms
{
    //TODO
	public class MyElementParentControlHandler: ViewHandler<MyElementParentControl, UIView> {
        public static IPropertyMapper<MyElementParentControl, MyElementParentControlHandler> PropertyMapper = new PropertyMapper<MyElementParentControl, MyElementParentControlHandler>(ViewHandler.ViewMapper) {
            [nameof(MyElementParentControl.Items)] = MapItems
        };

        public static void MapItems(MyElementParentControlHandler handler, MyElementParentControl myElementParentControl) {
            handler.UpdateItems(myElementParentControl.Items);
        }

        UICollectionView collectionView;

        public MyElementParentControlHandler() : base(PropertyMapper, null) { }

        void UpdateItems(IList<MyNestedElementItem> items) {
            foreach (var subview in collectionView.Subviews)
                subview.RemoveFromSuperview();

            if (items == null)
                return;

            foreach(var item in items) {
                AddTextItem(item);
            }

            collectionView.InvalidateMeasure(VirtualView);
        }

        void AddTextItem(MyNestedElementItem item) {
            MauiTextView textView = new MauiTextView();
            textView.Text = item.Text;
            collectionView.Add(textView);
            textView.InvalidateMeasure(VirtualView);
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

