using System.Runtime.InteropServices;
using ContextBinder.Models;
using ContextBinder.Services;

namespace ContextBinder.Forms;

public sealed class MainForm : Form
{
    private readonly FileTypeDetector _fileTypeDetector = new();
    private readonly ClipboardService _clipboardService = new();
    private readonly SearchService _searchService = new();
    private readonly TrashService _trashService = new();
    private readonly IconAssetService _iconAssetService = new();
    private readonly StoreService _storeService;
    private readonly ItemActionService _itemActionService;
    private readonly DragDropService _dragDropService;

    private readonly ListBox _groupListBox = new();
    private readonly DataGridView _itemGrid = new();
    private readonly Label _descriptionLabel = new();
    private readonly Label _statusLabel = new();
    private readonly Label _legendLabel = new();
    private readonly ContextMenuStrip _itemContextMenu = new();
    private readonly NotifyIcon _notifyIcon = new();
    private readonly ContextMenuStrip _trayMenu = new();

    private ContextBinderStore _store = new();
    private AppSettings _settings = new();
    private bool _allowExit;

    public MainForm(StoreService storeService)
    {
        _storeService = storeService;
        _itemActionService = new ItemActionService(_clipboardService);
        _dragDropService = new DragDropService(_fileTypeDetector);

        Text = "ContextBinder v2";
        MinimumSize = new Size(960, 600);
        Size = new Size(1120, 680);
        StartPosition = FormStartPosition.CenterScreen;
        Font = SystemFonts.MessageBoxFont;
        AllowDrop = true;
        Icon = _iconAssetService.GetAppIcon();

        BuildLayout();
        BuildContextMenu();
        BuildTrayIcon();
        WireEvents();
        LoadState();
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!_allowExit && e.CloseReason == CloseReason.UserClosing && _settings.MinimizeToTrayOnClose)
        {
            e.Cancel = true;
            Hide();
            _notifyIcon.Visible = true;
            SetStatus("タスクトレイに格納しました。");
            return;
        }

        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _iconAssetService.Dispose();
        _trayMenu.Dispose();
        _itemContextMenu.Dispose();
        base.OnFormClosing(e);
    }

    private void BuildLayout()
    {
        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 3,
            RowCount = 2,
            Padding = new Padding(10)
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 190));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 165));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));

        Panel groupPanel = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 0, 8, 0)
        };
        Label groupLabel = new()
        {
            Dock = DockStyle.Top,
            Height = 24,
            Text = "グループ一覧",
            TextAlign = ContentAlignment.MiddleLeft
        };
        _groupListBox.Dock = DockStyle.Fill;
        groupPanel.Controls.Add(_groupListBox);
        groupPanel.Controls.Add(groupLabel);

        _itemGrid.Dock = DockStyle.Fill;
        _itemGrid.AllowUserToAddRows = false;
        _itemGrid.AllowUserToDeleteRows = false;
        _itemGrid.AllowUserToResizeRows = false;
        _itemGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _itemGrid.BackgroundColor = SystemColors.Window;
        _itemGrid.BorderStyle = BorderStyle.FixedSingle;
        _itemGrid.MultiSelect = false;
        _itemGrid.ReadOnly = true;
        _itemGrid.RowTemplate.Height = 36;
        _itemGrid.RowHeadersVisible = false;
        _itemGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _itemGrid.ContextMenuStrip = _itemContextMenu;

        FlowLayoutPanel actionPanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            Padding = new Padding(8, 0, 0, 0)
        };

        actionPanel.Controls.AddRange([
            CreateActionButton("グループ追加", AddGroupButton_Click),
            CreateActionButton("ファイル追加", AddFileButton_Click),
            CreateActionButton("フォルダー追加", AddFolderButton_Click),
            CreateActionButton("URL追加", AddUrlButton_Click),
            CreateActionButton("テンプレート追加", AddTemplateButton_Click),
            CreateActionButton("開く", OpenSelectedButton_Click),
            CreateActionButton("コピー", CopySelectedButton_Click),
            CreateActionButton("編集", EditSelectedButton_Click),
            CreateActionButton("詳細", DetailSelectedButton_Click),
            CreateActionButton("削除", DeleteSelectedButton_Click)
        ]);

        TableLayoutPanel bottomPanel = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(0, 8, 0, 0)
        };
        bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 40));
        bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
        bottomPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

        _descriptionLabel.Dock = DockStyle.Fill;
        _descriptionLabel.Text = "ファイル・フォルダー・URL・テキストをここにドラッグ＆ドロップすると登録できます。右クリックで詳細操作を表示できます。";
        _descriptionLabel.TextAlign = ContentAlignment.MiddleLeft;

        _statusLabel.Dock = DockStyle.Fill;
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;

        _legendLabel.Dock = DockStyle.Fill;
        _legendLabel.Text = "凡例: フォルダー / ファイル / 画像 / 動画 / URL / テンプレート";
        _legendLabel.TextAlign = ContentAlignment.MiddleLeft;

        bottomPanel.Controls.Add(_descriptionLabel, 0, 0);
        bottomPanel.Controls.Add(_statusLabel, 0, 1);
        bottomPanel.Controls.Add(_legendLabel, 0, 2);

        root.Controls.Add(groupPanel, 0, 0);
        root.Controls.Add(_itemGrid, 1, 0);
        root.Controls.Add(actionPanel, 2, 0);
        root.Controls.Add(bottomPanel, 0, 1);
        root.SetColumnSpan(bottomPanel, 3);
        Controls.Add(root);
    }

    private Button CreateActionButton(string text, EventHandler clickHandler)
    {
        Button button = new()
        {
            Text = text,
            Width = 145,
            Height = 31,
            Margin = new Padding(0, 0, 0, 7)
        };
        button.Click += clickHandler;
        return button;
    }

    private void BuildContextMenu()
    {
        _itemContextMenu.Items.Add("開く", null, OpenSelectedButton_Click);
        _itemContextMenu.Items.Add("コピー", null, CopySelectedButton_Click);
        _itemContextMenu.Items.Add("編集", null, EditSelectedButton_Click);
        _itemContextMenu.Items.Add("詳細", null, DetailSelectedButton_Click);
        _itemContextMenu.Items.Add(new ToolStripSeparator());
        _itemContextMenu.Items.Add("削除", null, DeleteSelectedButton_Click);

        // TODO: 種類別メニュー、置いてあるフォルダーを開く、タイトルコピー、グループ間コピー/移動を追加する。
    }

    private void BuildTrayIcon()
    {
        _trayMenu.Items.Add("開く", null, (_, _) => ShowFromTray());
        _trayMenu.Items.Add("終了", null, (_, _) => ExitApplication());

        _notifyIcon.Icon = _iconAssetService.GetTrayIcon();
        _notifyIcon.Text = "ContextBinder v2";
        _notifyIcon.ContextMenuStrip = _trayMenu;
        _notifyIcon.Visible = true;
        _notifyIcon.DoubleClick += (_, _) => ShowFromTray();
    }

    private void WireEvents()
    {
        _groupListBox.SelectedIndexChanged += (_, _) => RefreshItemGrid();
        _itemGrid.CellDoubleClick += (_, _) => OpenSelectedItem();
        _itemGrid.MouseDown += ItemGrid_MouseDown;
        _itemGrid.DragEnter += ItemGrid_DragEnter;
        _itemGrid.DragDrop += ItemGrid_DragDrop;
        DragEnter += ItemGrid_DragEnter;
        DragDrop += ItemGrid_DragDrop;
    }

    private void LoadState()
    {
        try
        {
            _settings = _storeService.LoadSettings();
            _store = _storeService.LoadStore();
            RefreshGroupList();
            SetStatus($"登録内容と設定を読み込みました。保存先: {_storeService.Location.DataDirectory}");
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _settings = new AppSettings();
            _store = new ContextBinderStore();
            RefreshGroupList();
        }
    }

    private void RefreshGroupList()
    {
        string? selectedGroupId = GetSelectedGroup()?.Id;
        _groupListBox.DataSource = null;
        _groupListBox.DataSource = _store.Groups.OrderBy(group => group.SortOrder).ToList();
        _groupListBox.DisplayMember = nameof(BinderGroup.Name);

        if (!string.IsNullOrWhiteSpace(selectedGroupId))
        {
            BinderGroup? selectedGroup = _store.Groups.FirstOrDefault(group => group.Id == selectedGroupId);
            if (selectedGroup is not null)
            {
                _groupListBox.SelectedItem = selectedGroup;
            }
        }

        if (_groupListBox.SelectedIndex < 0 && _groupListBox.Items.Count > 0)
        {
            _groupListBox.SelectedIndex = 0;
        }

        RefreshItemGrid();
    }

    private void RefreshItemGrid()
    {
        BinderGroup? group = GetSelectedGroup();
        IReadOnlyList<BinderItem> items = group is null
            ? []
            : _searchService.FilterItems([group], string.Empty, SearchScope.CurrentGroup, group.Id);

        _itemGrid.DataSource = items.Select(item => new ItemGridRow(
            item.Id,
            _iconAssetService.GetItemIcon(item.Type),
            _fileTypeDetector.GetDisplayName(item.Type),
            item.Title,
            item.Type == BinderItemType.Template ? CreateTemplatePreview(item.TemplateText) : item.PathOrUrl,
            GetItemStatus(item))).ToList();

        if (_itemGrid.Columns[nameof(ItemGridRow.ItemId)] is DataGridViewColumn idColumn)
        {
            idColumn.Visible = false;
        }

        if (_itemGrid.Columns[nameof(ItemGridRow.Icon)] is DataGridViewImageColumn iconColumn)
        {
            iconColumn.HeaderText = "";
            iconColumn.Width = 42;
            iconColumn.FillWeight = 8;
            iconColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        if (_itemGrid.Columns[nameof(ItemGridRow.Type)] is DataGridViewColumn typeColumn)
        {
            typeColumn.HeaderText = "種類";
            typeColumn.FillWeight = 18;
        }

        if (_itemGrid.Columns[nameof(ItemGridRow.Title)] is DataGridViewColumn titleColumn)
        {
            titleColumn.HeaderText = "タイトル";
            titleColumn.FillWeight = 32;
        }

        if (_itemGrid.Columns[nameof(ItemGridRow.Reference)] is DataGridViewColumn referenceColumn)
        {
            referenceColumn.HeaderText = "参照先";
            referenceColumn.FillWeight = 42;
        }

        if (_itemGrid.Columns[nameof(ItemGridRow.Status)] is DataGridViewColumn statusColumn)
        {
            statusColumn.HeaderText = "状態";
            statusColumn.FillWeight = 12;
        }
    }

    private void AddGroupButton_Click(object? sender, EventArgs e)
    {
        string? name = InputDialog.Show(this, "グループ追加", "グループ名");
        if (string.IsNullOrWhiteSpace(name))
        {
            return;
        }

        if (_store.Groups.Any(group => string.Equals(group.Name, name, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show(this, "同じ名前のグループが既にあります。", "追加できません", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BinderGroup group = new()
        {
            Name = name,
            SortOrder = _store.Groups.Count
        };
        _store.Groups.Add(group);
        SaveAll();
        RefreshGroupList();
        _groupListBox.SelectedItem = group;
        SetStatus($"グループ「{name}」を追加しました。");
    }

    private void AddFileButton_Click(object? sender, EventArgs e)
    {
        BinderGroup? group = EnsureSelectedGroup();
        if (group is null)
        {
            return;
        }

        using OpenFileDialog dialog = new()
        {
            Title = "追加するファイルを選択",
            Multiselect = true
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        int addedCount = 0;
        foreach (string fileName in dialog.FileNames)
        {
            BinderItemType type = _fileTypeDetector.DetectFromPath(fileName);
            BinderItem item = new()
            {
                GroupId = group.Id,
                Type = type,
                Title = _fileTypeDetector.CreateDefaultTitle(type, fileName),
                PathOrUrl = fileName
            };

            if (AddItemToGroup(group, item, showEditDialog: true))
            {
                addedCount++;
            }
        }

        SetStatus($"{addedCount}件の項目を追加しました。");
    }

    private void AddFolderButton_Click(object? sender, EventArgs e)
    {
        BinderGroup? group = EnsureSelectedGroup();
        if (group is null)
        {
            return;
        }

        using FolderBrowserDialog dialog = new()
        {
            Description = "追加するフォルダーを選択",
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        BinderItem item = new()
        {
            GroupId = group.Id,
            Type = BinderItemType.Folder,
            Title = _fileTypeDetector.CreateDefaultTitle(BinderItemType.Folder, dialog.SelectedPath),
            PathOrUrl = dialog.SelectedPath
        };

        if (AddItemToGroup(group, item, showEditDialog: true))
        {
            SetStatus("フォルダーを追加しました。");
        }
    }

    private void AddUrlButton_Click(object? sender, EventArgs e)
    {
        BinderGroup? group = EnsureSelectedGroup();
        if (group is null)
        {
            return;
        }

        BinderItem item = new()
        {
            GroupId = group.Id,
            Type = BinderItemType.Url
        };

        if (AddItemToGroup(group, item, showEditDialog: true, allowTypeChange: false))
        {
            SetStatus("URLを追加しました。");
        }
    }

    private void AddTemplateButton_Click(object? sender, EventArgs e)
    {
        BinderGroup? group = EnsureSelectedGroup();
        if (group is null)
        {
            return;
        }

        BinderItem item = new()
        {
            GroupId = group.Id,
            Type = BinderItemType.Template
        };

        if (AddItemToGroup(group, item, showEditDialog: true, allowTypeChange: false))
        {
            SetStatus("テンプレートを追加しました。");
        }
    }

    private bool AddItemToGroup(BinderGroup group, BinderItem item, bool showEditDialog, bool allowTypeChange = false)
    {
        if (showEditDialog)
        {
            using ItemEditForm form = new(item, _fileTypeDetector, allowTypeChange);
            if (form.ShowDialog(this) != DialogResult.OK)
            {
                return false;
            }
        }

        item.GroupId = group.Id;
        if (_storeService.HasDuplicateReference(group, item))
        {
            MessageBox.Show(this, "同じグループ内に同じ参照先の項目が既にあります。", "重複チェック", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            SetStatus("既に登録されている項目を表示しました。");
            return false;
        }

        group.Items.Add(item);
        SaveAll();
        RefreshItemGrid();
        return true;
    }

    private void OpenSelectedButton_Click(object? sender, EventArgs e)
    {
        OpenSelectedItem();
    }

    private void CopySelectedButton_Click(object? sender, EventArgs e)
    {
        BinderItem? item = GetSelectedItem();
        if (item is null)
        {
            return;
        }

        try
        {
            _itemActionService.Copy(item);
            SetStatus("コピーしました。");
        }
        catch (Exception ex) when (ex is InvalidOperationException or ExternalException)
        {
            MessageBox.Show(this, $"コピーに失敗しました。\n{ex.Message}", "コピーエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void EditSelectedButton_Click(object? sender, EventArgs e)
    {
        BinderGroup? group = GetSelectedGroup();
        BinderItem? item = GetSelectedItem();
        if (group is null || item is null)
        {
            return;
        }

        BinderItem beforeEdit = new()
        {
            Id = item.Id,
            GroupId = item.GroupId,
            Type = item.Type,
            Title = item.Title,
            PathOrUrl = item.PathOrUrl,
            TemplateText = item.TemplateText,
            CreatedAt = item.CreatedAt,
            UpdatedAt = item.UpdatedAt
        };

        using ItemEditForm form = new(item, _fileTypeDetector, allowTypeChange: true);
        if (form.ShowDialog(this) != DialogResult.OK)
        {
            CopyItemValues(beforeEdit, item);
            return;
        }

        if (_storeService.HasDuplicateReference(group, item, item.Id))
        {
            CopyItemValues(beforeEdit, item);
            MessageBox.Show(this, "同じグループ内に同じ参照先の項目が既にあります。", "重複チェック", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SaveAll();
        RefreshItemGrid();
        SetStatus("項目を編集しました。");
    }

    private void DetailSelectedButton_Click(object? sender, EventArgs e)
    {
        BinderItem? item = GetSelectedItem();
        if (item is null)
        {
            return;
        }

        // TODO: 専用の詳細フォームを追加し、存在確認やコピー操作をまとめて表示する。
        MessageBox.Show(
            this,
            $"タイトル: {item.Title}\n種類: {_fileTypeDetector.GetDisplayName(item.Type)}\n参照先: {item.ReferenceText}\n作成: {item.CreatedAt.LocalDateTime}\n更新: {item.UpdatedAt.LocalDateTime}",
            "詳細",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void DeleteSelectedButton_Click(object? sender, EventArgs e)
    {
        BinderGroup? group = GetSelectedGroup();
        BinderItem? item = GetSelectedItem();
        if (group is null || item is null)
        {
            return;
        }

        DialogResult result = MessageBox.Show(
            this,
            $"「{item.Title}」を削除します。登録元のファイルやフォルダーは削除されません。",
            "削除確認",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Warning);

        if (result != DialogResult.OK)
        {
            return;
        }

        DeletedItemRecord deletedItemRecord = _trashService.MoveItemToTrash(group, item);
        _store.DeletedItems.Add(deletedItemRecord);
        SaveAll();
        RefreshItemGrid();
        SetStatus("項目を削除しました。");
    }

    private void OpenSelectedItem()
    {
        BinderItem? item = GetSelectedItem();
        if (item is null)
        {
            return;
        }

        try
        {
            _itemActionService.Open(item);
            SetStatus(item.Type == BinderItemType.Template ? "テンプレート文をコピーしました。" : "項目を開きました。");
        }
        catch (Exception ex) when (ex is InvalidOperationException or System.ComponentModel.Win32Exception or FileNotFoundException)
        {
            MessageBox.Show(this, $"項目を開けませんでした。\n{ex.Message}", "操作エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ItemGrid_MouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Right)
        {
            return;
        }

        DataGridView.HitTestInfo hit = _itemGrid.HitTest(e.X, e.Y);
        if (hit.RowIndex >= 0)
        {
            _itemGrid.ClearSelection();
            _itemGrid.Rows[hit.RowIndex].Selected = true;
        }
    }

    private void ItemGrid_DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data is not null
            && (e.Data.GetDataPresent(DataFormats.FileDrop) || e.Data.GetDataPresent(DataFormats.Text)))
        {
            e.Effect = DragDropEffects.Copy;
        }
    }

    private void ItemGrid_DragDrop(object? sender, DragEventArgs e)
    {
        BinderGroup? group = EnsureSelectedGroup();
        if (group is null || e.Data is null)
        {
            return;
        }

        IReadOnlyList<BinderItem> items = _dragDropService.CreateItemsFromDataObject(e.Data, group.Id);
        int addedCount = 0;
        int skippedCount = 0;

        foreach (BinderItem item in items)
        {
            if (_storeService.HasDuplicateReference(group, item))
            {
                skippedCount++;
                continue;
            }

            group.Items.Add(item);
            addedCount++;
        }

        if (addedCount > 0)
        {
            SaveAll();
            RefreshItemGrid();
        }

        SetStatus($"{addedCount}件を追加しました。{skippedCount}件をスキップしました。");
    }

    private BinderGroup? EnsureSelectedGroup()
    {
        BinderGroup? group = GetSelectedGroup();
        if (group is not null)
        {
            return group;
        }

        MessageBox.Show(this, "まずグループを作成してください。", "グループがありません", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return null;
    }

    private BinderGroup? GetSelectedGroup()
    {
        return _groupListBox.SelectedItem as BinderGroup;
    }

    private BinderItem? GetSelectedItem()
    {
        BinderGroup? group = GetSelectedGroup();
        if (group is null || _itemGrid.SelectedRows.Count == 0)
        {
            return null;
        }

        if (_itemGrid.SelectedRows[0].DataBoundItem is not ItemGridRow row)
        {
            return null;
        }

        return group.Items.FirstOrDefault(item => item.Id == row.ItemId);
    }

    private void SaveAll()
    {
        try
        {
            _storeService.SaveSettings(_settings);
            _storeService.SaveStore(_store, _settings);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or InvalidOperationException)
        {
            MessageBox.Show(this, $"登録内容と設定の保存に失敗しました。\n{ex.Message}", "保存エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void SetStatus(string message)
    {
        _statusLabel.Text = message;
    }

    private void ShowFromTray()
    {
        Show();
        WindowState = FormWindowState.Normal;
        Activate();
    }

    private void ExitApplication()
    {
        _allowExit = true;
        Close();
    }

    private static string CreateTemplatePreview(string templateText)
    {
        string singleLine = templateText.Replace("\r", " ").Replace("\n", " ").Trim();
        return singleLine.Length <= 80 ? singleLine : $"{singleLine[..80]}...";
    }

    private static string GetItemStatus(BinderItem item)
    {
        if (item.Type == BinderItemType.Folder)
        {
            return Directory.Exists(item.PathOrUrl) ? "" : "未確認";
        }

        if (item.Type is BinderItemType.File or BinderItemType.Image or BinderItemType.Video)
        {
            return File.Exists(item.PathOrUrl) ? "" : "未確認";
        }

        return "";
    }

    private static void CopyItemValues(BinderItem source, BinderItem destination)
    {
        destination.GroupId = source.GroupId;
        destination.Type = source.Type;
        destination.Title = source.Title;
        destination.PathOrUrl = source.PathOrUrl;
        destination.TemplateText = source.TemplateText;
        destination.CreatedAt = source.CreatedAt;
        destination.UpdatedAt = source.UpdatedAt;
    }

    private sealed record ItemGridRow(
        string ItemId,
        Image Icon,
        string Type,
        string Title,
        string Reference,
        string Status);
}
