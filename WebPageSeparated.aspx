<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WebPageSeparated.aspx.cs" Inherits="WebPageSeparated" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link rel="stylesheet" type="text/css" href="Content/StyleSheet.css"/>

    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Conteiner" class="conteiner">
        <div id="Header" class="header">
            <p><h2>Добро пожаловать в Справочник МОЭ</h2></p><br />
            <p><b>Название статьи:</b> <asp:TextBox ID="TextBox1" runat="server" Height="20px" Width="219px" BorderStyle="Dotted" Font-Size="Large"></asp:TextBox>
        <asp:Button ID="Button2" runat="server" Text="Редактировать" OnClick="Button2_Click" CssClass="button2" />
                <br /></p>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Читать" CssClass="button1" />
        <asp:Button ID="Button3" runat="server" Text="Создать новую" OnClick="Button3_Click" CssClass="button3" />
            </div>
        
        
        <br />
        <br />
        <br />
        <div id="content">
            <asp:Label ID="Label1" runat="server" Text="Статья" CssClass="lable" Font-Size="X-Large"></asp:Label><br />
        <br />
            <asp:TextBox ID="TextBox2" runat="server" style="margin-top: 0px; top: 274px; left: 137px; height: 301px; right: 529px; width: 323px;" ReadOnly="True" TextMode="MultiLine" CssClass="text" Font-Size="X-Large"></asp:TextBox>
        </div>
        
        
        
        
       
    
        <asp:Button ID="Button4" runat="server" Height="43px" OnClick="Button4_Click" Text="Сохранить" Visible="False" CssClass="button4" Width="135px" />
        <asp:Button ID="Button5" runat="server" Height="43px" OnClick="Button5_Click" style="margin-left: 330px; top: 296px; left: 464px;" Text="Завершить" Visible="False" Width="135px" CssClass="button5" />
    
    </div>
    </form>
</body>
</html>
