using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ElementVisuaElementHotReload;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}
}

public class MyNestedElementItem : Element
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(MyNestedElementItem), string.Empty, propertyChanged: OnTextPropertyChanged);

    static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
        var item = bindable as MyNestedElementItem;
        string? oldText = oldValue as string;
        string? newText = newValue as string;
        if(item.OnTextChanged != null)
            item.OnTextChanged(item, (string)oldText, (string)newText);
    }

    public delegate void TextChanged(object sender, string? oldText, string? newText);

    public event TextChanged OnTextChanged;

    public string Text {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
}

[ContentProperty(nameof(Items))]
public class MyElementParentControl : View
{
    public ObservableCollection<MyNestedElementItem> Items { get; set; } = new ObservableCollection<MyNestedElementItem>();

    public MyElementParentControl() {
        Items.CollectionChanged += Items_CollectionChanged;
    }

    void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        if(e.OldItems != null)
            foreach (MyNestedElementItem oldItem in e.OldItems)
                oldItem.Parent = null;

        if(e.NewItems != null)
            foreach (MyNestedElementItem newItem in e.NewItems)
                newItem.Parent = this;
    }
}


public class MyNestedVisualElementItem : VisualElement, IVisualTreeElement
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(MyNestedVisualElementItem), propertyChanged: OnTextPropertyChanged);

    static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue) {
        var item = bindable as MyNestedVisualElementItem;
        string? oldText = oldValue as string;
        string? newText = newValue as string;
        if(item.OnTextChanged != null)
            item.OnTextChanged(item, oldText, newText);
    }

    public delegate void TextChanged(object sender, string? oldText, string? newText);

    public event TextChanged OnTextChanged;

    public string Text {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    //public IReadOnlyList<IVisualTreeElement> GetVisualChildren() {
    //    return (this as VisualElement).GetVisualTreeDescendants();
    //}

	public IVisualTreeElement? GetVisualParent() {
        return Parent;
    }
}

[ContentProperty(nameof(Items))]
public class MyVisualElementParentControl : View, IVisualTreeElement
{
    public ObservableCollection<MyNestedVisualElementItem> Items { get; set; } = new ObservableCollection<MyNestedVisualElementItem>();

    public MyVisualElementParentControl() {
        Items.CollectionChanged += Items_CollectionChanged;
    }

    public IReadOnlyList<IVisualTreeElement> GetVisualChildren() {
        return Items;
    }

	public IVisualTreeElement? GetVisualParent() {
        return Parent;
    }

    void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
        if(e.OldItems != null)
            foreach (MyNestedVisualElementItem oldItem in e.OldItems)
                oldItem.Parent = null;

        if(e.NewItems != null)
            foreach (MyNestedVisualElementItem newItem in e.NewItems)
                newItem.Parent = this;
    }
}