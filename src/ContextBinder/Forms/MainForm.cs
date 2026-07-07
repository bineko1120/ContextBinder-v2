using System.Runtime.InteropServices;
using ContextBinder.Models;
using ContextBinder.Services;
using MainFormLayout = ContextBinder.UiLayoutSettings.Main;
using MainFormTexts = ContextBinder.UiTexts.MainForm;

namespace ContextBinder.Forms;

public sealed partial class MainForm : Form
{
    private readonly FileTypeDetector _fileTypeDetector = new();
    private readonly ClipboardService _clipboardService = new();
    private readonly SearchService _searchService = new();
    private readonly TrashService _trashService = new();
    private readonly IconAssetService _iconAssetService;
    private readonly StoreService _storeService;
    private readonly ItemActionService _itemActionService;
    private readonly DragDropService _dragDropService;
    private readonly ToolTip _toolTip = new();

    private ToolStripItem? _detailContextMenuItem;
    private RowStyle? _bottomRowStyle;
    private bool _loadingSettings;

    private ContextBinderStore _store = new();
    private AppSettings _settings = new();
    private bool _allowExit;
    private bool _runtimeResourcesDisposed;

    public MainForm()
        : this(CreateDesignerStoreService(), new IconAssetService(), designMode: true)
    {
    }

    public MainForm(StoreService storeService, IconAssetService? iconAssetService = null)
        : this(storeService, iconAssetService, designMode: false)
    {
    }

    private MainForm(StoreService storeService, IconAssetService? iconAssetService, bool designMode)
    {
        _storeService = storeService;
        _iconAssetService = iconAssetService ?? new IconAssetService();
        _itemActionService = new ItemActionService(_clipboardService);
        _dragDropService = new DragDropService(_fileTypeDetector);

        InitializeComponent();
        _bottomRowStyle = _rootLayout.RowStyles[1];

        ConfigureItemGridColumns();
        BuildContextMenu();
        ConfigureToolTips();
        WireEvents();

        if (designMode)
        {
            _settings = AppSettingsFactory.CreateRecommended();
            ApplySettingsToView();
            SetStatus(MainFormTexts.DesignerPreviewStatus);
            return;
        }

        Icon = _iconAssetService.GetAppIcon();
        BuildIconLegend();
        BuildTrayIcon();
        LoadState();
    }

    private static StoreService CreateDesignerStoreService()
    {
        string dataDirectory = Path.Combine(Path.GetTempPath(), "ContextBinderDesigner");
        StorageLocation location = new()
        {
            Mode = StorageMode.Standard,
            DataDirectory = dataDirectory,
            StoreFilePath = Path.Combine(dataDirectory, "contextbinder.store.json"),
            SettingsFilePath = Path.Combine(dataDirectory, "settings.json"),
            BackupDirectory = Path.Combine(dataDirectory, "backups"),
            TrashDirectory = Path.Combine(dataDirectory, "trash")
        };

        return new StoreService(location, new BackupService());
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
        DisposeRuntimeResources();
        base.OnFormClosing(e);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            DisposeRuntimeResources();
        }

