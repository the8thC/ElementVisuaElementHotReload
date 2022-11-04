using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

using AView = Android.Views.View;
using AApplication = Android.App.Application;

namespace ElementVisuaElementHotReload.Platforms
{
	public class MyVisualElementParentControlHandler: ViewHandler<MyVisualElementParentControl, AView> {
        const int TextHeight = 60;

        public static IPropertyMapper<MyVisualElementParentControl, MyVisualElementParentControlHandler> PropertyMapper = new PropertyMapper<MyVisualElementParentControl, MyVisualElementParentControlHandler>(ViewHandler.ViewMapper) {
            [nameof(MyVisualElementParentControl.Items)] = MapItems
        };

        public static void MapItems(MyVisualElementParentControlHandler handler, MyVisualElementParentControl myVisualElementParentControl) {
            handler.UpdateItems(myVisualElementParentControl.Items);
        }

        MauiViewGroup mauiViewGroup;
        Dictionary<MyNestedVisualElementItem, MauiTextView> nestedElementTextViewPairs;

        public MyVisualElementParentControlHandler() : base(PropertyMapper, null) { }

        void UpdateItems(IList<MyNestedVisualElementItem> items) {
            mauiViewGroup.RemoveAllViews();
            nestedElementTextViewPairs = new Dictionary<MyNestedVisualElementItem, MauiTextView>(items.Count);

            if (items == null)
                return;

            for (int i = 0; i < items.Count; i++) {
                int topIndent = i * TextHeight;
                var textView = AddTextItem(items[i], topIndent);
                nestedElementTextViewPairs[items[i]] = textView;
                items[i].OnTextChanged += (sender, oldText, newText) => {
                    MyNestedVisualElementItem item = sender as MyNestedVisualElementItem;
                    MauiTextView textView = nestedElementTextViewPairs[item];
                    textView.Text = newText;
                    textView.Layout(0, topIndent, 1000, topIndent + TextHeight);
                };
            }

            int height = (int)VirtualView.HeightRequest;
            int width = (int)VirtualView.WidthRequest;
            mauiViewGroup.Layout(0, 0, width, height);
        }

        MauiTextView AddTextItem(MyNestedVisualElementItem item, int topIndent) {
            MauiTextView textView = new MauiTextView(AApplication.Context);
            textView.Text = item.Text;
            mauiViewGroup.AddView(textView);
            textView.Layout(0, topIndent, 1000, topIndent + TextHeight);

            return textView;
        }

        protected override AView CreatePlatformView() {
            return mauiViewGroup = new MauiViewGroup(AApplication.Context);
        }

        protected override void ConnectHandler(AView platformView) {
            base.ConnectHandler(platformView);
            UpdateItems(VirtualView.Items);
            VirtualView.Items.CollectionChanged += (collection, args) => UpdateItems((IList<MyNestedVisualElementItem>)collection);
        }

        protected override void DisconnectHandler(AView platformView) {
            base.DisconnectHandler(platformView);
        }
	}

}

