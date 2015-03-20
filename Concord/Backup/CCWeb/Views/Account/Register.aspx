<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register
</asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Create a New Account</h2>
    <p style="padding:5px 5px 5px 5px;">
        Use the form below to create a new account. 
    </p>    
    <%= Html.ValidationSummary("Account creation was unsuccessful. Please correct the errors and try again.", new { style = "color:Red;" })%>

    <% using (Html.BeginForm()) { %>
        <div>
            <table style="padding:5px 5px 5px 5px;">
                <tr><td><label>Admin Username:</label></td><td>
                    <%= Html.TextBox("adminusername") %>
                    <%= Html.ValidationMessage("adminusername", new { style = "color:Red;" })%></td></tr>
                <tr><td><label>Admin Pasword:</label></td><td>
                    <%= Html.Password("adminpassword") %>
                    <%= Html.ValidationMessage("adminpassword", new { style = "color:Red;" })%></td></tr>
            </table>
        </div>
        <br />
        <div>
            <table style="padding:5px 5px 5px 5px;">
                <tr><td><label for="username">Username:</label></td><td>
                    <%= Html.TextBox("username") %>
                    <%= Html.ValidationMessage("username", new { style = "color:Red;" })%></td></tr>
                <tr><td><label for="email">Email:</label></td><td>
                    <%= Html.TextBox("email") %>
                    <%= Html.ValidationMessage("email", new { style = "color:Red;" })%></td></tr>
                <tr><td><label for="password">Password:</label></td><td>
                    <%= Html.Password("password") %>
                    <%= Html.ValidationMessage("password", new { style = "color:Red;" })%></td></tr>
                <tr><td><label for="confirmPassword">Confirm password:</label></td><td>
                    <%= Html.Password("confirmPassword") %>
                    <%= Html.ValidationMessage("confirmPassword", new { style = "color:Red;" })%></td></tr>
            </table>
                <p style="padding:5px 5px 5px 5px;">
                    <input type="submit" value="Register" />
                </p>
        </div>
    <% } %>
</asp:Content>
