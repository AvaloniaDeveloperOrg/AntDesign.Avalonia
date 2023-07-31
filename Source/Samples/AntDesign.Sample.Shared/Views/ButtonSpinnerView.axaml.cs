using AntDesign.Sample.ViewModels;

namespace AntDesign.Sample.Views;
public partial class ButtonSpinnerView : ReactiveUserControl<ButtonSpinnerViewModel>
{
    public ButtonSpinnerView()
    {
        InitializeComponent();
        //PART_ButtonSpinner.Spin += PART_ButtonSpinner_Spin;
    }

    private void PART_ButtonSpinner_Spin(object? sender, SpinEventArgs e)
    {
        //LayoutTransformControl
        //LayoutTransformControl
        //ProgressBar.HorizontalAlignmentProperty
        switch (e.Direction)
        {
            case SpinDirection.Increase:
                PART_PinMessage.Text = $"�������ߵİ�ť";
                break;
            case SpinDirection.Decrease:
                PART_PinMessage.Text = $"������ұߵİ�ť";
                break;
            default:
                break;
        }
        

    }
}
