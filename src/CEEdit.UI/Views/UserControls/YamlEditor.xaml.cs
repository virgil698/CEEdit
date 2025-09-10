using System;
using System.Windows;
using System.Windows.Controls;

namespace CEEdit.UI.Views.UserControls
{
    /// <summary>
    /// YamlEditor.xaml 的交互逻辑
    /// </summary>
    public partial class YamlEditor : UserControl
    {
        public YamlEditor()
        {
            InitializeComponent();
        }

        private void CodeEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && DataContext is ViewModels.YamlEditorViewModel viewModel)
            {
                // 获取当前光标位置
                int caretIndex = textBox.CaretIndex;
                string text = textBox.Text;
                
                // 计算行号和列号
                int line = 1;
                int column = 1;
                
                for (int i = 0; i < caretIndex && i < text.Length; i++)
                {
                    if (text[i] == '\n')
                    {
                        line++;
                        column = 1;
                    }
                    else
                    {
                        column++;
                    }
                }
                
                viewModel.UpdateCursorPosition(line, column);
            }
        }

        private void CodeEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && DataContext is ViewModels.YamlEditorViewModel viewModel)
            {
                // 更新行号
                UpdateLineNumbers(textBox.Text);
                
                // 触发内容变更事件
                viewModel.OnContentChanged();
            }
        }

        private void UpdateLineNumbers(string text)
        {
            if (LineNumbers == null) return;
            
            int lineCount = 1;
            foreach (char c in text)
            {
                if (c == '\n')
                    lineCount++;
            }
            
            var lineNumbers = new System.Text.StringBuilder();
            for (int i = 1; i <= lineCount; i++)
            {
                lineNumbers.AppendLine(i.ToString());
            }
            
            LineNumbers.Text = lineNumbers.ToString();
        }
    }
}
