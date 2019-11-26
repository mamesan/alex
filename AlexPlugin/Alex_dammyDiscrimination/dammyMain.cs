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
        private string MyJobName = "";
        private string LogMyJobName = "";


        public dammyMain()
        {
            InitializeComponent();
            ActPluginHelper.ActHelper.Initialize();
        }

        public void DeInitPlugin()
        {
            ActPluginHelper.ACTInitSetting.SaveSettings(xmlSettings, PluginName);
        }

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        { //lbStatus = pluginStatusText;   // Hand the status label's reference to our local var
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

            }
        }
    }
}
