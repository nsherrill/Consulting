<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log On
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Log On</h2>
    <p style="padding:5px 5px 5px 5px;">
        Please enter your username and password.<!-- <%= Html.ActionLink("Register", "Register") %> if you don't have an account.-->
    </p>
    <%= Html.ValidationSummary("Login was unsuccessful. Please correct the errors and try again.", new { style = "color:Red;" })%>

    <% using (Html.BeginForm()) { %>
        <div>
            <table style="padding:5px 5px 5px 5px;">
                <tr><td><label for="username">Username:</label></td>
                    <td><%= Html.TextBox("username") %>
                        <%= Html.ValidationMessage("username", new { style = "color:Red;" })%></td></tr>
                <tr><td><label for="password">Password:</label></td>
                    <td><%= Html.Password("password") %>
                        <%= Html.ValidationMessage("password", new { style = "color:Red;" })%></td></tr>
            </table>
                <p style="padding:5px 5px 5px 5px;">
                    <input type="submit" value="Log On" />
                </p>
        </div>
    <% } %>
</asp:Content>
