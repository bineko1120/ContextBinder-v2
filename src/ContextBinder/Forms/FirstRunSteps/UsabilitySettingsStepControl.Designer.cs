namespace ContextBinder.Forms.FirstRunSteps;

public sealed partial class UsabilitySettingsStepControl
{
    private FlowLayoutPanel _rootPanel = null!;
    private Label _headingLabel = null!;
    private Label _descriptionLabel = null!;
    private CheckBox _editRecommendedSettingsCheckBox = null!;
    private GroupBox _displayGroupBox = null!;
    private FlowLayoutPanel _displayPanel = null!;
    private Label _displayDescriptionLabel = null!;
    private Label _typeDisplayModeLabel = null!;
    private ComboBox _typeDisplayModeComboBox = null!;
    private CheckBox _showBeginnerHintsCheckBox = null!;
    private CheckBox _showIconLegendCheckBox = null!;
    private CheckBox _showOperationStatusCheckBox = null!;
    private GroupBox _dragDropGroupBox = null!;
    private FlowLayoutPanel _dragDropPanel = null!;
    private Label _dragDropDescriptionLabel = null!;
    private CheckBox _confirmTitleOnDropAddCheckBox = null!;
    private CheckBox _focusExistingItemOnDuplicateCheckBox = null!;
    private CheckBox _enableGroupDropModifierShortcutsCheckBox = null!;
    private CheckBox _confirmGroupDropCopyMoveCheckBox = null!;
    private CheckBox _enableItemDragReorderCheckBox = null!;
    private CheckBox _enableExternalFileDropOutCheckBox = null!;
    private CheckBox _enableExternalUrlTextDragOutCheckBox = null!;
    private CheckBox _enableExternalTemplateTextDragOutCheckBox = null!;
    private GroupBox _closeBehaviorGroupBox = null!;
    private FlowLayoutPanel _closeBehaviorPanel = null!;
    private Label _closeBehaviorDescriptionLabel = null!;
    private CheckBox _minimizeToTrayOnCloseCheckBox = null!;
    private GroupBox _contextMenuGroupBox = null!;
    private FlowLayoutPanel _contextMenuPanel = null!;
    private Label _contextMenuDescriptionLabel = null!;
    private CheckBox _enableContextMenuDetailsCheckBox = null!;
    private GroupBox _backupDeleteGroupBox = null!;
    private FlowLayoutPanel _backupDeletePanel = null!;
    private Label _backupDeleteDescriptionLabel = null!;
    private CheckBox _autoBackupEnabledCheckBox = null!;
    private Label _maxBackupCountLabel = null!;
    private NumericUpDown _maxBackupCountNumeric = null!;
    private CheckBox _confirmBeforeDeleteCheckBox = null!;
    private CheckBox _moveDeletedItemsToTrashCheckBox = null!;
    private GroupBox _searchGroupBox = null!;
    private FlowLayoutPanel _searchPanel = null!;
    private Label _searchDescriptionLabel = null!;
    private CheckBox _searchTemplateBodyCheckBox = null!;

