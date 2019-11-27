using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.Text.RegularExpressions;

namespace Alex_dammyDiscrimination
{
    public partial class dammyMain : UserControl, IActPluginV1
    {
        private static readonly string PluginName = "Alex_dammyDiscrimination";
        private SettingsSerializer xmlSettings;
        private bool combatFlg = false;
        private bool initButtoleFlg = false;

        // 未来観測α設定用の変数
        private bool 未来確定αflg = false;
        private bool 未来確定βflg = false;
        private string MyJobName = "";
        private string LogMyJobName = "";
        private Dictionary<int, string> dammyAlex = new Dictionary<int, string>();
        private Dictionary<int, string> dammyAlex2 = new Dictionary<int, string>();
        private List<int> dammyList = new List<int>();
        private int dammy拝火 = 0;
        // 加重罰
        private List<string> 加重罰List = new List<string>();
        // 集団罰
        private List<string> 集団罰List = new List<string>();
        // 名誉罰
        private List<string> 名誉罰List = new List<string>();

        // 停止命令 flg
        private bool 停止命令flg = false;

        // 行動命令 flg
        private bool 行動命令flg = false;

        private bool αStopFlg = false;
        private bool 名誉罰用flg = false;

        private DammyForm dammyForm = new DammyForm();
        private DammyForm2 dammyForm2 = new DammyForm2();

        private Size dammyFormSize = new Size();
        private Size dammyFormpictureBoxSize = new Size();
        private Size dammyForm2Size = new Size();
        private Size dammyForm2pictureBoxSize = new Size();

        public dammyMain()
        {
            InitializeComponent();
            ActHelper.Initialize();
        }

