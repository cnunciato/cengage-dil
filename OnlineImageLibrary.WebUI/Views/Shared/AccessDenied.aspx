<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Access Denied</title>
    <link rel="Stylesheet" href="/Content/App.css" />
</head>
<body>
    <div id="container">
        <div id="header"></div>
        <div id="content">
            <div id="access-denied">
                <h1>Invalid Request</h1>
                <p>
                    Your request could not be processed, either because of an expired session or missing entitlement.
                    Please sign out and back in again, and if the problem persists, contact an administrator.
                </p>
            </div>
        </div>
    <div>
</body>
</html>
