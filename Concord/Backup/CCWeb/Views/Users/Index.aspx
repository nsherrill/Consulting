<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
        Inherits="System.Web.Mvc.ViewPage<List<Common.Users>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Users</h2>

    <br />
    <%=Html.ActionLink("Create New User", "Edit", new { id = -1 })%>
    <br /><br />
      <table style="border-color:#69523b; border-width:2px; border-style:solid;">
        <tr style="font-weight:bold;">
            <td valign="middle" align="center" style="padding:5px 5px 5px 5px;display:none;">ID</td>
            <td valign="middle" align="center" style="padding:5px 5px 5px 5px;">Username</td>
            <td valign="middle" align="center" style="padding:5px 5px 5px 5px;">Email</td>
            <td valign="middle" align="center" style="padding:5px 5px 5px 5px;">UserGroup</td>
            <td valign="middle" align="center" style="padding:5px 5px 5px 5px;">Update<br />By Email</td></tr>
            <%if (Model != null && Model.Count > 0)
              {%>
              <%
                foreach (Common.Users user in Model)
                { %>
                    <tr><td valign="top" style="padding:5px 5px 5px 5px;display:none;"><%=user.Id%></td>
                        <td valign="top" style="padding:5px 5px 5px 5px;">
                            <%=Html.ActionLink(user.UserName, "Edit", new { id = user.Id })%></td>
                        <td valign="top" style="padding:5px 5px 5px 5px;"><%=user.Email%></td>
                        <td valign="top" style="padding:5px 5px 5px 5px;"><%=user.UserGroup.ToString()%></td>
                        <td style="padding:5px 5px 5px 5px;" align="center">
                            <%=Html.CheckBox("updatebyemail", user.UpdateByEmail, new { disabled="disabled" })%></td>
                        <td valign="top" style="padding:5px 5px 5px 5px;">
                            <%=Html.ActionLink("Remove", "Remove", new { id = user.Id })%></td>
                        </tr>
                <%} %>
            <%} %>
        </table>
</asp:Content>
