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
    private readonly IconAssetService _iconAssetService;
    private readonly StoreService _storeService;
    private readonly ItemActionService _itemActionService;
    private readonly DragDropService _dragDropService;
    private readonly ToolTip _toolTip = new();

    private readonly ListBox _groupListBox = new();
    private readonly DataGridView _itemGrid = new();
    private readonly CheckBox _showBeginnerHintsCheckBox = new();
    private readonly CheckBox _showIconLegendCheckBox = new();
    private readonly Label _beginnerHintsLabel = new();
    private readonly Label _statusLabel = new();
    private readonly Panel _beginnerHintsPanel = new();
    private readonly Panel _iconLegendPanel = new();
    private readonly FlowLayoutPanel _iconLegendFlow = new();
    private readonly ContextMenuStrip _itemContextMenu = new();
    private readonly NotifyIcon _notifyIcon = new();
    private readonly ContextMenuStrip _trayMenu = new();
    private readonly TableLayoutPanel _bottomPanel = new();

    private Button? _detailButton;
    private ToolStripItem? _detailContextMenuItem;
    private RowStyle? _bottomRowStyle;
    private bool _loadingSettings;

    private ContextBinderStore _store = new();
    private AppSettings _settings = new();
    private bool _allowExit;
    private bool _runtimeResourcesDisposed;

    public MainForm(StoreService storeService, IconAssetService? iconAssetService = null)
    {
        _storeService = storeService;
        _iconAssetService = iconAssetService ?? new IconAssetService();
        _itemActionService = new ItemActionService(_clipboardService);
        _dragDropService = new DragDropService(_fileTypeDetector);

        Text = "ContextBinder v2";
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(980, 700);
        Size = new Size(1180, 780);
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
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 185));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 90));
        _bottomRowStyle = root.RowStyles[1];

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
        _itemGrid.AutoGenerateColumns = false;
        _itemGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _itemGrid.BackgroundColor = SystemColors.Window;
        _itemGrid.BorderStyle = BorderStyle.FixedSingle;
        _itemGrid.MultiSelect = false;
        _itemGrid.ReadOnly = true;
        _itemGrid.RowTemplate.Height = 36;
        _itemGrid.RowHeadersVisible = false;
        _itemGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _itemGrid.ContextMenuStrip = _itemContextMenu;
        ConfigureItemGridColumns();

        FlowLayoutPanel actionPanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            AutoScroll = true,
            Padding = new Padding(8, 0, 0, 0)
        };

        Button detailButton = CreateActionButton("詳細", "登録内容の詳細を確認します。", DetailSelectedButton_Click);
        _detailButton = detailButton;
        actionPanel.Controls.AddRange([
            CreateActionButton("グループ追加", "用途ごとに登録先を分けます。", AddGroupButton_Click),
            CreateActionButton("ファイル追加", "ファイルへの参照を現在のグループへ登録します。", AddFileButton_Click),
            CreateActionButton("フォルダー追加", "フォルダーへの参照を現在のグループへ登録します。", AddFolderButton_Click),
            CreateActionButton("URL追加", "WebページのURLを現在のグループへ登録します。", AddUrlButton_Click),
            CreateActionButton("テンプレート追加", "よく使う文章を登録します。", AddTemplateButton_Click),
            CreateActionButton("開く", "選択した項目を開きます。テンプレートは本文をコピーします。", OpenSelectedButton_Click),
            CreateActionButton("コピー", "選択した項目のパス、URL、本文などをコピーします。", CopySelectedButton_Click),
            CreateActionButton("編集", "タイトルや参照先を編集します。", EditSelectedButton_Click),
            detailButton,
            CreateActionButton("削除", "選択した項目を削除します。登録元ファイルは削除されません。", DeleteSelectedButton_Click)
        ]);

        BuildBottomPanel();

        root.Controls.Add(groupPanel, 0, 0);
        root.Controls.Add(_itemGrid, 1, 0);
        root.Controls.Add(actionPanel, 2, 0);
        root.Controls.Add(_bottomPanel, 0, 1);
        root.SetColumnSpan(_bottomPanel, 3);
        Controls.Add(root);
    }

    private void BuildBottomPanel()
    {
        _bottomPanel.Dock = DockStyle.Fill;
        _bottomPanel.ColumnCount = 1;
        _bottomPanel.RowCount = 3;
        _bottomPanel.Padding = new Padding(0, 8, 0, 0);
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 124));
        _bottomPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 148));

        FlowLayoutPanel togglePanel = new()
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };
        _showBeginnerHintsCheckBox.Text = "初心者向け説明を表示";
        _showBeginnerHintsCheckBox.Width = 170;
        _showBeginnerHintsCheckBox.CheckedChanged += DisplayToggleCheckBox_CheckedChanged;
        _showIconLegendCheckBox.Text = "アイコンの意味を表示";
        _showIconLegendCheckBox.Width = 170;
        _showIconLegendCheckBox.CheckedChanged += DisplayToggleCheckBox_CheckedChanged;
        _statusLabel.AutoSize = false;
        _statusLabel.Width = 520;
        _statusLabel.Height = 26;
        _statusLabel.TextAlign = ContentAlignment.MiddleLeft;
        _statusLabel.AutoEllipsis = true;
        togglePanel.Controls.Add(_showBeginnerHintsCheckBox);
        togglePanel.Controls.Add(_showIconLegendCheckBox);
        togglePanel.Controls.Add(_statusLabel);

        _beginnerHintsPanel.Dock = DockStyle.Fill;
        _beginnerHintsPanel.BorderStyle = BorderStyle.FixedSingle;
        _beginnerHintsPanel.Padding = new Padding(8);
        TableLayoutPanel hintsLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        hintsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        hintsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Label hintsTitleLabel = CreateBottomTitleLabel("使い方のヒント");
        _beginnerHintsLabel.Dock = DockStyle.Fill;
        _beginnerHintsLabel.TextAlign = ContentAlignment.TopLeft;
        _beginnerHintsLabel.Text = """
・グループ追加: 用途ごとに登録先を分けます。
・ファイル / フォルダー / URL / テンプレート追加: 参照先や文章を登録します。
・開く / コピー: 選択した項目を開いたり、パス・URL・本文をコピーします。
・編集 / 詳細 / 削除: 登録内容を確認・整理します。登録元ファイルそのものは削除されません。
""";
        hintsLayout.Controls.Add(hintsTitleLabel, 0, 0);
        hintsLayout.Controls.Add(_beginnerHintsLabel, 0, 1);
        _beginnerHintsPanel.Controls.Add(hintsLayout);

        _iconLegendPanel.Dock = DockStyle.Fill;
        _iconLegendPanel.BorderStyle = BorderStyle.FixedSingle;
        _iconLegendPanel.Padding = new Padding(8);
        TableLayoutPanel iconLayout = new()
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2
        };
        iconLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
        iconLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Label iconTitleLabel = CreateBottomTitleLabel("アイコンの意味");
        _iconLegendFlow.Dock = DockStyle.Fill;
        _iconLegendFlow.FlowDirection = FlowDirection.LeftToRight;
        _iconLegendFlow.WrapContents = true;
        _iconLegendFlow.AutoScroll = true;
        iconLayout.Controls.Add(iconTitleLabel, 0, 0);
        iconLayout.Controls.Add(_iconLegendFlow, 0, 1);
        _iconLegendPanel.Controls.Add(iconLayout);
        BuildIconLegend();

        _bottomPanel.Controls.Add(togglePanel, 0, 0);
        _bottomPanel.Controls.Add(_beginnerHintsPanel, 0, 1);
        _bottomPanel.Controls.Add(_iconLegendPanel, 0, 2);
    }

    private void BuildIconLegend()
    {
        _iconLegendFlow.Controls.Clear();
        AddLegendItem(BinderItemType.Folder, "フォルダー", "フォルダーを開きます");
        AddLegendItem(BinderItemType.File, "ファイル", "既定のアプリで開きます");
        AddLegendItem(BinderItemType.Image, "画像", "画像ファイルです");
        AddLegendItem(BinderItemType.Video, "動画", "動画ファイルです");
        AddLegendItem(BinderItemType.Url, "URL", "ブラウザで開きます");
        AddLegendItem(BinderItemType.Template, "テンプレート", "本文をコピーして使います");
    }

    private void AddLegendItem(BinderItemType type, string title, string description)
    {
        Panel itemPanel = new()
        {
            Width = 250,
            Height = 50,
            Margin = new Padding(0, 0, 8, 6)
        };
        PictureBox pictureBox = new()
        {
            Image = _iconAssetService.GetItemIcon(type),
            SizeMode = PictureBoxSizeMode.Zoom,
            Location = new Point(0, 7),
            Size = new Size(30, 30)
        };
        Label label = new()
        {
            Text = $"{title}: {description}",
            Location = new Point(36, 2),
            Size = new Size(208, 44),
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
            HeaderText = "種類",
            FillWeight = 18
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Title),
            DataPropertyName = nameof(ItemGridRow.Title),
            HeaderText = "タイトル",
            FillWeight = 32
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Reference),
            DataPropertyName = nameof(ItemGridRow.Reference),
            HeaderText = "参照先",
            FillWeight = 42
        });
        _itemGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = nameof(ItemGridRow.Status),
            DataPropertyName = nameof(ItemGridRow.Status),
            HeaderText = "状態",
            FillWeight = 12
        });
    }

    private Button CreateActionButton(string text, string toolTipText, EventHandler clickHandler)
    {
        Button button = new()
        {
            Text = text,
            Width = 165,
            Height = 31,
            Margin = new Padding(0, 0, 0, 7)
        };
        button.Click += clickHandler;
        _toolTip.SetToolTip(button, toolTipText);
        return button;
    }

    private void BuildContextMenu()
    {
        _itemContextMenu.Items.Add("開く", null, OpenSelectedButton_Click);
        _itemContextMenu.Items.Add("コピー", null, CopySelectedButton_Click);
        _itemContextMenu.Items.Add("編集", null, EditSelectedButton_Click);
        _detailContextMenuItem = _itemContextMenu.Items.Add("詳細", null, DetailSelectedButton_Click);
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

        int hintsHeight = _settings.ShowBeginnerHints ? 124 : 0;
        int iconMeaningHeight = _settings.ShowIconLegend ? 148 : 0;
        int height = 44;
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
