<%@ Page language="c#" Codebehind="default.aspx.cs" AutoEventWireup="True" Inherits="HatCMS.setup.setupPage" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Setup HatCMS</title>
		<link rel="stylesheet" type="text/css" href="../css/_system/Setup.css" />
	</head>
	<body>
		<div id="navigation"><div class="wrapper"><img src="../images/_system/hatCms_logo.png" style="float: left; margin-top: 5px;" /> <h1>HatCMS Setup</h1></div></div>
	    <div class="wrapper">
		<form id="Form1" method="post" runat="server">
			
			<p><asp:Label id="l_msg" runat="server"></asp:Label></p>
			
				<h2>Step 1: Edit the web.config file</h2>
            <p>
                Ensure that configuration items are configured properly. These especially include
                the following keys:</p>
            <ul>
                <li>TemplateEngineVersion</li>
                <li>SiteName</li>
                <li>languages</li>
                <li>LinkMacrosIncludeLanguage</li>
            </ul>
            <p>
                Ensure also that the Author and Administrator roles are configured properly.</p>
            <p>
            </p>
            <h2>
                Step 2: Initialize the Database</h2>
				<p>Fill in the following information to create the new database:<br />
				<strong>Note: the database should *not* be exist before this step is run (the script will create the database)</strong>
				</p>
				<table>
			        <tr>
			            <td>Database server host name: <span class="required">[required]</span></td>
				        <td><asp:TextBox id="db_host" runat="server" Columns="40">localhost</asp:TextBox></td>
				    </tr>
				    <tr>
				        <td>New Database Name: <span class="required">[required]</span></td>
				        <td><asp:TextBox id="tb_DbName" runat="server" Columns="40">hatCMS3</asp:TextBox>(the script will create this database and must not already exist)</td>
				    </tr>
				    <tr>
				        <td>Database access username: <span class="required">[required]</span></td>
				        <td><asp:TextBox id="db_un" runat="server" Columns="40">hatCMS</asp:TextBox>(used just for this installation step)</td>
				    </tr>
				    <tr>
				        <td>Database access password:</td>
				        <td><asp:TextBox id="db_pw" runat="server" Columns="40">hatCMS</asp:TextBox>(used just for this installation step)</td>
				    </tr>
				</table>
			
                
                    
                    
			
			<p>
				<asp:Button id="b_db" runat="server" Text="create &amp; setup database" onclick="b_db_Click"></asp:Button></p>
			<h2>Step 3: Update the ConnectionString</h2>
            <p>
                &nbsp;<asp:Label ID="l_NewConnStr" runat="server"></asp:Label>
                <strong><u></u></strong>
            </p>
			<h2>
                Step 4: Create Standard Pages</h2>
            <p>
                Create standard pages - including the home page and other pages needed for the CMS
                to function.</p>
            <h2>
                <asp:Button ID="b_CreatePages" runat="server" OnClick="b_CreatePages_Click" Text="Create Standard Pages" />&nbsp;</h2>
            <h2>
                Step 5: Verify the configuration</h2>
            <p>
                Some directories need to be made writable by the webserver.</p>
            <p>
				<asp:Button id="b_verifyConfig" runat="server" Text="verify configuration" onclick="b_verifyConfig_Click"></asp:Button> </p>
				<h2>
                    Step 6: Delete the setup directory</h2>
            <p>
                After the configuration has been verified, please delete the "setup" directory.
                You can always verify your configuration through the "Admin Tools"</p>
            <h2>
                Step 7: Customize Templates and Controls</h2>
            <p>
                Your site is now installed, but is quite bland. You can customize your site by creating
                custom templates and controls.</p>
		</form>
		</div>
	</body>
</html>
