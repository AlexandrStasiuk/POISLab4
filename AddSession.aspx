<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddSession.aspx.cs" Inherits="POISLab4.AddSession" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Добавить сеанс</h2>
        <asp:DropDownList ID="ddlFilms" runat="server"></asp:DropDownList><br />
        <asp:TextBox ID="txtStart" runat="server" placeholder="2025-10-14 18:30:00" /><br />
        <asp:TextBox ID="txtHall" runat="server" placeholder="Зал №1" /><br />
        <asp:Button ID="btnAddSession" runat="server" Text="Добавить сеанс" OnClick="btnAddSession_Click" /><br />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
    </form>
</body>
</html>
