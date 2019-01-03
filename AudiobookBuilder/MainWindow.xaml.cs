using AudiobookBuilder.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AudiobookBuilder
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FileItems_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void FileItems_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop) && DataContext is AudioBookBuilderViewModel vm)
            {                
                foreach (var file in ((IEnumerable<string>)e.Data.GetData(DataFormats.FileDrop)).OrderBy(o=>o))
                {
                    vm.FileItems.Add(new FileItem(file));
                }               
            }
        }
    }
}
