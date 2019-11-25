using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace Alex_zigendanzetu
{
    public partial class zigendanzetu : UserControl, IActPluginV1
    {
        private static readonly string PluginName = "Alex_zigendanzetu";
        private SettingsSerializer xmlSettings;
        private bool combatFlg = false;
        private bool initButtoleFlg = false;
        private string MyName = "";

        Terop terop = new Terop();
        private Size teropssize = new Size();
        private Size teropspicsize = new Size();

        // 次元断絶設定用の変数
        private Dictionary<int, string> リミッターカットdic = new Dictionary<int, string>();
        private bool 次元startFlg = false;

        // 未来観測α設定用の変数
        private bool 未来確定αflg = false;
        private string MyJobName = "";
        private string LogMyJobName = "";


        public zigendanzetu()
        {
            InitializeComponent();
            ActPluginHelper.ActHelper.Initialize();
        }

        public void DeInitPlugin()
        {
            ActPluginHelper.ACTInitSetting.SaveSettings(xmlSettings,PluginName);
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            //lbStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
                                   // MultiProject.BasePlugin.xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance

            pluginScreenSpace.Text = "絶アレキ_MaliktenderOnly";
            pluginStatusText.Text = "AbsoluteAlexanderPluginStarts";

            // インターフェイス情報を格納する
            this.xmlSettings = new SettingsSerializer(this);

            // コントロール情報を取得する
            Control[] ct = ActPluginHelper.ACTInitSetting.GetAllControls(this);

            // 取得したコントロール情報を全て回し、初期表示用の情報を格納する
            foreach (Control tempct in ct)
            {
                if (tempct.Name.IndexOf("_init") > 0)
                {
                    // コントロールリストの情報を格納する
                    this.xmlSettings.AddControlSetting(tempct.Name, tempct);
                }
            }

            // 設定ファイルを読み込む
            ActPluginHelper.ACTInitSetting.LoadSettings(xmlSettings, PluginName);

            // 空だった場合
            if (string.IsNullOrEmpty(textBox1_path_init.Text))
            {
            }

            // ログを取得するイベントを生成する
            ActGlobals.oFormActMain.OnLogLineRead += OFormActMain_OnLogLineRead;
            // 戦闘開始をチェックする
            ActGlobals.oFormActMain.OnCombatStart += OFormActMain_OnCombatStart;
            // 戦闘終了をキャッチする
            ActGlobals.oFormActMain.OnCombatEnd += OFormActMain_OnCombatEnd;

            // リミッターカット番号
            リミッターカットdic = new Dictionary<int, string>();
            リミッターカットdic.Add(1, ":004F:0000:");
            リミッターカットdic.Add(2, ":0050:0000:");
            リミッターカットdic.Add(3, ":0051:0000:");
            リミッターカットdic.Add(4, ":0052:0000:");
            リミッターカットdic.Add(5, ":0053:0000:");
            リミッターカットdic.Add(6, ":0054:0000:");
            リミッターカットdic.Add(7, ":0055:0000:");
            リミッターカットdic.Add(8, ":0056:0000:");

            // フォームの初期化
            terop.pictureBox1.Visible = true;
            terop.pictureBox2.Visible = true;
            terop.pictureBox3.Visible = true;
            terop.pictureBox4.Visible = true;
            terop.pictureBox5.Visible = true;
            terop.pictureBox6.Visible = true;
            terop.pictureBox7.Visible = true;
            terop.pictureBox8.Visible = true;
            terop.pictureBox9.Visible = true;
            terop.pictureBox10.Visible = true;
            terop.pictureBox11.Visible = true;
            terop.pictureBox12.Visible = true;
            terop.Show();
            terop.Hide();
            terop.pictureBox1.Visible = false;
            terop.pictureBox2.Visible = false;
            terop.pictureBox3.Visible = false;
            terop.pictureBox4.Visible = false;
            terop.pictureBox5.Visible = false;
            terop.pictureBox6.Visible = false;
            terop.pictureBox7.Visible = false;
            terop.pictureBox8.Visible = false;
            terop.pictureBox9.Visible = false;
            terop.pictureBox10.Visible = false;
            terop.pictureBox11.Visible = false;
            terop.pictureBox12.Visible = false;


        }

        /// <summary>
        /// formの初期化
        /// </summary>
        private void formInit()
        {
            terop.pictureBox1.Visible = false;
            terop.pictureBox2.Visible = false;
            terop.pictureBox3.Visible = false;
            terop.pictureBox4.Visible = false;
            terop.pictureBox5.Visible = false;
            terop.pictureBox6.Visible = false;
            terop.pictureBox7.Visible = false;
            terop.pictureBox8.Visible = false;
            terop.pictureBox9.Visible = false;
            terop.pictureBox10.Visible = false;
            terop.pictureBox11.Visible = false;
            terop.pictureBox12.Visible = false;
            terop.Hide();
        }

        /// <summary>
        /// logの読み込みイベント
        /// </summary>
        /// <param name="isImport"></param>
        /// <param name="logInfo"></param>
        private void OFormActMain_OnLogLineRead(bool isImport, LogLineEventArgs logInfo)
        {
            try
            {


            // 18文字以下のログは読み捨てる
            // なぜならば、タイムスタンプ＋ログタイプのみのログだから
            if (logInfo.logLine.Length <= 18)
            {
                return;
            }
            // -------------------------- 戦闘開始時の処理 --------------------------
            // 戦闘開始のお知らせ
            if (combatFlg && !initButtoleFlg)
            {
                // 戦闘前の初期処理
                initButtoleFlg = true;
                未来確定αflg = false;
                MyName = ActPluginHelper.ActHelper.MyName();
                MyJobName = ActPluginHelper.ActHelper.MyJob();

                formInit();

                // 次元断絶用の処理
                if (!string.IsNullOrEmpty(textBox1_path_init.Text) &&
                    ActPluginHelper.FileOutPut.checkDirectory(textBox1_path_init.Text))
                {
                    次元startFlg = false;
                }
            }
            // -------------------------- 戦闘開始時の処理 --------------------------

            // -------------------------- 戦闘終了時の処理 --------------------------
            // 戦闘終了時
            if (!combatFlg && initButtoleFlg)
            {
                initButtoleFlg = false;

                formInit();

                // 次元断絶用の処理
                次元startFlg = false;
            }
                // -------------------------- 戦闘終了時の処理 --------------------------


            // -------------------------- 未来観測α --------------------------
            if (logInfo.logLine.Contains("は「未来観測α」の構え。"))
            {
                未来確定αflg = true;
            }
            if (未来確定αflg) { 
                Regex regex = new Regex(@"^.*03:([A-Z0-9]{8}):Added new combatant.*  Job: " + MyJobName + " Level: 80 .*");
                if (regex.IsMatch(logInfo.logLine))
                {
                    LogMyJobName = regex.Replace(logInfo.logLine, "$1");
                }

                // 定数が取れた場合のみ、処理に入れる
                if (!string.IsNullOrEmpty(LogMyJobName))
                {
                    // ここで、logとマッチさせる処理を入れる
                    if (logInfo.logLine.Contains(LogMyJobName))
                    {

                    }
                }

            }
            // -------------------------- 未来観測α --------------------------
            }
            catch (Exception)
            {
                // エラーをすべて握りつぶす
            }
        }

        private Image GetImageFile(int number)
        {
            string imgPth = textBox1_path_init.Text + number.ToString() + "\\1.jpg";
            return Image.FromFile(@imgPth);
        }


        /// <summary>
        /// 戦闘開始時のイベント
        /// </summary>
        /// <param name="isImport"></param>
        /// <param name="encounterInfo"></param>
        private void OFormActMain_OnCombatStart(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            combatFlg = true;
        }
        /// <summary>
        /// 戦闘終了時のイベント
        /// </summary>
        /// <param name="isImport"></param>
        /// <param name="encounterInfo"></param>
        private void OFormActMain_OnCombatEnd(bool isImport, CombatToggleEventArgs encounterInfo)
        {
            combatFlg = false;
        }

        /// <summary>
        /// フォルダ選択ボタン押下時の処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBox1_path_init.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }




        private void button1_表示位置確認_Click_1(object sender, EventArgs e)
        {
            if (textBox_位置確認.Text == "位置確認")
            {
                int X = textBox_zahyoX_init.Text == "" ? 100 : int.Parse(textBox_zahyoX_init.Text);
                int Y = textBox_zahyoY_init.Text == "" ? 100 : int.Parse(textBox_zahyoY_init.Text);
                Point point = new Point(X, Y);
                // 位置を指定してしまう
                terop.Location = point;

                terop.Show();

                textBox_位置確認.Text = "確認終了";
                button1_表示位置確認.Text = "確認終了";
            }
            else
            {
                terop.Hide();

                textBox_位置確認.Text = "位置確認";
                button1_表示位置確認.Text = "位置確認";
            }
        }

        private void listBox_倍率_init_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            BairituSetting();
        }

        private void BairituSetting()
        {
            listBox_倍率_text_init.Text = listBox_倍率_init.Text;

            double bairitsu = double.Parse(listBox_倍率_text_init.Text.ToString().Replace("倍", ""));

            terop.Size = new Size((int)(teropssize.Width * bairitsu), (int)(teropssize.Height * bairitsu));
            terop.pictureBox1.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox2.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox3.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox4.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox5.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox6.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox7.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox8.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox9.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox10.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox11.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
            terop.pictureBox12.Size = new Size((int)(teropspicsize.Width * bairitsu), (int)(teropspicsize.Height * bairitsu));
        }

        private void textBox_zahyoX_init_TextChanged(object sender, EventArgs e)
        {
            try
            {
                terop.Location = SettingPoint(textBox_zahyoX_init, textBox_zahyoY_init);
            }
            catch
            { }
        }

        private void textBox_zahyoY_init_TextChanged(object sender, EventArgs e)
        {
            try
            {
                terop.Location = SettingPoint(textBox_zahyoX_init, textBox_zahyoY_init);
            }
            catch
            { }
        }

        private Point SettingPoint(TextBox textBoxX, TextBox textBoxY)
        {
            int X = textBoxX.Text == "" ? 100 : int.Parse(textBoxX.Text);
            int Y = textBoxY.Text == "" ? 100 : int.Parse(textBoxY.Text);
            return new Point(X, Y);
        }
    }
}
