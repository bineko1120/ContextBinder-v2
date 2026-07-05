using ContextBinder.Models;
using ContextBinder.Services;

namespace ContextBinder.Forms;

public sealed class ItemEditForm : Form
{
    private readonly FileTypeDetector _fileTypeDetector;
    private readonly ComboBox _typeComboBox = new();
    private readonly TextBox _titleTextBox = new();
    private readonly TextBox _pathOrUrlTextBox = new();
    private readonly TextBox _templateTextBox = new();

    public ItemEditForm(BinderItem item, FileTypeDetector fileTypeDetector, bool allowTypeChange)
    {
        Item = item;
        _fileTypeDetector = fileTypeDetector;

        Text = "項目編集";
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(560, 420);
        Size = new Size(640, 500);
        Font = SystemFonts.MessageBoxFont;

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(14),
            RowCount = 6,
            ColumnCount = 2
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 12));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

        _typeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeComboBox.DataSource = Enum.GetValues<BinderItemType>();
        _typeComboBox.Format += (_, e) =>
        {
            if (e.ListItem is BinderItemType type)
            {
                e.Value = _fileTypeDetector.GetDisplayName(type);
            }
        };
        _typeComboBox.SelectedItem = item.Type;
        _typeComboBox.Enabled = allowTypeChange;
        _typeComboBox.SelectedIndexChanged += (_, _) => UpdateFieldAvailability();

        _titleTextBox.Text = item.Title;
        _pathOrUrlTextBox.Text = item.PathOrUrl;
        _templateTextBox.Multiline = true;
        _templateTextBox.ScrollBars = ScrollBars.Vertical;
        _templateTextBox.Text = item.TemplateText;

        AddLabeledControl(root, "種類", _typeComboBox, 0);
        AddLabeledControl(root, "タイトル", _titleTextBox, 1);
        AddLabeledControl(root, "参照先", _pathOrUrlTextBox, 2);
        AddLabeledControl(root, "テンプレート文", _templateTextBox, 3);

        FlowLayoutPanel buttonPanel = new()
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill
        };

        Button okButton = new()
        {
            Text = "保存",
            Width = 88,
            DialogResult = DialogResult.OK
        };
        okButton.Click += SaveButton_Click;

        Button cancelButton = new()
        {
            Text = "キャンセル",
            Width = 88,
            DialogResult = DialogResult.Cancel
        };

        buttonPanel.Controls.AddRange([cancelButton, okButton]);
        root.Controls.Add(buttonPanel, 0, 5);
        root.SetColumnSpan(buttonPanel, 2);

        Controls.Add(root);
        AcceptButton = okButton;
        CancelButton = cancelButton;
        UpdateFieldAvailability();
    }

    public BinderItem Item { get; }

    private static void AddLabeledControl(TableLayoutPanel root, string labelText, Control control, int row)
    {
        Label label = new()
        {
            AutoSize = true,
            Text = labelText,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };

        control.Dock = DockStyle.Fill;
        root.Controls.Add(label, 0, row);
        root.Controls.Add(control, 1, row);
    }

    private void UpdateFieldAvailability()
    {
        BinderItemType selectedType = (BinderItemType)_typeComboBox.SelectedItem!;
        bool isTemplate = selectedType == BinderItemType.Template;
        _pathOrUrlTextBox.Enabled = !isTemplate;
        _templateTextBox.Enabled = isTemplate;
    }

    private void SaveButton_Click(object? sender, EventArgs e)
    {
        BinderItemType selectedType = (BinderItemType)_typeComboBox.SelectedItem!;
        string title = _titleTextBox.Text.Trim();
        string pathOrUrl = _pathOrUrlTextBox.Text.Trim();
        string templateText = _templateTextBox.Text;

        if (string.IsNullOrWhiteSpace(title))
        {
            title = _fileTypeDetector.CreateDefaultTitle(selectedType, pathOrUrl, templateText);
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            MessageBox.Show(this, "タイトルを入力してください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        if (selectedType == BinderItemType.Template)
        {
            if (string.IsNullOrWhiteSpace(templateText))
            {
                MessageBox.Show(this, "テンプレート文を入力してください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                DialogResult = DialogResult.None;
                return;
            }
        }
        else if (string.IsNullOrWhiteSpace(pathOrUrl))
        {
            MessageBox.Show(this, "参照先を入力してください。", "入力確認", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            DialogResult = DialogResult.None;
            return;
        }

        Item.Type = selectedType;
        Item.Title = title;
        Item.PathOrUrl = selectedType == BinderItemType.Template ? string.Empty : pathOrUrl;
        Item.TemplateText = selectedType == BinderItemType.Template ? templateText : string.Empty;
        Item.UpdatedAt = DateTimeOffset.Now;
    }
}
