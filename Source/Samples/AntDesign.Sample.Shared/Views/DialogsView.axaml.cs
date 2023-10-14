using AntDesign.Sample.ViewModels;
using Avalonia.Dialogs;
using Avalonia.Platform.Storage;
using AntDesign.Toolkit.Helpers;
using System.Text;

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

    async void SaveFileDialog(object sender, RoutedEventArgs args)
    {
        var storageFile = await GetWindow().StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions () { Title = "����λ��"});
        if (storageFile is null)
            return;

        using var stream = await storageFile!.OpenWriteAsync();
        var buffer = Encoding.UTF8.GetBytes("1231231231313");
        stream.Write(buffer);
    }
}
