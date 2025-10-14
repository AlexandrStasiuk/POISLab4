<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddFilm.aspx.cs" Inherits="POISLab4.AddFilm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Добавить фильм</h2>
        <asp:TextBox ID="txtTitle" runat="server" placeholder="Название фильма" /><br />
        <asp:TextBox ID="txtGenre" runat="server" placeholder="Жанр" /><br />
        <asp:TextBox ID="txtDuration" runat="server" placeholder="Длительность (мин)" /><br />
        <asp:Button ID="btnAdd" runat="server" Text="Добавить" OnClick="btnAdd_Click" /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
    </form>
</body>
</html>