        base.Dispose(disposing);
    }

    private void BuildIconLegend()
    {
        _iconLegendFlow.Controls.Clear();
        foreach (ContextBinder.UiTexts.IconMeaningText text in MainFormTexts.IconMeanings)
        {
            AddLegendItem(text.Type, text.Title, text.Description);
        }
    }

    private void AddLegendItem(BinderItemType type, string title, string description)
    {
        Panel itemPanel = new()
        {
            Width = MainFormLayout.IconMeaningItemWidth,
            Height = MainFormLayout.IconMeaningItemHeight,
            Margin = new Padding(0, 0, 8, 6)
        };
        PictureBox pictureBox = new()
        {
            Image = _iconAssetService.GetItemIcon(type),
            SizeMode = PictureBoxSizeMode.Zoom,
            Location = new Point(0, (MainFormLayout.IconMeaningItemHeight - MainFormLayout.IconMeaningIconSize) / 2),
            Size = new Size(MainFormLayout.IconMeaningIconSize, MainFormLayout.IconMeaningIconSize)
        };
        Label label = new()
        {
            Text = $"{title}: {description}",
            Location = new Point(MainFormLayout.IconMeaningTextOffsetX, 2),
            Size = new Size(MainFormLayout.IconMeaningItemWidth - MainFormLayout.IconMeaningTextOffsetX - 6, MainFormLayout.IconMeaningItemHeight - 6),
            TextAlign = ContentAlignment.MiddleLeft
        };
        itemPanel.Controls.Add(pictureBox);
        itemPanel.Controls.Add(label);
        _iconLegendFlow.Controls.Add(itemPanel);
    }

    private static Label CreateBottomTitleLabel(string text)
    {
        return new Label
        {
            Text = text,
            Dock = DockStyle.Fill,
            Font = new Font(Control.DefaultFont, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private void ConfigureItemGridColumns()
    {
        _itemGrid.Columns.Clear();
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.ItemId),
            DataPropertyName = nameof(ItemGridRow.ItemId),
            Visible = false
        });
        _itemGrid.Columns.Add(new DataGridViewImageColumn
        {
            Name = nameof(ItemGridRow.Icon),
            DataPropertyName = nameof(ItemGridRow.Icon),
            HeaderText = "",
            Width = 42,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            ImageLayout = DataGridViewImageCellLayout.Zoom
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Type),
            DataPropertyName = nameof(ItemGridRow.Type),
            HeaderText = MainFormTexts.TypeColumnHeader,
            FillWeight = 18
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Title),
            DataPropertyName = nameof(ItemGridRow.Title),
            HeaderText = MainFormTexts.TitleColumnHeader,
            FillWeight = 32
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Reference),
            DataPropertyName = nameof(ItemGridRow.Reference),
            HeaderText = MainFormTexts.ReferenceColumnHeader,
            FillWeight = 42
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Status),
            DataPropertyName = nameof(ItemGridRow.Status),
            HeaderText = MainFormTexts.StatusColumnHeader,
            FillWeight = 12
        });
    }

    private void BuildContextMenu()
    {
        _itemContextMenu.Items.Add(MainFormTexts.OpenButton.Label, null, OpenSelectedButton_Click);
        _itemContextMenu.Items.Add(MainFormTexts.CopyButton.Label, null, CopySelectedButton_Click);
        _itemContextMenu.Items.Add(MainFormTexts.EditButton.Label, null, EditSelectedButton_Click);
        _detailContextMenuItem = _itemContextMenu.Items.Add(MainFormTexts.DetailButton.Label, null, DetailSelectedButton_Click);
        _itemContextMenu.Items.Add(new ToolStripSeparator());
        _itemContextMenu.Items.Add(MainFormTexts.DeleteButton.Label, null, DeleteSelectedButton_Click);

        // TODO: 種類別メニュー、置いてあるフォルダーを開く、タイトルコピー、グループ間コピー/移動を追加する。
    }

    private void ConfigureToolTips()
    {
        _toolTip.SetToolTip(_addGroupButton, MainFormTexts.AddGroupButton.ToolTip);
        _toolTip.SetToolTip(_addFileButton, MainFormTexts.AddFileButton.ToolTip);
        _toolTip.SetToolTip(_addFolderButton, MainFormTexts.AddFolderButton.ToolTip);
        _toolTip.SetToolTip(_addUrlButton, MainFormTexts.AddUrlButton.ToolTip);
        _toolTip.SetToolTip(_addTemplateButton, MainFormTexts.AddTemplateButton.ToolTip);
        _toolTip.SetToolTip(_openButton, MainFormTexts.OpenButton.ToolTip);
        _toolTip.SetToolTip(_copyButton, MainFormTexts.CopyButton.ToolTip);
        _toolTip.SetToolTip(_editButton, MainFormTexts.EditButton.ToolTip);
        _toolTip.SetToolTip(_detailButton, MainFormTexts.DetailButton.ToolTip);
        _toolTip.SetToolTip(_deleteButton, MainFormTexts.DeleteButton.ToolTip);
    }

    private void BuildTrayIcon()
    {
        _trayMenu.Items.Add(MainFormTexts.TrayOpen, null, (_, _) => ShowFromTray());
        _trayMenu.Items.Add(MainFormTexts.TrayExit, null, (_, _) => ExitApplication());

        _notifyIcon.Icon = _iconAssetService.GetTrayIcon();
        _notifyIcon.Text = MainFormTexts.WindowTitle;
        _notifyIcon.ContextMenuStrip = _trayMenu;
        _notifyIcon.Visible = true;
        _notifyIcon.DoubleClick += (_, _) => ShowFromTray();
    }

    private void WireEvents()
    {
        _addGroupButton.Click += AddGroupButton_Click;
        _addFileButton.Click += AddFileButton_Click;
        _addFolderButton.Click += AddFolderButton_Click;
        _addUrlButton.Click += AddUrlButton_Click;
        _addTemplateButton.Click += AddTemplateButton_Click;
        _openButton.Click += OpenSelectedButton_Click;
        _copyButton.Click += CopySelectedButton_Click;
        _editButton.Click += EditSelectedButton_Click;
        _detailButton.Click += DetailSelectedButton_Click;
        _deleteButton.Click += DeleteSelectedButton_Click;
        _showBeginnerHintsCheckBox.CheckedChanged += DisplayToggleCheckBox_CheckedChanged;
        _showIconLegendCheckBox.CheckedChanged += DisplayToggleCheckBox_CheckedChanged;
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
            ApplySettingsToView();
            RefreshGroupList();
            SetStatus($"登録内容と設定を読み込みました。保存先: {_storeService.Location.DataDirectory}");
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "読み込みエラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _settings = new AppSettings();
            _store = new ContextBinderStore();
            ApplySettingsToView();
            RefreshGroupList();
        }
    }

    private void ApplySettingsToView()
    {
        _loadingSettings = true;
        _showBeginnerHintsCheckBox.Checked = _settings.ShowBeginnerHints;
        _showIconLegendCheckBox.Checked = _settings.ShowIconLegend;
        _loadingSettings = false;

        _beginnerHintsPanel.Visible = _settings.ShowBeginnerHints;
        _iconLegendPanel.Visible = _settings.ShowIconLegend;
        _statusLabel.Visible = _settings.ShowOperationStatus;
        if (_detailButton is not null)
        {
            _detailButton.Enabled = _settings.EnableContextMenuDetails;
        }

        if (_detailContextMenuItem is not null)
        {
            _detailContextMenuItem.Enabled = _settings.EnableContextMenuDetails;
        }

        ApplyTypeDisplayMode();
        UpdateBottomPanelHeight();
    }

    private void ApplyTypeDisplayMode()
    {
        bool showIcon = _settings.TypeDisplayMode is TypeDisplayMode.IconAndText or TypeDisplayMode.IconOnly;
        bool showText = _settings.TypeDisplayMode is TypeDisplayMode.IconAndText or TypeDisplayMode.TextOnly;

        if (_itemGrid.Columns[nameof(ItemGridRow.Icon)] is DataGridViewColumn iconColumn)
        {
            iconColumn.Visible = showIcon;
        }

        if (_itemGrid.Columns[nameof(ItemGridRow.Type)] is DataGridViewColumn typeColumn)
        {
            typeColumn.Visible = showText;
        }
    }

    private void UpdateBottomPanelHeight()
    {
        if (_bottomRowStyle is null)
        {
            return;
        }

        int hintsHeight = _settings.ShowBeginnerHints ? MainFormLayout.BeginnerHintsHeight : 0;
        int iconMeaningHeight = _settings.ShowIconLegend ? MainFormLayout.IconMeaningHeight : 0;
        int height = MainFormLayout.ToggleRowHeight + MainFormLayout.BottomTopPadding + 2;
        _bottomPanel.RowStyles[1].Height = hintsHeight;
        _bottomPanel.RowStyles[2].Height = iconMeaningHeight;
        if (_settings.ShowBeginnerHints)
        {
            height += hintsHeight;
        }

        if (_settings.ShowIconLegend)
        {
            height += iconMeaningHeight;
        }

        _bottomRowStyle.Height = height;
    }

    private void DisplayToggleCheckBox_CheckedChanged(object? sender, EventArgs e)
    {
        if (_loadingSettings)
        {
            return;
        }

        _settings.ShowBeginnerHints = _showBeginnerHintsCheckBox.Checked;
        _settings.ShowIconLegend = _showIconLegendCheckBox.Checked;
        ApplySettingsToView();
        SaveSettingsOnly();
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

        ApplyTypeDisplayMode();
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

        if (_settings.ConfirmBeforeDelete)
        {
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
        }

        if (_settings.MoveDeletedItemsToTrash)
        {
            DeletedItemRecord deletedItemRecord = _trashService.MoveItemToTrash(group, item);
            _store.DeletedItems.Add(deletedItemRecord);
        }
        else
        {
            group.Items.Remove(item);
        }

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

    private void SaveSettingsOnly()
    {
        try
        {
            _storeService.SaveSettings(_settings);
            SetStatus("表示設定を保存しました。");
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

    private void DisposeRuntimeResources()
    {
        if (_runtimeResourcesDisposed)
        {
            return;
        }

        _notifyIcon.Visible = false;
        _notifyIcon.Dispose();
        _iconAssetService.Dispose();
        _toolTip.Dispose();
        _trayMenu.Dispose();
        _itemContextMenu.Dispose();
        _runtimeResourcesDisposed = true;
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
