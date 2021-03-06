﻿using System;
using System.Windows.Forms;
using Install4ibas.Tools.Services.Basis;

namespace Install4ibas.UI
{
    public partial class ShellControl : UserControl
    {
        public ShellControl()
        {
            InitializeComponent();
        }
        private void splitContainer_SizeChanged(object sender, EventArgs e)
        {
            this.splitContainer.SplitterDistance = this.splitContainer.Height * 11 / 13;
        }
        #region 公用属性及方法
        public void setButtonsVisible(ButtonsVisibleStyle style)
        {
            this.btn_Cancel.Visible = style.HasFlag(ButtonsVisibleStyle.Cancel);
            this.btn_Back.Visible = style.HasFlag(ButtonsVisibleStyle.Back);
            this.btn_Next.Visible = style.HasFlag(ButtonsVisibleStyle.NextEnable) | style.HasFlag(ButtonsVisibleStyle.NextDisable);
            this.btn_Next.Enabled = !style.HasFlag(ButtonsVisibleStyle.NextDisable);
            this.btn_Finish.Visible = style.HasFlag(ButtonsVisibleStyle.Finish);
        }
        public IInstallService installService;
        #region 壳当前加载页面
        protected ChildControl CurrentControl;
        public bool SetCurrentControl(ChildControl control)
        {
            try
            {
                if (control == null) return false;
                if (this.splitContainer.Panel1.Controls.Count > 0)
                {
                    var adddedControl = this.splitContainer.Panel1.Controls[0] as ChildControl;
                    if (adddedControl != null)
                        adddedControl.SaveAppSetting();
                }
                this.splitContainer.Panel1.Controls.Clear();
                this.splitContainer.Panel1.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                control.ShellControl = this;
                control.LoadAppSetting();
                this.CurrentControl = control;
                this.setButtonsVisible(control.ButtonsVisibleStyle);
                control.Initialize();
                return true;
            }
            catch (Exception error)
            {
                return false;
            }

        }
        public bool SetCurrentControl(ControlTypes type)
        {
            ChildControl control = null;
            switch (type)
            {
                case ControlTypes.Welcome:
                    control = new WelcomeControl();
                    break;
                case ControlTypes.LicenseAccept:
                    control = new LicenseAcceptControl();
                    break;
                case ControlTypes.InstallationOptions:
                    control = new InstallationOptionsControl();
                    break;
                case ControlTypes.ModulesChoose:
                    control = new ModulesChooseControl();
                    break;
                case ControlTypes.EditConfig:
                    control = new EditConfigControl();
                    break;
                case ControlTypes.InstallationProgress:
                    control = new InstallationProgressControl();
                    break;
                case ControlTypes.Finish:
                    control = new FinishControl();
                    break;
                case ControlTypes.LicenseCreateControl:
                    control = new LicenseCreateControl();
                    break;
                default:
                    break;
            }
            return SetCurrentControl(control);

        }
        #endregion
        #endregion
        #region UIAction
        private void btn_Next_Click(object sender, EventArgs e)
        {
            var flag = false;//子项控件事件是否执行
            if (this.CurrentControl != null)
                flag = this.CurrentControl.OnNextEvent(sender, e);
            if (flag) return;//子项控件事件成功执行,返回
            //子项控件事件未执行,可执行默认操作
            //以下添加默认操作
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            var flag = false;//子项控件事件是否执行
            if (this.CurrentControl != null)
                flag = this.CurrentControl.OnCancelEvent(sender, e);
            if (flag) return;//子项控件事件成功执行,返回
            //子项控件事件未执行,可执行默认操作
            //以下添加默认操作
            if (MessageBox.Show("是否确认取消安装", "提示",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            var flag = false;//子项控件事件是否执行
            if (this.CurrentControl != null)
                flag = this.CurrentControl.OnBackEvent(sender, e);
            if (flag) return;//子项控件事件成功执行,返回
            //子项控件事件未执行,可执行默认操作
            //以下添加默认操作

        }

        private void btn_Finish_Click(object sender, EventArgs e)
        {
            var flag = false;//子项控件事件是否执行
            if (this.CurrentControl != null)
                flag = this.CurrentControl.OnNextEvent(sender, e);
            if (flag) return;//子项控件事件成功执行,返回
            //子项控件事件未执行,可执行默认操作
            //以下添加默认操作
            Application.Exit();
        }
        #endregion
    }
}
