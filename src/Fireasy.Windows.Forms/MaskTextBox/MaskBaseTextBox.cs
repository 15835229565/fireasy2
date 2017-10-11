using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Fireasy.Windows.Forms
{
    [ToolboxItem(false),
    DesignTimeVisible(false),
    DefaultProperty("Text")]
    public abstract class MaskBaseTextBox : TextBox
    {
        protected Behavior m_behavior = null;

        protected MaskBaseTextBox()
        {
        }

        internal MaskBaseTextBox(Behavior behavior)
        {
            m_behavior = behavior;
        }

        public bool UpdateText()
        {
            return m_behavior.UpdateText();
        }

        [Category("Behavior")]
        [Description("")]
        public int Flags
        {
            get { return m_behavior.Flags; }
            set { m_behavior.Flags = value; }
        }

        /// <summary>
        /// �޸ı�־��
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="addOrRemove"></param>
        public void ModifyFlags(int flags, bool addOrRemove)
        {
            m_behavior.ModifyFlags(flags, addOrRemove);
        }

        /// <summary>
        /// ��֤���ݡ�
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            return m_behavior.Validate();
        }

        /// <summary>
        /// ��ȡ�Ƿ���Ч��
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return m_behavior.IsValid();
        }

        /// <summary>
        /// ��������Ի���
        /// </summary>
        /// <param name="message"></param>
        public void ShowErrorMessageBox(string message)
        {
            m_behavior.ShowErrorMessageBox(message);
        }

        /// <summary>
        /// ��ʾ����ͼ�ꡣ
        /// </summary>
        /// <param name="message"></param>
        public void ShowErrorIcon(string message)
        {
            m_behavior.ShowErrorIcon(message);
        }

        [Browsable(false)]
        public string ErrorMessage
        {
            get { return m_behavior.ErrorMessage; }
        }

        internal class Designer : ControlDesigner
        {
        }
    }
}
