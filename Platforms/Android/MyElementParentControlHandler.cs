using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

using AView = Android.Views.View;
using AApplication = Android.App.Application;

namespace ElementVisuaElementHotReload.Platforms
{
	public class MyElementParentControlHandler: ViewHandler<MyElementParentControl, AView> {
        const int TextHeight = 60;

        public static IPropertyMapper<MyElementParentControl, MyElementParentControlHandler> PropertyMapper = new PropertyMapper<MyElementParentControl, MyElementParentControlHandler>(ViewHandler.ViewMapper) {
            [nameof(MyElementParentControl.Items)] = MapItems
        };

        public static void MapItems(MyElementParentControlHandler handler, MyElementParentControl myElementParentControl) {
            handler.UpdateItems(myElementParentControl.Items);
        }

        MauiViewGroup mauiViewGroup;
        Dictionary<MyNestedElementItem, MauiTextView> nestedElementTextViewPairs;

        public MyElementParentControlHandler() : base(PropertyMapper, null) { }

        void UpdateItems(IList<MyNestedElementItem> items) {
            mauiViewGroup.RemoveAllViews();
            nestedElementTextViewPairs = new Dictionary<MyNestedElementItem, MauiTextView>(items.Count);

            if (items == null)
                return;

            for(int i = 0; i < items.Count; i++) {
                int topIndent = i * TextHeight;
                var textView = AddTextItem(items[i], topIndent);
                nestedElementTextViewPairs[items[i]] = textView;
                items[i].OnTextChanged += (sender, oldText, newText) => {
                    MyNestedElementItem item = sender as MyNestedElementItem;
                    MauiTextView textView = nestedElementTextViewPairs[item];
                    textView.Text = newText;
                    textView.Layout(0, topIndent, 1000, topIndent + TextHeight);
                };
            }

            int height = (int)VirtualView.HeightRequest;
            int width = (int)VirtualView.WidthRequest;
            mauiViewGroup.Layout(0, 0, width, height);
        }

        MauiTextView AddTextItem(MyNestedElementItem item, int topIndent) {
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
        }

        protected override void DisconnectHandler(AView platformView) {
            base.DisconnectHandler(platformView);
        }
	}

}