        public void DeInitPlugin()
        {
            ACTInitSetting.SaveSettings(xmlSettings, PluginName);
            dammyForm.Hide();
            dammyForm.Close();
            dammyForm2.Hide();
            dammyForm2.Close();

        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        { //lbStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
                                   // MultiProject.BasePlugin.xmlSettings = new SettingsSerializer(this); // Create a new settings serializer and pass it this instance

            pluginScreenSpace.Text = "dammyDiscrimination";
            pluginStatusText.Text = "Alex_dammyDiscrimination";

            // インターフェイス情報を格納する
            this.xmlSettings = new SettingsSerializer(this);

            // コントロール情報を取得する
            Control[] ct = ACTInitSetting.GetAllControls(this);

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
            ACTInitSetting.LoadSettings(xmlSettings, PluginName);


            dammyAlex = new Dictionary<int, string>();
            dammyAlex.Add(1, "489F:Unknown_489F.*:1000:78.*91");
            dammyAlex.Add(2, "489F:Unknown_489F.*:1000:91.*78");
            dammyAlex.Add(3, "489F:Unknown_489F.*:1000:108.*78");
            dammyAlex.Add(4, "489F:Unknown_489F.*:1000:121.*91");

            dammyAlex2 = new Dictionary<int, string>();
            dammyAlex2.Add(1, "489E:Unknown_489E.*:1000:117.*107");
            dammyAlex2.Add(2, "489E:Unknown_489E.*:1000:107.*117");
            dammyAlex2.Add(3, "489E:Unknown_489E.*:1000:92.*117");
            dammyAlex2.Add(4, "489E:Unknown_489E.*:1000:82.*107");

            dammyList = new List<int>();
            dammyList.Add(1);
            dammyList.Add(2);
            dammyList.Add(3);
            dammyList.Add(4);

            dammy拝火 = 0;

            // ログを取得するイベントを生成する
            ActGlobals.oFormActMain.BeforeLogLineRead += OFormActMain_OnLogLineRead;
            // 戦闘開始をチェックする
            ActGlobals.oFormActMain.OnCombatStart += OFormActMain_OnCombatStart;
            // 戦闘終了をキャッチする
            ActGlobals.oFormActMain.OnCombatEnd += OFormActMain_OnCombatEnd;



            dammyFormSize = new Size(364, 376);
            dammyFormpictureBoxSize = new Size(350, 350);

            dammyForm2Size = new Size(390, 220);
            dammyForm2pictureBoxSize = new Size(185, 185);

            // フォームの初期化
            formInit();

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
        /// formの初期化
        /// </summary>
        private void formInit()
        {
            dammyForm.Show();
            dammyForm.pictureBox1.Visible = true;
            dammyForm.pictureBox2.Visible = true;
            dammyForm.pictureBox3.Visible = true;
            dammyForm.pictureBox4.Visible = true;
            dammyForm.pictureBox5.Visible = true;
            dammyForm.pictureBox6.Visible = true;
            dammyForm.pictureBox7.Visible = true;
            dammyForm.pictureBox8.Visible = true;
            dammyForm.pictureBox9.Visible = true;
            dammyForm.pictureBox10.Visible = true;
            dammyForm.pictureBox11.Visible = true;
            dammyForm.pictureBox12.Visible = true;
            dammyForm.pictureBox1.Visible = false;
            dammyForm.pictureBox2.Visible = false;
            dammyForm.pictureBox3.Visible = false;
            dammyForm.pictureBox4.Visible = false;
            dammyForm.pictureBox5.Visible = false;
            dammyForm.pictureBox6.Visible = false;
            dammyForm.pictureBox7.Visible = false;
            dammyForm.pictureBox8.Visible = false;
            dammyForm.pictureBox9.Visible = false;
            dammyForm.pictureBox10.Visible = false;
            dammyForm.pictureBox11.Visible = false;
            dammyForm.pictureBox12.Visible = false;
            dammyForm.Hide();

            dammyForm2.Show();
            dammyForm2.pictureBox1.Visible = true;
            dammyForm2.pictureBox2.Visible = true;
            dammyForm2.pictureBox3.Visible = true;
            dammyForm2.pictureBox4.Visible = true;
            dammyForm2.pictureBox5.Visible = true;
            dammyForm2.pictureBox6.Visible = true;
            dammyForm2.pictureBox7.Visible = true;
            dammyForm2.pictureBox8.Visible = true;
            dammyForm2.pictureBox9.Visible = true;
            dammyForm2.pictureBox10.Visible = true;
            dammyForm2.pictureBox11.Visible = true;
            dammyForm2.pictureBox12.Visible = true;
            dammyForm2.pictureBox1.Visible = false;
            dammyForm2.pictureBox2.Visible = false;
            dammyForm2.pictureBox3.Visible = false;
            dammyForm2.pictureBox4.Visible = false;
            dammyForm2.pictureBox5.Visible = false;
            dammyForm2.pictureBox6.Visible = false;
            dammyForm2.pictureBox7.Visible = false;
            dammyForm2.pictureBox8.Visible = false;
            dammyForm2.pictureBox9.Visible = false;
            dammyForm2.pictureBox10.Visible = false;
            dammyForm2.pictureBox11.Visible = false;
            dammyForm2.pictureBox12.Visible = false;
            dammyForm2.Hide();

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
                    //MyJobName = ActHelper.MyJob();
                    MyJobName = textBox1_job_init.Text;

                    // 未来観測α用
                    dammyList = new List<int>();
                    dammyList.Add(1);
                    dammyList.Add(2);
                    dammyList.Add(3);
                    dammyList.Add(4);
                    dammy拝火 = 0;
                    加重罰List = new List<string>();
                    集団罰List = new List<string>();
                    名誉罰List = new List<string>();
                    停止命令flg = false;
                    行動命令flg = false;
                    名誉罰用flg = false;
                    αStopFlg = false;
                    formInit();

                }
                // -------------------------- 戦闘開始時の処理 --------------------------

                // -------------------------- 戦闘終了時の処理 --------------------------
                // 戦闘終了時
                if (!combatFlg && initButtoleFlg)
                {
                    // 未来観測α用
                    dammyList = new List<int>();
                    dammyList.Add(1);
                    dammyList.Add(2);
                    dammyList.Add(3);
                    dammyList.Add(4);
                    dammy拝火 = 0;
                    加重罰List = new List<string>();
                    集団罰List = new List<string>();
                    名誉罰List = new List<string>();
                    停止命令flg = false;
                    行動命令flg = false;
                    名誉罰用flg = false;
                    αStopFlg = false;
                    initButtoleFlg = false;

                    formInit();

                }
                // -------------------------- 戦闘終了時の処理 --------------------------

                // -------------------------- 未来観測α --------------------------
                if (logInfo.logLine.Contains("は「未来観測α」の構え。"))
                {
                    未来確定αflg = true;
                }
                if (未来確定αflg)
                {
                    // 定数が取れた場合のみ、処理に入れる
                    if (string.IsNullOrEmpty(LogMyJobName))
                    {
                        Regex regex = new Regex(@"^.*03:([A-Z0-9]{8}):Added new combatant.*  Job: " + MyJobName + " Level: 80 .*");
                        if (regex.IsMatch(logInfo.logLine))
                        {
                            LogMyJobName = regex.Replace(logInfo.logLine, "$1");
                        }
                    }
                    // 定数が取れた場合のみ、処理に入れる
                    if (!string.IsNullOrEmpty(LogMyJobName) && !αStopFlg)
                    {

                        // 停止命令
                        if (logInfo.logLine.Contains("4B0E:Unknown_4B0e"))
                        {
                            停止命令flg = true;
                        }
                        // 行動命令
                        else if (logInfo.logLine.Contains("4B0E:Unknown_4B0e"))
                        {
                            行動命令flg = true;
                        }



                        // 集団罰
                        if (logInfo.logLine.Contains("48A6:Unknown_48A6"))
                        {
                            Regex regex = new Regex(@"^.*15:([A-Z0-9]{8})::48A6:Unknown_48A6.*");
                            集団罰List.Add(regex.Replace(logInfo.logLine, "$1"));
                        }
                        // 加重罰
                        else if (logInfo.logLine.Contains("48A5:Unknown_48A5"))
                        {
                            Regex regex = new Regex(@"^.*15:([A-Z0-9]{8})::48A5:Unknown_48A5.*");
                            加重罰List.Add(regex.Replace(logInfo.logLine, "$1"));
                        }
                        /*
                        // 名誉罰
                        else if (logInfo.logLine.Contains("489A:Unknown_489A"))
                        {
                            名誉罰用flg = true;
                            Regex regex = new Regex(@"^.*15:([A-Z0-9]{8})::489A:Unknown_489A.*");
                            string targetJobLog = regex.Replace(logInfo.logLine, "$1");
                            if (!集団罰List.Contains(targetJobLog) || !加重罰List.Contains(targetJobLog))
                            {
                                名誉罰List.Add(targetJobLog);
                                αStopFlg = true;
                            }
                        }
                        else if (名誉罰用flg)
                        {
                            名誉罰用flg = false;
                            αStopFlg = true;
                        }
                        */

                        // 集団罰・加重罰が付いた場合は、即処理を中断する
                        if (集団罰List.Count == 1 && 加重罰List.Count == 3 && (停止命令flg || 行動命令flg))
                        {
                            αStopFlg = true;
                        }

                        // StopFlgが立っている場合のみ、後続処理を実施する
                        if (αStopFlg)
                        {
                            string TTSString = "";
                            dammyForm2.Show();
                            if (停止命令flg)
                            {
                                TTSString = "とまれー、";
                                dammyForm2.pictureBox1.Visible = false;
                                dammyForm2.pictureBox2.Visible = true;
                                dammyForm2.pictureBox6.Visible = false;
                                dammyForm2.pictureBox7.Visible = false;
                                dammyForm2.pictureBox8.Visible = false;
                                dammyForm2.pictureBox9.Visible = false;
                                dammyForm2.pictureBox10.Visible = false;
                                dammyForm2.pictureBox11.Visible = false;
                                dammyForm2.pictureBox12.Visible = false;
                            }
                            else if (行動命令flg)
                            {
                                TTSString = "うごけー、";
                                dammyForm2.pictureBox1.Visible = true;
                                dammyForm2.pictureBox2.Visible = false;
                                dammyForm2.pictureBox6.Visible = false;
                                dammyForm2.pictureBox7.Visible = false;
                                dammyForm2.pictureBox8.Visible = false;
                                dammyForm2.pictureBox9.Visible = false;
                                dammyForm2.pictureBox10.Visible = false;
                                dammyForm2.pictureBox11.Visible = false;
                                dammyForm2.pictureBox12.Visible = false;
                            }
                            // 自分が集団罰の場合の処理
                            if (集団罰List.Contains(LogMyJobName))
                            {
                                TTSString += "しゅうだんばつ";
                                dammyForm2.pictureBox3.Visible = false;
                                dammyForm2.pictureBox4.Visible = false;
                                dammyForm2.pictureBox5.Visible = true;
                                dammyForm2.pictureBox6.Visible = false;
                                dammyForm2.pictureBox7.Visible = false;
                                dammyForm2.pictureBox8.Visible = false;
                                dammyForm2.pictureBox9.Visible = false;
                                dammyForm2.pictureBox10.Visible = false;
                                dammyForm2.pictureBox11.Visible = false;
                                dammyForm2.pictureBox12.Visible = false;
                            }
                            // 自分が加重罰の場合の処理
                            else if (加重罰List.Contains(LogMyJobName))
                            {
                                TTSString += "かじゅうばつ";
                                dammyForm2.pictureBox3.Visible = false;
                                dammyForm2.pictureBox4.Visible = true;
                                dammyForm2.pictureBox5.Visible = false;
                                dammyForm2.pictureBox6.Visible = false;
                                dammyForm2.pictureBox7.Visible = false;
                                dammyForm2.pictureBox8.Visible = false;
                                dammyForm2.pictureBox9.Visible = false;
                                dammyForm2.pictureBox10.Visible = false;
                                dammyForm2.pictureBox11.Visible = false;
                                dammyForm2.pictureBox12.Visible = false;
                            }
                            // 自分が名誉罰の場合の処理
                            else 
                            {
                                TTSString += "めいよかむしょく";
                                dammyForm2.pictureBox3.Visible = true; // 名誉
                                dammyForm2.pictureBox4.Visible = false;
                                dammyForm2.pictureBox5.Visible = false;
                                dammyForm2.pictureBox6.Visible = false;　// 無色
                                dammyForm2.pictureBox7.Visible = false;
                                dammyForm2.pictureBox8.Visible = false;
                                dammyForm2.pictureBox9.Visible = false;
                                dammyForm2.pictureBox10.Visible = false;
                                dammyForm2.pictureBox11.Visible = false;
                                dammyForm2.pictureBox12.Visible = false;
                            }

                            if (!checkBox1_miraikansokugazou_init.Checked)
                            {
                                ActGlobals.oFormActMain.TTS(TTSString);
                            }
                        }
                    }

                    // ダミーアレキ観測
                    if (checkBox1_arekibimu_init.Checked)
                    {

                        // ダミーアレキ判定用
                        if (logInfo.logLine.Contains("489F:Unknown_489F"))
                        {
                            foreach (KeyValuePair<int, string> pair in dammyAlex)
                            {
                                Regex regex = new Regex(@pair.Value);
                                if (regex.IsMatch(logInfo.logLine))
                                {
                                    // マッチした場所の番号を除去する
                                    dammyList.Remove(pair.Key);
                                }
                            }
                        }

                        // リストが残り一つになった場合に処理を実施する
                        if (dammyList.Count == 1)
                        {
                            string TTSstr = "";
                            dammyForm.Show();
                            switch (dammyList[0])
                            {
                                case 1:
                                    TTSstr = "いちばんひだり";
                                    dammyForm.pictureBox1.Visible = true;
                                    dammyForm.pictureBox2.Visible = false;
                                    dammyForm.pictureBox3.Visible = false;
                                    dammyForm.pictureBox4.Visible = false;
                                    dammyForm.pictureBox5.Visible = false;
                                    dammyForm.pictureBox6.Visible = false;
                                    dammyForm.pictureBox7.Visible = false;
                                    dammyForm.pictureBox8.Visible = false;
                                    dammyForm.pictureBox9.Visible = false;
                                    dammyForm.pictureBox10.Visible = false;
                                    dammyForm.pictureBox11.Visible = false;
                                    dammyForm.pictureBox12.Visible = false;

                                    break;
                                case 2:
                                    TTSstr = "ひだりからにばんめ";
                                    dammyForm.pictureBox1.Visible = false;
                                    dammyForm.pictureBox2.Visible = true;
                                    dammyForm.pictureBox3.Visible = false;
                                    dammyForm.pictureBox4.Visible = false;
                                    dammyForm.pictureBox5.Visible = false;
                                    dammyForm.pictureBox6.Visible = false;
                                    dammyForm.pictureBox7.Visible = false;
                                    dammyForm.pictureBox8.Visible = false;
                                    dammyForm.pictureBox9.Visible = false;
                                    dammyForm.pictureBox10.Visible = false;
                                    dammyForm.pictureBox11.Visible = false;
                                    dammyForm.pictureBox12.Visible = false;
                                    break;
                                case 3:
                                    TTSstr = "みぎからにばんめ";
                                    dammyForm.pictureBox1.Visible = false;
                                    dammyForm.pictureBox2.Visible = false;
                                    dammyForm.pictureBox3.Visible = true;
                                    dammyForm.pictureBox4.Visible = false;
                                    dammyForm.pictureBox5.Visible = false;
                                    dammyForm.pictureBox6.Visible = false;
                                    dammyForm.pictureBox7.Visible = false;
                                    dammyForm.pictureBox8.Visible = false;
                                    dammyForm.pictureBox9.Visible = false;
                                    dammyForm.pictureBox10.Visible = false;
                                    dammyForm.pictureBox11.Visible = false;
                                    dammyForm.pictureBox12.Visible = false;

                                    break;
                                case 4:
                                    TTSstr = "いちばんみぎ";
                                    dammyForm.pictureBox1.Visible = false;
                                    dammyForm.pictureBox2.Visible = false;
                                    dammyForm.pictureBox3.Visible = false;
                                    dammyForm.pictureBox4.Visible = true;
                                    dammyForm.pictureBox5.Visible = false;
                                    dammyForm.pictureBox6.Visible = false;
                                    dammyForm.pictureBox7.Visible = false;
                                    dammyForm.pictureBox8.Visible = false;
                                    dammyForm.pictureBox9.Visible = false;
                                    dammyForm.pictureBox10.Visible = false;
                                    dammyForm.pictureBox11.Visible = false;
                                    dammyForm.pictureBox12.Visible = false;
                                    break;
                                default:
                                    dammyForm.Hide();
                                    TTSstr = "";
                                    dammyForm.pictureBox1.Visible = false;
                                    dammyForm.pictureBox2.Visible = false;
                                    dammyForm.pictureBox3.Visible = false;
                                    dammyForm.pictureBox4.Visible = false;
                                    dammyForm.pictureBox5.Visible = false;
                                    dammyForm.pictureBox6.Visible = false;
                                    dammyForm.pictureBox7.Visible = false;
                                    dammyForm.pictureBox8.Visible = false;
                                    dammyForm.pictureBox9.Visible = false;
                                    dammyForm.pictureBox10.Visible = false;
                                    dammyForm.pictureBox11.Visible = false;
                                    dammyForm.pictureBox12.Visible = false;
                                    break;
                            }
                            // 読上げを実施する
                            if (!checkBox1_arekibimugazou_init.Checked)
                            {
                                ActGlobals.oFormActMain.TTS(TTSstr);
                            }
                            dammyList = new List<int>();
                            dammyList.Add(1);
                            dammyList.Add(2);
                            dammyList.Add(3);
                            dammyList.Add(4);
                        }
                    }

                    // 未来観測αを終了する処理
                    if (logInfo.logLine.Contains("十字の秘蹟"))
                    {
                        dammyForm2.Hide();
                        dammyForm.Hide();
                        αStopFlg = false;
                        未来確定αflg = false;
                        停止命令flg = false;
                        行動命令flg = false;
                    }
                }

                // -------------------------- 未来観測α --------------------------

                // -------------------------- 未来観測β --------------------------
                // 要調整
                if (logInfo.logLine.Contains("は「未来観測β」の構え。"))
                {
                    未来確定βflg = true;
                }
                if (未来確定βflg)
                {
                    // 要調整
                    if (false)
                    {

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

                    // ダミーアレキ判定用
                    if (logInfo.logLine.Contains("489E:Unknown_489E"))
                    {
                        foreach (KeyValuePair<int, string> pair in dammyAlex2)
                        {
                            Regex regex2 = new Regex(@pair.Value);
                            if (regex2.IsMatch(logInfo.logLine))
                            {
                                // マッチした場所の番号を追加する
                                dammy拝火 = pair.Key;
                                break;
                            }
                        }
                    }

                    // dammy拝火が0ではなくなった場合、処理を実施する
                    if (dammy拝火 != 0)
                    {
                        string TTSstr = "";
                        dammyForm.Show();
                        switch (dammy拝火)
                        {
                            case 1:
                                TTSstr = "いちばんみぎ";
                                dammyForm.pictureBox1.Visible = false;
                                dammyForm.pictureBox2.Visible = false;
                                dammyForm.pictureBox3.Visible = false;
                                dammyForm.pictureBox4.Visible = false;
                                dammyForm.pictureBox5.Visible = true;
                                dammyForm.pictureBox6.Visible = false;
                                dammyForm.pictureBox7.Visible = false;
                                dammyForm.pictureBox8.Visible = false;
                                dammyForm.pictureBox9.Visible = false;
                                dammyForm.pictureBox10.Visible = false;
                                dammyForm.pictureBox11.Visible = false;
                                dammyForm.pictureBox12.Visible = false;

                                break;
                            case 2:
                                TTSstr = "みぎからにばんめ";
                                dammyForm.pictureBox1.Visible = false;
                                dammyForm.pictureBox2.Visible = false;
                                dammyForm.pictureBox3.Visible = false;
                                dammyForm.pictureBox4.Visible = false;
                                dammyForm.pictureBox5.Visible = false;
                                dammyForm.pictureBox6.Visible = true;
                                dammyForm.pictureBox7.Visible = false;
                                dammyForm.pictureBox8.Visible = false;
                                dammyForm.pictureBox9.Visible = false;
                                dammyForm.pictureBox10.Visible = false;
                                dammyForm.pictureBox11.Visible = false;
                                dammyForm.pictureBox12.Visible = false;

                                break;
                            case 3:
                                TTSstr = "ひだりからにばんめ";
                                dammyForm.pictureBox1.Visible = false;
                                dammyForm.pictureBox2.Visible = false;
                                dammyForm.pictureBox3.Visible = false;
                                dammyForm.pictureBox4.Visible = false;
                                dammyForm.pictureBox5.Visible = false;
                                dammyForm.pictureBox6.Visible = false;
                                dammyForm.pictureBox7.Visible = true;
                                dammyForm.pictureBox8.Visible = false;
                                dammyForm.pictureBox9.Visible = false;
                                dammyForm.pictureBox10.Visible = false;
                                dammyForm.pictureBox11.Visible = false;
                                dammyForm.pictureBox12.Visible = false;

                                break;
                            case 4:
                                TTSstr = "いちばんひだり";
                                dammyForm.pictureBox1.Visible = false;
                                dammyForm.pictureBox2.Visible = false;
                                dammyForm.pictureBox3.Visible = false;
                                dammyForm.pictureBox4.Visible = false;
                                dammyForm.pictureBox5.Visible = false;
                                dammyForm.pictureBox6.Visible = false;
                                dammyForm.pictureBox7.Visible = false;
                                dammyForm.pictureBox8.Visible = true;
                                dammyForm.pictureBox9.Visible = false;
                                dammyForm.pictureBox10.Visible = false;
                                dammyForm.pictureBox11.Visible = false;
                                dammyForm.pictureBox12.Visible = false;

                                break;
                            default:
                                dammyForm.Hide();
                                TTSstr = "";
                                dammyForm.pictureBox1.Visible = false;
                                dammyForm.pictureBox2.Visible = false;
                                dammyForm.pictureBox3.Visible = false;
                                dammyForm.pictureBox4.Visible = false;
                                dammyForm.pictureBox5.Visible = false;
                                dammyForm.pictureBox6.Visible = false;
                                dammyForm.pictureBox7.Visible = false;
                                dammyForm.pictureBox8.Visible = false;
                                dammyForm.pictureBox9.Visible = false;
                                dammyForm.pictureBox10.Visible = false;
                                dammyForm.pictureBox11.Visible = false;
                                dammyForm.pictureBox12.Visible = false;
                                break;
                        }
                        // 読上げを実施する
                        if (!checkBox1_arekibimugazou_init.Checked)
                        {
                            ActGlobals.oFormActMain.TTS(TTSstr);
                        }
                        dammy拝火 = 0;
                    }

                    if (logInfo.logLine.Contains("拝火の秘蹟"))
                    {
                        未来確定βflg = false;
                        dammyForm.Hide();
                    }
                }


                // -------------------------- 未来観測β --------------------------
            }
            catch (Exception)
            {

            }
        }

        private void textBox_アレキビームzahyoX_init_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dammyForm.Location = SettingPoint(textBox_arekibimuzahyoX_init, textBox_arekibimuzahyoY_init);
            }
            catch
            { }
        }

