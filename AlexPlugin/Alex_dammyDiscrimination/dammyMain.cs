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
        private List<int> dammyList2 = new List<int>();
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
        private Size dammyFormSize = new Size();
        private Size dammyFormpictureBoxSize = new Size();

        public dammyMain()
        {
            InitializeComponent();
            ActPluginHelper.ActHelper.Initialize();
        }

        public void DeInitPlugin()
        {
            ActPluginHelper.ACTInitSetting.SaveSettings(xmlSettings, PluginName);
            dammyForm.Hide();
            dammyForm.Close();

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


            dammyAlex = new Dictionary<int, string>();
            dammyAlex.Add(1, "489F:Unknown_489F.*:1000:78.* 91");
            dammyAlex.Add(2, "489F:Unknown_489F.*:1000:91.* 78");
            dammyAlex.Add(3, "489F:Unknown_489F.*:1000:108.* 78");
            dammyAlex.Add(4, "489F:Unknown_489F.*:1000:121.* 91");

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
            
            dammyList2 = new List<int>();
            dammyList2.Add(1);
            dammyList2.Add(2);
            dammyList2.Add(3);
            dammyList2.Add(4);

            // ログを取得するイベントを生成する
            ActGlobals.oFormActMain.OnLogLineRead += OFormActMain_OnLogLineRead;
            // 戦闘開始をチェックする
            ActGlobals.oFormActMain.OnCombatStart += OFormActMain_OnCombatStart;
            // 戦闘終了をキャッチする
            ActGlobals.oFormActMain.OnCombatEnd += OFormActMain_OnCombatEnd;



            dammyFormSize = new Size(150, 150);
            dammyFormpictureBoxSize = new Size(150, 150);

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
                // -------------------------- 戦闘開始時の処理 --------------------------
                // 戦闘開始のお知らせ
                if (combatFlg && !initButtoleFlg)
                {
                    // 戦闘前の初期処理
                    initButtoleFlg = true;
                    MyJobName = ActPluginHelper.ActHelper.MyJob();
                    
                    // 未来観測α用
                    dammyList = new List<int>();
                    dammyList.Add(1);
                    dammyList.Add(2);
                    dammyList.Add(3);
                    dammyList.Add(4);
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
                        if (logInfo.logLine.Contains("4B0E:Unknown_4B0e"))
                        {
                            行動命令flg = true;
                        }
                            
                            
                        
                        // 集団罰
                        if (logInfo.logLine.Contains("48A6:Unknown_48A6"))
                        {
                            Regex regex = new Regex(@"^.*15:([A-Z0-9]{8})::48A5:Unknown_48A5.*");
                            集団罰List.Add(regex.Replace(logInfo.logLine, "$1"));
                        }
                        // 加重罰
                        else if (logInfo.logLine.Contains("48A5:Unknown_48A5"))
                        {
                            Regex regex = new Regex(@"^.*15:([A-Z0-9]{8})::48A5:Unknown_48A5.*");
                            加重罰List.Add(regex.Replace(logInfo.logLine, "$1"));
                        }
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
                    }

                    // 集団罰・加重罰が付いた場合は、即処理を中断する
                    if (集団罰List.Contains(LogMyJobName) || 加重罰List.Contains(LogMyJobName))
                    {
                        αStopFlg = true;
                    }

                    // StopFlgが立っている場合のみ、後続処理を実施する
                    if (αStopFlg)
                    {
                        // 自分が集団罰の場合の処理
                        if (集団罰List.Contains(LogMyJobName))
                        {
                            if (停止命令flg)
                            {
                            }
                            else if (行動命令flg)
                            {
                            }
                        }
                        // 自分が加重罰の場合の処理
                        else if (加重罰List.Contains(LogMyJobName))
                        {
                            if (停止命令flg)
                            {
                            }
                            else if (行動命令flg)
                            {
                            }
                        }
                        // 自分が名誉罰の場合の処理
                        else if(名誉罰List.Contains(LogMyJobName))
                        {
                            if (停止命令flg)
                            {
                            }
                            else if (行動命令flg)
                            {
                            }
                        }
                        // どれにも入っていなかったら、無職
                        else
                        {
                            if (停止命令flg)
                            {
                            }
                            else if (行動命令flg)
                            {
                            }
                        }
                    }

                    // ダミーアレキ判定用
                    if (logInfo.logLine.Contains("489F:Unknown_489F"))
                    {
                        foreach (KeyValuePair<int,string> pair in dammyAlex)
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
                        ActGlobals.oFormActMain.TTS(TTSstr);
                        dammyList = new List<int>();
                        dammyList.Add(1);
                        dammyList.Add(2);
                        dammyList.Add(3);
                        dammyList.Add(4);
                    }

                    // 未来観測αを終了する処理
                    if (logInfo.logLine.Contains("十字の秘蹟"))
                    {
                        αStopFlg = false;
                        未来確定αflg = false; 
                        停止命令flg = false;
                        行動命令flg = false;
                    }
                }

                // -------------------------- 未来観測α --------------------------
                
                // -------------------------- 未来観測β --------------------------
                if (logInfo.logLine.Contains("は「未来観測β」の構え。"))
                {
                    未来確定βflg = true;
                }
                if (未来確定βflg)
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
                if (未来確定βflg)
                {
                    if (logInfo.logLine.Contains("489E:Unknown_489E"))
                    {
                        foreach (KeyValuePair<int,string> pair in dammyAlex2)
                        {
                            Regex regex = new Regex(@pair.Value);
                            if (regex.IsMatch(logInfo.logLine))
                            {
                                // マッチした場所の番号を除去する
                                dammyList2.Remove(pair.Key);
                            }
                        }
                    }

                    // リストが残り一つになった場合に処理を実施する
                    if (dammyList2.Count == 1)
                    {
                        string TTSstr = "";
                        dammyForm.Show();
                        switch (dammyList2[0])
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
                        ActGlobals.oFormActMain.TTS(TTSstr);
                        dammyList2 = new List<int>();
                        dammyList2.Add(1);
                        dammyList2.Add(2);
                        dammyList2.Add(3);
                        dammyList2.Add(4);
                    }
                }

                // -------------------------- 未来観測β --------------------------
            }
            catch (Exception)
            {

            }
        }
    }
}
