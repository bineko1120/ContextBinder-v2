namespace ContextBinder.Forms;

public sealed class InputDialog : Form
{
    private readonly TextBox _textBox = new();

    private InputDialog(string title, string labelText, string initialValue)
    {
        Text = title;
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MinimizeBox = false;
        MaximizeBox = false;
        ClientSize = new Size(420, 140);
        Font = SystemFonts.MessageBoxFont;

        Label label = new()
        {
            AutoSize = true,
            Text = labelText,
            Location = new Point(16, 18)
        };

        _textBox.Location = new Point(16, 48);
        _textBox.Width = 388;
        _textBox.Text = initialValue;

        Button okButton = new()
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Location = new Point(238, 96),
            Width = 78
        };

        Button cancelButton = new()
        {
            Text = "キャンセル",
            DialogResult = DialogResult.Cancel,
            Location = new Point(326, 96),
            Width = 78
        };

        Controls.AddRange([label, _textBox, okButton, cancelButton]);
        AcceptButton = okButton;
        CancelButton = cancelButton;
    }

    public static string? Show(IWin32Window owner, string title, string labelText, string initialValue = "")
    {
        using InputDialog dialog = new(title, labelText, initialValue);
        return dialog.ShowDialog(owner) == DialogResult.OK ? dialog._textBox.Text.Trim() : null;
    }
}
