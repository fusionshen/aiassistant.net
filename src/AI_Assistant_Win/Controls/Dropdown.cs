// COPYRIGHT (C) Tom. ALL RIGHTS RESERVED.
// THE AntdUI PROJECT IS AN WINFORM LIBRARY LICENSED UNDER THE Apache-2.0 License.
// LICENSED UNDER THE Apache License, VERSION 2.0 (THE "License")
// YOU MAY NOT USE THIS FILE EXCEPT IN COMPLIANCE WITH THE License.
// YOU MAY OBTAIN A COPY OF THE LICENSE AT
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING, SOFTWARE
// DISTRIBUTED UNDER THE LICENSE IS DISTRIBUTED ON AN "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED.
// SEE THE LICENSE FOR THE SPECIFIC LANGUAGE GOVERNING PERMISSIONS AND
// LIMITATIONS UNDER THE License.
// GITEE: https://gitee.com/antdui/AntdUI
// GITHUB: https://github.com/AntdUI/AntdUI
// CSDN: https://blog.csdn.net/v_132
// QQ: 17379620

using System.Collections.Generic;
using System.Windows.Forms;

namespace AI_Assistant_Win.Controls
{
    public partial class Dropdown : UserControl
    {
        Form form;
        public Dropdown(Form _form)
        {
            form = _form;
            InitializeComponent();
            button17.Items.Add(new AntdUI.SelectItem(Properties.Resources.bg1, "����� Hannibal"));
            dropdown1.Items.AddRange(new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("one st menu item"),
                new AntdUI.SelectItem("two nd menu item"),
                new AntdUI.SelectItem("three rd menu item"){
                    Sub = new List<object>{
                        new AntdUI.SelectItem("�Ӳ˵�1"){
                            Sub=new List<object>{ new AntdUI.SelectItem("sub menu") {
                                Sub=new List<object>{
                                    "one st menu item","two nd menu item","three rd menu item"
                                }
                            } }
                        },
                        new AntdUI.SelectItem( "�Ӳ˵�2")
                    }
                },
                new AntdUI.SelectItem("four menu item"){ Sub=new List<object>{ "five menu item", "six six six menu item"} },
            });
            dropdown1.Items.AddRange(new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("one st menu item"),
                new AntdUI.SelectItem("two nd menu item"),
                new AntdUI.SelectItem("three rd menu item"){
                    Sub = new List<object>{
                        new AntdUI.SelectItem("�Ӳ˵�1"){
                            Sub=new List<object>{ new AntdUI.SelectItem("sub menu") {
                                Sub=new List<object>{
                                    "one st menu item","two nd menu item","three rd menu item"
                                }
                            } }
                        },
                        new AntdUI.SelectItem( "�Ӳ˵�2")
                    }
                },
                new AntdUI.SelectItem("four menu item"){ Sub=new List<object>{ "five menu item", "six six six menu item"} },
            });
        }

        private void dropdown1_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            AntdUI.Message.info(form, "��ѡ�У�" + e.Value, Font);
        }
    }
}