        private void textBox_アレキビームzahyoY_init_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dammyForm.Location = SettingPoint(textBox_arekibimuzahyoX_init, textBox_arekibimuzahyoY_init);
            }
            catch
            { }
        }

        private void listBox_アレキビーム倍率_init_SelectedIndexChanged(object sender, EventArgs e)
        {
            BairituアレキビームSetting();
        }
        private void BairituアレキビームSetting()
        {
            listBox_arekibimubairitu_text_init.Text = listBox_arekibimubairitu_init.Text;

            double bairitsu = double.Parse(listBox_arekibimubairitu_text_init.Text.ToString().Replace("倍", ""));

            dammyForm.Size = new Size((int)(dammyFormSize.Width * bairitsu), (int)(dammyFormSize.Height * bairitsu));
            dammyForm.pictureBox1.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox2.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox3.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox4.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox5.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox6.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox7.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox8.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox9.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox10.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox11.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
            dammyForm.pictureBox12.Size = new Size((int)(dammyFormpictureBoxSize.Width * bairitsu), (int)(dammyFormpictureBoxSize.Height * bairitsu));
        }


        private void button1_アレキビーム表示位置確認_Click(object sender, EventArgs e)
        {
            if (textBox_アレキビーム位置確認.Text == "位置確認")
            {
                int X = textBox_arekibimuzahyoX_init.Text == "" ? 100 : int.Parse(textBox_arekibimuzahyoX_init.Text);
                int Y = textBox_arekibimuzahyoY_init.Text == "" ? 100 : int.Parse(textBox_arekibimuzahyoY_init.Text);
                Point point = new Point(X, Y);
                // 位置を指定してしまう
                dammyForm.Location = point;
                dammyForm.pictureBox1.Visible = true;
                dammyForm.Show();

                textBox_アレキビーム位置確認.Text = "確認終了";
                button1_アレキビーム表示位置確認.Text = "確認終了";
            }
            else
            {
                dammyForm.Hide();
                dammyForm.pictureBox1.Visible = false;
                textBox_アレキビーム位置確認.Text = "位置確認";
                button1_アレキビーム表示位置確認.Text = "位置確認";
            }
        }


        private void textBox_未来観測zahyoX_init_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dammyForm.Location = SettingPoint(textBox_miraikansokuzahyoX_init, textBox_miraikansokuzahyoY_init);
            }
            catch
            { }
        }

        private void textBox_未来観測zahyoY_init_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dammyForm.Location = SettingPoint(textBox_miraikansokuzahyoX_init, textBox_miraikansokuzahyoY_init);
            }
            catch
            { }
        }

        private void button1_未来観測表示位置確認_Click(object sender, EventArgs e)
        {
            if (textBox_未来観測位置確認.Text == "位置確認")
            {
                int X = textBox_miraikansokuzahyoX_init.Text == "" ? 100 : int.Parse(textBox_miraikansokuzahyoX_init.Text);
                int Y = textBox_miraikansokuzahyoY_init.Text == "" ? 100 : int.Parse(textBox_miraikansokuzahyoY_init.Text);
                Point point = new Point(X, Y);
                // 位置を指定してしまう
                dammyForm2.Location = point;
                dammyForm2.pictureBox1.Visible = true;
                dammyForm2.pictureBox5.Visible = true;
                dammyForm2.Show();

                textBox_未来観測位置確認.Text = "確認終了";
                button1_未来観測表示位置確認.Text = "確認終了";
            }
            else
            {
                dammyForm2.Hide();
                dammyForm2.pictureBox1.Visible = false;
                dammyForm2.pictureBox5.Visible = false;
                textBox_未来観測位置確認.Text = "位置確認";
                button1_未来観測表示位置確認.Text = "位置確認";
            }
        }

        private void listBox_未来観測倍率_init_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bairitu未来観測Setting();
        }

        private void Bairitu未来観測Setting()
        {
            listBox_miraikansokubairitu_text_init.Text = listBox_miraikansokubairitu_init.Text;

            double bairitsu = double.Parse(listBox_miraikansokubairitu_text_init.Text.ToString().Replace("倍", ""));

            dammyForm2.Size = new Size((int)(dammyForm2Size.Width * bairitsu), (int)(dammyForm2Size.Height * bairitsu));
            dammyForm2.pictureBox1.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox2.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox3.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox4.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox5.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox6.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox7.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox8.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox9.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox10.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox11.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
            dammyForm2.pictureBox12.Size = new Size((int)(dammyForm2pictureBoxSize.Width * bairitsu), (int)(dammyForm2pictureBoxSize.Height * bairitsu));
        }

        private Point SettingPoint(TextBox textBoxX, TextBox textBoxY)
        {
            int X = textBoxX.Text == "" ? 100 : int.Parse(textBoxX.Text);
            int Y = textBoxY.Text == "" ? 100 : int.Parse(textBoxY.Text);
            return new Point(X, Y);
        }
    }
}