    private void InitializeComponent()
    {
        _rootPanel = new FlowLayoutPanel();
        _headingLabel = new Label();
        _descriptionLabel = new Label();
        _editRecommendedSettingsCheckBox = new CheckBox();
        _displayGroupBox = new GroupBox();
        _displayPanel = new FlowLayoutPanel();
        _displayDescriptionLabel = new Label();
        _typeDisplayModeLabel = new Label();
        _typeDisplayModeComboBox = new ComboBox();
        _showBeginnerHintsCheckBox = new CheckBox();
        _showIconLegendCheckBox = new CheckBox();
        _showOperationStatusCheckBox = new CheckBox();
        _dragDropGroupBox = new GroupBox();
        _dragDropPanel = new FlowLayoutPanel();
        _dragDropDescriptionLabel = new Label();
        _confirmTitleOnDropAddCheckBox = new CheckBox();
        _focusExistingItemOnDuplicateCheckBox = new CheckBox();
        _enableGroupDropModifierShortcutsCheckBox = new CheckBox();
        _confirmGroupDropCopyMoveCheckBox = new CheckBox();
        _enableItemDragReorderCheckBox = new CheckBox();
        _enableExternalFileDropOutCheckBox = new CheckBox();
        _enableExternalUrlTextDragOutCheckBox = new CheckBox();
        _enableExternalTemplateTextDragOutCheckBox = new CheckBox();
        _closeBehaviorGroupBox = new GroupBox();
        _closeBehaviorPanel = new FlowLayoutPanel();
        _closeBehaviorDescriptionLabel = new Label();
        _minimizeToTrayOnCloseCheckBox = new CheckBox();
        _contextMenuGroupBox = new GroupBox();
        _contextMenuPanel = new FlowLayoutPanel();
        _contextMenuDescriptionLabel = new Label();
        _enableContextMenuDetailsCheckBox = new CheckBox();
        _backupDeleteGroupBox = new GroupBox();
        _backupDeletePanel = new FlowLayoutPanel();
        _backupDeleteDescriptionLabel = new Label();
        _autoBackupEnabledCheckBox = new CheckBox();
        _maxBackupCountLabel = new Label();
        _maxBackupCountNumeric = new NumericUpDown();
        _confirmBeforeDeleteCheckBox = new CheckBox();
        _moveDeletedItemsToTrashCheckBox = new CheckBox();
        _searchGroupBox = new GroupBox();
        _searchPanel = new FlowLayoutPanel();
        _searchDescriptionLabel = new Label();
        _searchTemplateBodyCheckBox = new CheckBox();
        ((System.ComponentModel.ISupportInitialize)_maxBackupCountNumeric).BeginInit();
        _rootPanel.SuspendLayout();
        _displayGroupBox.SuspendLayout();
        _displayPanel.SuspendLayout();
        _dragDropGroupBox.SuspendLayout();
        _dragDropPanel.SuspendLayout();
        _closeBehaviorGroupBox.SuspendLayout();
        _closeBehaviorPanel.SuspendLayout();
        _contextMenuGroupBox.SuspendLayout();
        _contextMenuPanel.SuspendLayout();
        _backupDeleteGroupBox.SuspendLayout();
        _backupDeletePanel.SuspendLayout();
        _searchGroupBox.SuspendLayout();
        _searchPanel.SuspendLayout();
        SuspendLayout();

        _rootPanel.AutoScroll = true;
        _rootPanel.Controls.Add(_headingLabel);
        _rootPanel.Controls.Add(_descriptionLabel);
        _rootPanel.Controls.Add(_editRecommendedSettingsCheckBox);
        _rootPanel.Controls.Add(_displayGroupBox);
        _rootPanel.Controls.Add(_dragDropGroupBox);
        _rootPanel.Controls.Add(_closeBehaviorGroupBox);
        _rootPanel.Controls.Add(_contextMenuGroupBox);
        _rootPanel.Controls.Add(_backupDeleteGroupBox);
        _rootPanel.Controls.Add(_searchGroupBox);
        _rootPanel.Dock = DockStyle.Fill;
        _rootPanel.FlowDirection = FlowDirection.TopDown;
        _rootPanel.Location = new Point(0, 0);
        _rootPanel.Name = "_rootPanel";
        _rootPanel.Padding = new Padding(0, 0, 8, 0);
        _rootPanel.Size = new Size(780, 580);
        _rootPanel.TabIndex = 0;
        _rootPanel.WrapContents = false;

        _headingLabel.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
        _headingLabel.Name = "_headingLabel";
        _headingLabel.Size = new Size(740, 36);
        _headingLabel.TabIndex = 0;
        _headingLabel.Text = "おすすめ設定の内容を確認してください";
        _headingLabel.TextAlign = ContentAlignment.MiddleLeft;

        _descriptionLabel.AutoSize = true;
        _descriptionLabel.MaximumSize = new Size(740, 0);
        _descriptionLabel.Name = "_descriptionLabel";
        _descriptionLabel.Size = new Size(740, 40);
        _descriptionLabel.TabIndex = 1;
        _descriptionLabel.Text = "初心者おすすめ設定は、見やすさと安全性を優先した初期値です。必要なら少しだけ変更できます。";

        _editRecommendedSettingsCheckBox.AutoSize = true;
        _editRecommendedSettingsCheckBox.Name = "_editRecommendedSettingsCheckBox";
        _editRecommendedSettingsCheckBox.Size = new Size(210, 24);
        _editRecommendedSettingsCheckBox.TabIndex = 2;
        _editRecommendedSettingsCheckBox.Text = "おすすめ設定を少し変更する";
        _editRecommendedSettingsCheckBox.UseVisualStyleBackColor = true;
        _editRecommendedSettingsCheckBox.CheckedChanged += EditRecommendedSettingsCheckBox_CheckedChanged;

        _displayGroupBox.Controls.Add(_displayPanel);
        _displayGroupBox.Margin = new Padding(0, 8, 0, 10);
        _displayGroupBox.Name = "_displayGroupBox";
        _displayGroupBox.Size = new Size(740, 190);
        _displayGroupBox.TabIndex = 3;
        _displayGroupBox.TabStop = false;
        _displayGroupBox.Text = "表示";

        _displayPanel.Controls.Add(_displayDescriptionLabel);
        _displayPanel.Controls.Add(_typeDisplayModeLabel);
        _displayPanel.Controls.Add(_typeDisplayModeComboBox);
        _displayPanel.Controls.Add(_showBeginnerHintsCheckBox);
        _displayPanel.Controls.Add(_showIconLegendCheckBox);
        _displayPanel.Controls.Add(_showOperationStatusCheckBox);
        _displayPanel.FlowDirection = FlowDirection.TopDown;
        _displayPanel.Location = new Point(12, 26);
        _displayPanel.Name = "_displayPanel";
        _displayPanel.Size = new Size(710, 152);
        _displayPanel.TabIndex = 0;
        _displayPanel.WrapContents = false;

        _displayDescriptionLabel.ForeColor = SystemColors.GrayText;
        _displayDescriptionLabel.Name = "_displayDescriptionLabel";
        _displayDescriptionLabel.Size = new Size(690, 24);
        _displayDescriptionLabel.Text = "一覧の見え方と、画面下に出す説明を選びます。";

        _typeDisplayModeLabel.Name = "_typeDisplayModeLabel";
        _typeDisplayModeLabel.Size = new Size(690, 24);
        _typeDisplayModeLabel.Text = "種類表示";

        _typeDisplayModeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        _typeDisplayModeComboBox.Items.AddRange(new object[] { "アイコン＋文字", "アイコンのみ", "文字のみ", "非表示（非推奨）" });
        _typeDisplayModeComboBox.Name = "_typeDisplayModeComboBox";
        _typeDisplayModeComboBox.Size = new Size(180, 28);
        _typeDisplayModeComboBox.TabIndex = 0;

        _showBeginnerHintsCheckBox.AutoSize = true;
        _showBeginnerHintsCheckBox.Name = "_showBeginnerHintsCheckBox";
        _showBeginnerHintsCheckBox.Size = new Size(170, 24);
        _showBeginnerHintsCheckBox.TabIndex = 1;
        _showBeginnerHintsCheckBox.Text = "初心者向け説明を表示";
        _showBeginnerHintsCheckBox.UseVisualStyleBackColor = true;

        _showIconLegendCheckBox.AutoSize = true;
        _showIconLegendCheckBox.Name = "_showIconLegendCheckBox";
        _showIconLegendCheckBox.Size = new Size(170, 24);
        _showIconLegendCheckBox.TabIndex = 2;
        _showIconLegendCheckBox.Text = "アイコンの意味を表示";
        _showIconLegendCheckBox.UseVisualStyleBackColor = true;

        _showOperationStatusCheckBox.AutoSize = true;
        _showOperationStatusCheckBox.Name = "_showOperationStatusCheckBox";
        _showOperationStatusCheckBox.Size = new Size(190, 24);
        _showOperationStatusCheckBox.TabIndex = 3;
        _showOperationStatusCheckBox.Text = "操作結果ステータスを表示";
        _showOperationStatusCheckBox.UseVisualStyleBackColor = true;

        _dragDropGroupBox.Controls.Add(_dragDropPanel);
        _dragDropGroupBox.Margin = new Padding(0, 8, 0, 10);
        _dragDropGroupBox.Name = "_dragDropGroupBox";
        _dragDropGroupBox.Size = new Size(740, 280);
        _dragDropGroupBox.TabIndex = 4;
        _dragDropGroupBox.TabStop = false;
        _dragDropGroupBox.Text = "ドラッグ＆ドロップ";

        _dragDropPanel.Controls.Add(_dragDropDescriptionLabel);
        _dragDropPanel.Controls.Add(_confirmTitleOnDropAddCheckBox);
        _dragDropPanel.Controls.Add(_focusExistingItemOnDuplicateCheckBox);
        _dragDropPanel.Controls.Add(_enableGroupDropModifierShortcutsCheckBox);
        _dragDropPanel.Controls.Add(_confirmGroupDropCopyMoveCheckBox);
        _dragDropPanel.Controls.Add(_enableItemDragReorderCheckBox);
        _dragDropPanel.Controls.Add(_enableExternalFileDropOutCheckBox);
        _dragDropPanel.Controls.Add(_enableExternalUrlTextDragOutCheckBox);
        _dragDropPanel.Controls.Add(_enableExternalTemplateTextDragOutCheckBox);
        _dragDropPanel.FlowDirection = FlowDirection.TopDown;
        _dragDropPanel.Location = new Point(12, 26);
        _dragDropPanel.Name = "_dragDropPanel";
        _dragDropPanel.Size = new Size(710, 240);
        _dragDropPanel.TabIndex = 0;
        _dragDropPanel.WrapContents = false;

        _dragDropDescriptionLabel.ForeColor = SystemColors.GrayText;
        _dragDropDescriptionLabel.Name = "_dragDropDescriptionLabel";
        _dragDropDescriptionLabel.Size = new Size(690, 24);
        _dragDropDescriptionLabel.Text = "登録するとき、または他のアプリへ渡すときの動きを選びます。";

        _confirmTitleOnDropAddCheckBox.AutoSize = true;
        _confirmTitleOnDropAddCheckBox.Name = "_confirmTitleOnDropAddCheckBox";
        _confirmTitleOnDropAddCheckBox.Size = new Size(210, 24);
        _confirmTitleOnDropAddCheckBox.Text = "追加時にタイトルを確認する";
        _confirmTitleOnDropAddCheckBox.UseVisualStyleBackColor = true;

        _focusExistingItemOnDuplicateCheckBox.AutoSize = true;
        _focusExistingItemOnDuplicateCheckBox.Name = "_focusExistingItemOnDuplicateCheckBox";
        _focusExistingItemOnDuplicateCheckBox.Size = new Size(250, 24);
        _focusExistingItemOnDuplicateCheckBox.Text = "重複していたら既存項目へ移動する";
        _focusExistingItemOnDuplicateCheckBox.UseVisualStyleBackColor = true;

        _enableGroupDropModifierShortcutsCheckBox.AutoSize = true;
        _enableGroupDropModifierShortcutsCheckBox.Name = "_enableGroupDropModifierShortcutsCheckBox";
        _enableGroupDropModifierShortcutsCheckBox.Size = new Size(300, 24);
        _enableGroupDropModifierShortcutsCheckBox.Text = "Ctrl/Shiftキーでコピー・移動を切り替える";
        _enableGroupDropModifierShortcutsCheckBox.UseVisualStyleBackColor = true;

        _confirmGroupDropCopyMoveCheckBox.AutoSize = true;
        _confirmGroupDropCopyMoveCheckBox.Name = "_confirmGroupDropCopyMoveCheckBox";
        _confirmGroupDropCopyMoveCheckBox.Size = new Size(260, 24);
        _confirmGroupDropCopyMoveCheckBox.Text = "グループへドロップしたときに確認する";
        _confirmGroupDropCopyMoveCheckBox.UseVisualStyleBackColor = true;

        _enableItemDragReorderCheckBox.AutoSize = true;
        _enableItemDragReorderCheckBox.Name = "_enableItemDragReorderCheckBox";
        _enableItemDragReorderCheckBox.Size = new Size(210, 24);
        _enableItemDragReorderCheckBox.Text = "項目をドラッグして並び替える";
        _enableItemDragReorderCheckBox.UseVisualStyleBackColor = true;

        _enableExternalFileDropOutCheckBox.AutoSize = true;
        _enableExternalFileDropOutCheckBox.Name = "_enableExternalFileDropOutCheckBox";
        _enableExternalFileDropOutCheckBox.Size = new Size(300, 24);
        _enableExternalFileDropOutCheckBox.Text = "ファイルやURLを外へドラッグして渡せる";
        _enableExternalFileDropOutCheckBox.UseVisualStyleBackColor = true;

        _enableExternalUrlTextDragOutCheckBox.AutoSize = true;
        _enableExternalUrlTextDragOutCheckBox.Name = "_enableExternalUrlTextDragOutCheckBox";
        _enableExternalUrlTextDragOutCheckBox.Size = new Size(260, 24);
        _enableExternalUrlTextDragOutCheckBox.Text = "URLを外へドラッグして渡せる";
        _enableExternalUrlTextDragOutCheckBox.UseVisualStyleBackColor = true;

        _enableExternalTemplateTextDragOutCheckBox.AutoSize = true;
        _enableExternalTemplateTextDragOutCheckBox.Name = "_enableExternalTemplateTextDragOutCheckBox";
        _enableExternalTemplateTextDragOutCheckBox.Size = new Size(300, 24);
        _enableExternalTemplateTextDragOutCheckBox.Text = "テンプレート本文を外へドラッグして渡せる";
        _enableExternalTemplateTextDragOutCheckBox.UseVisualStyleBackColor = true;

        _closeBehaviorGroupBox.Controls.Add(_closeBehaviorPanel);
        _closeBehaviorGroupBox.Margin = new Padding(0, 8, 0, 10);
        _closeBehaviorGroupBox.Name = "_closeBehaviorGroupBox";
        _closeBehaviorGroupBox.Size = new Size(740, 100);
        _closeBehaviorGroupBox.TabIndex = 5;
        _closeBehaviorGroupBox.TabStop = false;
        _closeBehaviorGroupBox.Text = "閉じるときの動作";

        _closeBehaviorPanel.Controls.Add(_closeBehaviorDescriptionLabel);
        _closeBehaviorPanel.Controls.Add(_minimizeToTrayOnCloseCheckBox);
        _closeBehaviorPanel.FlowDirection = FlowDirection.TopDown;
        _closeBehaviorPanel.Location = new Point(12, 26);
        _closeBehaviorPanel.Name = "_closeBehaviorPanel";
        _closeBehaviorPanel.Size = new Size(710, 62);
        _closeBehaviorPanel.WrapContents = false;

        _closeBehaviorDescriptionLabel.ForeColor = SystemColors.GrayText;
        _closeBehaviorDescriptionLabel.Size = new Size(690, 24);
        _closeBehaviorDescriptionLabel.Text = "アプリを閉じたとき、完全終了するか右下にしまうかを選びます。";

        _minimizeToTrayOnCloseCheckBox.AutoSize = true;
        _minimizeToTrayOnCloseCheckBox.Name = "_minimizeToTrayOnCloseCheckBox";
        _minimizeToTrayOnCloseCheckBox.Size = new Size(250, 24);
        _minimizeToTrayOnCloseCheckBox.Text = "閉じるボタンでタスクトレイに格納する";
        _minimizeToTrayOnCloseCheckBox.UseVisualStyleBackColor = true;

        _contextMenuGroupBox.Controls.Add(_contextMenuPanel);
        _contextMenuGroupBox.Margin = new Padding(0, 8, 0, 10);
        _contextMenuGroupBox.Name = "_contextMenuGroupBox";
        _contextMenuGroupBox.Size = new Size(740, 100);
        _contextMenuGroupBox.TabIndex = 6;
        _contextMenuGroupBox.TabStop = false;
        _contextMenuGroupBox.Text = "右クリックの便利機能";

        _contextMenuPanel.Controls.Add(_contextMenuDescriptionLabel);
        _contextMenuPanel.Controls.Add(_enableContextMenuDetailsCheckBox);
        _contextMenuPanel.FlowDirection = FlowDirection.TopDown;
        _contextMenuPanel.Location = new Point(12, 26);
        _contextMenuPanel.Name = "_contextMenuPanel";
        _contextMenuPanel.Size = new Size(710, 62);
        _contextMenuPanel.WrapContents = false;

        _contextMenuDescriptionLabel.ForeColor = SystemColors.GrayText;
        _contextMenuDescriptionLabel.Size = new Size(690, 24);
        _contextMenuDescriptionLabel.Text = "項目を右クリックしたときに使える操作を増やします。";

        _enableContextMenuDetailsCheckBox.AutoSize = true;
        _enableContextMenuDetailsCheckBox.Name = "_enableContextMenuDetailsCheckBox";
        _enableContextMenuDetailsCheckBox.Size = new Size(260, 24);
        _enableContextMenuDetailsCheckBox.Text = "右クリックの詳細操作を使えるようにする";
        _enableContextMenuDetailsCheckBox.UseVisualStyleBackColor = true;

        _backupDeleteGroupBox.Controls.Add(_backupDeletePanel);
        _backupDeleteGroupBox.Margin = new Padding(0, 8, 0, 10);
        _backupDeleteGroupBox.Name = "_backupDeleteGroupBox";
        _backupDeleteGroupBox.Size = new Size(740, 170);
        _backupDeleteGroupBox.TabIndex = 7;
        _backupDeleteGroupBox.TabStop = false;
        _backupDeleteGroupBox.Text = "バックアップと削除";

        _backupDeletePanel.Controls.Add(_backupDeleteDescriptionLabel);
        _backupDeletePanel.Controls.Add(_autoBackupEnabledCheckBox);
        _backupDeletePanel.Controls.Add(_maxBackupCountLabel);
        _backupDeletePanel.Controls.Add(_maxBackupCountNumeric);
        _backupDeletePanel.Controls.Add(_confirmBeforeDeleteCheckBox);
        _backupDeletePanel.Controls.Add(_moveDeletedItemsToTrashCheckBox);
        _backupDeletePanel.FlowDirection = FlowDirection.TopDown;
        _backupDeletePanel.Location = new Point(12, 26);
        _backupDeletePanel.Name = "_backupDeletePanel";
        _backupDeletePanel.Size = new Size(710, 132);
        _backupDeletePanel.WrapContents = false;

        _backupDeleteDescriptionLabel.ForeColor = SystemColors.GrayText;
        _backupDeleteDescriptionLabel.Size = new Size(690, 24);
        _backupDeleteDescriptionLabel.Text = "登録内容と設定を守り、間違って消しにくくするための設定です。";

        _autoBackupEnabledCheckBox.AutoSize = true;
        _autoBackupEnabledCheckBox.Name = "_autoBackupEnabledCheckBox";
        _autoBackupEnabledCheckBox.Size = new Size(170, 24);
        _autoBackupEnabledCheckBox.Text = "自動バックアップを使う";
        _autoBackupEnabledCheckBox.UseVisualStyleBackColor = true;

        _maxBackupCountLabel.Size = new Size(180, 24);
        _maxBackupCountLabel.Text = "バックアップ保存数";

        _maxBackupCountNumeric.Maximum = 100;
        _maxBackupCountNumeric.Minimum = 1;
        _maxBackupCountNumeric.Name = "_maxBackupCountNumeric";
        _maxBackupCountNumeric.Size = new Size(80, 27);
        _maxBackupCountNumeric.Value = 20;

        _confirmBeforeDeleteCheckBox.AutoSize = true;
        _confirmBeforeDeleteCheckBox.Name = "_confirmBeforeDeleteCheckBox";
        _confirmBeforeDeleteCheckBox.Size = new Size(140, 24);
        _confirmBeforeDeleteCheckBox.Text = "削除前に確認する";
        _confirmBeforeDeleteCheckBox.UseVisualStyleBackColor = true;

        _moveDeletedItemsToTrashCheckBox.AutoSize = true;
        _moveDeletedItemsToTrashCheckBox.Name = "_moveDeletedItemsToTrashCheckBox";
        _moveDeletedItemsToTrashCheckBox.Size = new Size(240, 24);
        _moveDeletedItemsToTrashCheckBox.Text = "削除時にアプリ内のごみ箱へ移動する";
        _moveDeletedItemsToTrashCheckBox.UseVisualStyleBackColor = true;

        _searchGroupBox.Controls.Add(_searchPanel);
        _searchGroupBox.Margin = new Padding(0, 8, 0, 10);
        _searchGroupBox.Name = "_searchGroupBox";
        _searchGroupBox.Size = new Size(740, 100);
        _searchGroupBox.TabIndex = 8;
        _searchGroupBox.TabStop = false;
        _searchGroupBox.Text = "検索";

        _searchPanel.Controls.Add(_searchDescriptionLabel);
        _searchPanel.Controls.Add(_searchTemplateBodyCheckBox);
        _searchPanel.FlowDirection = FlowDirection.TopDown;
        _searchPanel.Location = new Point(12, 26);
        _searchPanel.Name = "_searchPanel";
        _searchPanel.Size = new Size(710, 62);
        _searchPanel.WrapContents = false;

        _searchDescriptionLabel.ForeColor = SystemColors.GrayText;
        _searchDescriptionLabel.Size = new Size(690, 24);
        _searchDescriptionLabel.Text = "項目を探すとき、どこまで検索対象にするかを選びます。";

        _searchTemplateBodyCheckBox.AutoSize = true;
        _searchTemplateBodyCheckBox.Name = "_searchTemplateBodyCheckBox";
        _searchTemplateBodyCheckBox.Size = new Size(210, 24);
        _searchTemplateBodyCheckBox.Text = "テンプレート本文も検索する";
        _searchTemplateBodyCheckBox.UseVisualStyleBackColor = true;

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(_rootPanel);
        Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        Name = "UsabilitySettingsStepControl";
        Size = new Size(780, 580);
        ((System.ComponentModel.ISupportInitialize)_maxBackupCountNumeric).EndInit();
        _searchPanel.ResumeLayout(false);
        _searchPanel.PerformLayout();
        _searchGroupBox.ResumeLayout(false);
        _backupDeletePanel.ResumeLayout(false);
        _backupDeletePanel.PerformLayout();
        _backupDeleteGroupBox.ResumeLayout(false);
        _contextMenuPanel.ResumeLayout(false);
        _contextMenuPanel.PerformLayout();
        _contextMenuGroupBox.ResumeLayout(false);
        _closeBehaviorPanel.ResumeLayout(false);
        _closeBehaviorPanel.PerformLayout();
        _closeBehaviorGroupBox.ResumeLayout(false);
        _dragDropPanel.ResumeLayout(false);
        _dragDropPanel.PerformLayout();
        _dragDropGroupBox.ResumeLayout(false);
        _displayPanel.ResumeLayout(false);
        _displayPanel.PerformLayout();
        _displayGroupBox.ResumeLayout(false);
        _rootPanel.ResumeLayout(false);
        _rootPanel.PerformLayout();
        ResumeLayout(false);
    }
}
