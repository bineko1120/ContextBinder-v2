namespace ContextBinder.Forms;

public partial class MainFormDesignDraft : Form
{
    public MainFormDesignDraft()
    {
        InitializeComponent();
        ApplyDesignerContractCorrections();
    }

    /// <summary>
    /// Designerで確定した表示契約を、機能接続前の段階でも維持するための補正です。
    ///
    /// 現在のDesigner生成コードでは、グループ追加の文字ボタンとアイコンボタンの
    /// Beginner／Compact用Tagが逆になっているため、実行時とWinFormsUiViewerでは
    /// 正式契約どおりに補正します。
    ///
    /// Codexが内部名を整理する際は、Designer側のTag値も正式契約へ合わせたうえで、
    /// この補正が不要になったか確認してください。
    /// </summary>
    private void ApplyDesignerContractCorrections()
    {
        addGroupButton.Tag = "view:Beginner";
        addGroupIconButton.Tag = "view:Compact";
    }

    private void bottomInformationPanel_Paint(object sender, PaintEventArgs e)
    {

    }

    private void mainMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
    {

    }

    private void groupTitleLabel_Click(object sender, EventArgs e)
    {

    }

    private void groupTitleLabel_Click_1(object sender, EventArgs e)
    {

    }

    private void searchTextBox_TextChanged(object sender, EventArgs e)
    {

    }

    private void searchBoxLavel_Click(object sender, EventArgs e)
    {

    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {

    }

    private void typeFilterTitleLabel_Click(object sender, EventArgs e)
    {

    }

    private void searchInputPanel_Paint(object sender, PaintEventArgs e)
    {

    }
}
