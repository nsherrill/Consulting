<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
        Inherits="System.Web.Mvc.ViewPage<Common.Users>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit Project
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

        <%using (Html.BeginForm())
          { %>
    <h2>Edit User: <%=Model.Id==-1?"[new]":Model.UserName %></h2>
        <%=Html.TextBox("id", Model.Id, new {style = "display:none;"}) %>
        <table style="padding:15px 5px 5px 25px;">
            <tr><td style="padding:5px 5px 5px 5px;">Username:</td>
                <td style="padding:5px 5px 5px 5px;">
                    <%=Html.TextBox("username", Model.UserName, new {style="width:100px;"}) %></td></tr>
            <tr><td style="padding:5px 5px 5px 5px;">Password:</td>
                <td style="padding:5px 5px 5px 5px;">
                    <%=Html.TextBox("password", Model.Password, new {style="width:100px;"}) %></td></tr>
            <tr><td style="padding:5px 5px 5px 5px;">Email:</td>
                <td style="padding:5px 5px 5px 5px;" colspan="3">
                    <%=Html.TextBox("email", Model.Email, new {style="width:250px;"}) %></td></tr>
            <tr><td style="padding:5px 5px 5px 5px;">UserGroup:</td>
                <td style="padding:5px 5px 5px 5px;">
                    <%=Html.DropDownList("usergroup", Common.Users.GetUserGroupsList((int)Model.UserGroup)) %></td>
                <td>Update By Email:</td><td><%=Html.CheckBox("updatebyemail", Model.UpdateByEmail) %></td></tr>
        </table>
        <br />
        <input type="submit" value="Save Changes" />
        <%} %>
</asp:Content>
