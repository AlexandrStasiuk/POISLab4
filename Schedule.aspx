<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="POISLab4.Schedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Расписание кинотеатра</h2>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true"></asp:GridView>
        <asp:Button ID="btnSaveXml" runat="server" Text="Сохранить в XML" OnClick="btnSaveXml_Click" /><br />
        <asp:Button ID="btnClear" runat="server" Text="Очистить базу" OnClick="btnClear_Click" />
        <asp:Button ID="btnLoadXml" runat="server" Text="Загрузить из XML" OnClick="btnLoadXml_Click" />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
    </form>
    <a href="AddSession.aspx">Добавить в расписание</a>
    <a href="AddFilm.aspx">Добавить фильм</a>
</body>
</html>
