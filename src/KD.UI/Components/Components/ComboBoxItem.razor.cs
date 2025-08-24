using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Utilities;
using MudExtensions;
using Color = MudBlazor.Color;
using Size = MudBlazor.Size;

namespace KD.UI.Components.Components;

public partial class ComboBoxItem<T> : MudComponentBase, IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    protected string? Classname => new CssBuilder("mud-combobox-item")
        .AddClass($"mud-combobox-item-{MudComboBox?.Dense.ToDescriptionString()}")
        .AddClass("mud-ripple", Ripple && !Disabled)
        .AddClass("mud-combobox-item-gutters")
        .AddClass("mud-combobox-item-clickable")
        .AddClass("mud-combobox-item-hilight", Active && !Disabled)
        .AddClass("mud-combobox-item-hilight-selected", Active && Selected && !Disabled)
        .AddClass($"mud-selected-item mud-{MudComboBox?.Color.ToDescriptionString()}-text mud-{MudComboBox?.Color.ToDescriptionString()}-hover", Selected && !Disabled && !Active)
        .AddClass("mud-combobox-item-disabled", Disabled)
        .AddClass("mud-combobox-item-bordered", MudComboBox?.Bordered == true && Active)
        .AddClass($"mud-combobox-item-bordered-{MudComboBox?.Color.ToDescriptionString()}", MudComboBox?.Bordered == true && Selected)
        .AddClass("d-none", !Eligible)
        .AddClass(Class)
        .Build();

    /// <summary>
    /// 
    /// </summary>
    protected string? HighlighterClassname => MudComboBox is null ? null : new CssBuilder()
        .AddClass("mud-combobox-highlighter", MudComboBox.Highlight && string.IsNullOrWhiteSpace(MudComboBox.HighlightClass))
        .AddClass(MudComboBox?.HighlightClass, MudComboBox?.Highlight)
        .Build();

    internal string ItemId { get; } = string.Concat("_", Guid.NewGuid().ToString().AsSpan(0, 8));

    /// <summary>
    /// The parent select component
    /// </summary>
    [CascadingParameter]
    ComboBox<T> MudComboBox { get; set; }

    /// <summary>
    /// Prevents the user from interacting with this item.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.General.Behavior)]
    public bool Disabled { get; set; }

    /// <summary>
    /// Shows a ripple effect when the user clicks the button.
    /// </summary>
    /// <remarks>
    /// Defaults to <c>true</c>.
    /// </remarks>
    [Parameter]
    [Category(CategoryTypes.General.Appearance)]
    public bool Ripple { get; set; } = true;

    /// <summary>
    /// The text to display
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.List.Behavior)]
    public string? Text { get; set; }

    /// <summary>
    /// A user-defined option that can be selected
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public T? Value { get; set; }

    /// <summary>
    /// The content within this item.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.General.Behavior)]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The color of the text. It supports the theme colors.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.List.Behavior)]
    public Color? TextColor { get; set; } = null;

    /// <summary>
    /// The color of the checked checkbox. It supports the theme colors.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.List.Behavior)]
    public Color? CheckBoxCheckedColor { get; set; } = null;

    /// <summary>
    /// The color of the unchecked checkbox. It supports the theme colors.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.Radio.Appearance)]
    public Color? CheckBoxUnCheckedColor { get; set; } = null;

    /// <summary>
    /// The size of the checkbox.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Appearance)]
    public Size? CheckBoxSize { get; set; } = null;

    /// <summary>
    /// OnClick event.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// 
    /// </summary>
    protected internal bool Selected { get; set; }
    /// <summary>
    /// 
    /// </summary>
    protected internal bool Active { get; set; }

    /// <summary>
    /// Change the item's active status.
    /// </summary>
    /// <param name="isActive"></param>
    public void SetActive(bool isActive)
    {
        Active = isActive;
        //StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    [Parameter]
    public bool Eligible { get; set; } = true;

    /// <summary>
    /// 
    /// </summary>
    protected string? DisplayString
    {
        get
        {
            var converter = MudComboBox?.Converter;
            if (MudComboBox?.ItemPresenter == ValuePresenter.None)
            {
                if (converter == null)
                    return Value?.ToString();
                return converter.Set(Value);
            }

            if (converter == null)
                return $"{(string.IsNullOrWhiteSpace(Text) ? Value : Text)}";
            return !string.IsNullOrWhiteSpace(Text) ? Text : converter.Set(Value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ForceRender()
    {
        CheckEligible();
        StateHasChanged();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task ForceUpdate()
    {
        SyncSelected();
        await InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnInitialized()
    {
        MudComboBox?.AddItem(this);
    }

    //bool? _oldMultiselection;
    //bool? _oldSelected;
    bool _selectedChanged = false;
    //bool? _oldEligible = true;
    //bool _eligibleChanged = false;
    /// <summary>
    /// 
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        //SyncSelected();
        //if (_oldSelected != Selected || _oldEligible != Eligible)
        //{
        //    _selectedChanged = true;
        //}
        //_oldSelected = Selected;
        CheckEligible();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="firstRender"></param>
    /// <returns></returns>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);
        if (_selectedChanged)
        {
            _selectedChanged = false;
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    protected internal void CheckEligible()
    {
        Eligible = IsEligible();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected bool IsEligible()
    {
        if (MudComboBox is null)
            return true;

        if (!MudComboBox.Editable || MudComboBox.EnableFilter == false)
            return true;

        if (string.IsNullOrWhiteSpace(MudComboBox._searchString))
            return true;

        if (MudComboBox.SearchFunc is not null)
            return MudComboBox.SearchFunc.Invoke(Value, Text, MudComboBox.GetSearchString());

        if (!string.IsNullOrWhiteSpace(Text))
        {
            if (Text.Contains(MudComboBox._searchString ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        else
        {
            if (MudComboBox?.Converter?.Set(Value)?.Contains(MudComboBox._searchString ?? string.Empty, StringComparison.OrdinalIgnoreCase) == true)
                return true;
        }

        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    protected void SyncSelected()
    {
        if (MudComboBox is null)
            return;

        if (MudComboBox.MultiSelection && MudComboBox?.SelectedValues?.Contains(Value) == true)
            Selected = true;

        else if (MudComboBox?.MultiSelection == false && ((MudComboBox.Value is null && Value is null) || MudComboBox.Value?.Equals(Value) == true))
            Selected = true;
        else
            Selected = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected async Task HandleOnClick()
    {
        await MudComboBox.ToggleOption(this, !Selected);
        await InvokeAsync(StateHasChanged);
        await MudComboBox.FocusAsync();
        await OnClick.InvokeAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        try
        {
            MudComboBox?.RemoveItem(this);
        }
        catch { }
    }
}