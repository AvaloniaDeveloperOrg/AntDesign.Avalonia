﻿using AntDesign.Controls.Helpers;
using System;

namespace AntDesign.Controls;

[PseudoClasses(AntDesignPseudoNameHelpers.PC_Coloring)]
public class AntDesignMenuItem : TreeViewItem
{
    static AntDesignMenuItem()
    {
        IsColorProperty.Changed.AddClassHandler<AntDesignMenuItem, bool>((s, e) =>
        {
            s.UpdatePseudoClasses();
        });

        IsMenuOpenProperty.Changed.AddClassHandler<AntDesignMenuItem, bool>((s, e) =>
        {
            s.PopupShowCore(e.NewValue.Value);
        });
    }

    public AntDesignMenuItem()
    {
         
    }

    bool _isColor = false;
    bool _isPanelClosing = false;
    bool _isMenuOpen = false;
    protected Control? _header;
    protected AntDesignMenu? _antDesignMenu;
    protected Popup? _popup;
    protected Menu? _menu;
 
    public static readonly DirectProperty<AntDesignMenuItem, bool> IsColorProperty =
           AvaloniaProperty.RegisterDirect<AntDesignMenuItem, bool>(nameof(IsColor), b => b.IsColor);

    public static readonly DirectProperty<AntDesignMenuItem, bool> IsPanelClosingProperty =
           AvaloniaProperty.RegisterDirect<AntDesignMenuItem, bool>(nameof(IsPanelClosing), b => b.IsPanelClosing);

    public static readonly DirectProperty<AntDesignMenuItem, bool> IsMenuOpenProperty =
           AvaloniaProperty.RegisterDirect<AntDesignMenuItem, bool>(nameof(IsMenuOpen), b => b.IsMenuOpen);

    public static readonly StyledProperty<object?> PopupContentProperty =
           AvaloniaProperty.Register<AntDesignMenuItem, object?>(nameof(PopupContent));

    public static readonly StyledProperty<IDataTemplate?> PopupContentTemplateProperty =
           AvaloniaProperty.Register<AntDesignMenuItem, IDataTemplate?>(nameof(PopupContentTemplate));


    [DependsOn(nameof(PopupContentTemplate))]
    public object? PopupContent
    {
        get { return GetValue(PopupContentProperty); }
        set { SetValue(PopupContentProperty, value); }
    }

    /// <summary>
    /// Gets or sets the data template used to display the content of the control.
    /// </summary>
    public IDataTemplate? PopupContentTemplate
    {
        get { return GetValue(PopupContentTemplateProperty); }
        set { SetValue(PopupContentTemplateProperty, value); }
    }


    public bool IsColor
    {
        get => _isColor;
        internal set => SetAndRaise(IsColorProperty, ref _isColor, value);
    }

    public bool IsPanelClosing
    {
        get => _isPanelClosing;
        internal set => SetAndRaise(IsColorProperty, ref _isPanelClosing, value);
    }

    public bool IsMenuOpen
    {
        get => _isMenuOpen;
        internal set => SetAndRaise(IsMenuOpenProperty, ref _isMenuOpen, value);
    }

    protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnAttachedToLogicalTree(e);
        _antDesignMenu = this.GetLogicalAncestors().OfType<AntDesignMenu>().FirstOrDefault();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_header is not null)
        {
            _header.PointerPressed -= Header_PointerPressed;
            _header = null;
        }

        _header = e.NameScope.Find<Control>("PART_Header");
        if (_header is not null)
            _header.PointerPressed += Header_PointerPressed;

        _popup = e.NameScope.Find<Popup>("PART_Popup");
 
        UpdatePseudoClasses();
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);

        if (_header is not null)
            _header.PointerPressed -= Header_PointerPressed;

        IsMenuOpen = false;
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
    }

    protected override void OnHeaderDoubleTapped(TappedEventArgs e)
    {
        if (_antDesignMenu is not null)
        {
            if (!_antDesignMenu.IsPanelExpanded)
                return;
        }

        base.OnHeaderDoubleTapped(e);
    }

    private void Header_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (ItemCount > 0)
        {
            if (_antDesignMenu is not null)
            {
                if (!_antDesignMenu.IsPanelExpanded)
                    return;
            }

            IsExpanded = !IsExpanded;
        }
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
    }

    void UpdatePseudoClasses()
    {
        PseudoClasses.Set(AntDesignPseudoNameHelpers.PC_Coloring, IsColor);
    }

    void PopupShowCore(bool isOpen)
    {
        if (_popup is null)
            return;

        if (_popup.IsOpen && !isOpen)
            _popup.IsOpen = false;

        if (_antDesignMenu?.IsPanelExpanded == true)
            return;

        if (_menu is null)
        {
            _menu = new Menu()
            {
                //Theme = 
                ItemsPanel = new FuncTemplate<Panel?>(() => new StackPanel() { Orientation = Orientation.Vertical }),
            };

            _menu.Items.Add(new MenuItem() { Header = "1231233" });
            _menu.Items.Add(new MenuItem() { Header = "1231233" });
            _menu.Items.Add(new MenuItem() { Header = "1231233" });
            _menu.Items.Add(new MenuItem() { Header = "1231233" });
            _menu.Items.Add(new MenuItem() { Header = "1231233" });
            _menu.Items.Add(new MenuItem() { Header = "1231233" });
            _menu.Items.Add(new MenuItem() { Header = "1231233" });

            PopupContent = _menu;
        }


        _popup.IsOpen = isOpen;
    }
}
