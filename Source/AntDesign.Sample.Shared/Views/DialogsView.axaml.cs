using AntDesign.Sample.ViewModels;
using Avalonia.Dialogs;
using Avalonia.Platform.Storage;
using Avalonia.Toolkit.Helpers;

namespace AntDesign.Sample.Views;
public partial class DialogsView : ReactiveUserControl<DialogsViewModel>
{
    public DialogsView()
    {
        InitializeComponent();
        //NumericUpDown.ButtonSpinnerLocationProperty

    }


    Window GetWindow() => TopLevel.GetTopLevel(this) as Window ?? throw new NullReferenceException("Invalid Owner");


    async void OpenFolderClick(object sender, RoutedEventArgs args)
    {
        await GetWindow().StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions() { Title = "ѡ���ļ���", AllowMultiple = true });
    }

    async void OpenFileDialog(object sender, RoutedEventArgs args)
    {

        if (OperatingSystemEx.IsAndroid() || OperatingSystemEx.IsBrowser() || OperatingSystemEx.IsIOS())
            await GetWindow().StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions() { Title = "ѡ���ļ�", AllowMultiple = true });
        else
        {
            await new OpenFileDialog()
            {
                Title = "ѡ���ļ�",
                AllowMultiple = true
            }.ShowManagedAsync(GetWindow(), new ManagedFileDialogOptions
            {
                AllowDirectorySelection = true
            });
        }
    }
}
