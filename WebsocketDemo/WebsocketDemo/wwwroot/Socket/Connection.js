{
//-----------------------------------------------------------------------------------------------------------------------------------
    /* page maps */
    function PageMaps(destination) {
        var frameSrc = document.getElementById("iframecontent").src;
        switch (destination) {
            case 0:
                frameSrc = "/users/getuserlist";
                break;
        }
    }
//-----------------------------------------------------------------------------------------------------------------------------------
    /* connection to websocket server on first time */
    var PasswordCache = "";
    var logName;
    var webSocket;

    //The address of our HTTP-handler
    var handlerUrl = "ws://" + window.location.host + "/ws";

    //Initialize function,open the websocket connection,calls on page loading 
    function initWebSocket() {

        //Initialize websocket if its undefined
        if (webSocket == undefined) {

            try {
                webSocket = new WebSocket(handlerUrl);
            }
            catch (err) {
                console.log(err.message);
                return;
            }

            //Open connection  handler.
            webSocket.onopen = function () {
                console.log("WebSocket opened successfully!");
                console.log(webSocket.readyState);
            };

            //Message data handler.
            webSocket.onmessage = function (e) {
                console.log(e.data);

                OpreationMap(e.data);
            };

            //Close event handler.
            webSocket.onclose = function (e) {
                console.log("connect closed");
            };

            //Error event handler.
            webSocket.onerror = function (e) {
                console.log("error code:" + e.data + ",trying to reconnect.")
                Reconnect();
            }
        }
    }

    function Reconnect() {
        initWebSocket();
    }

    function CloseWebSocket() {
        webSocket.close();
    }
//----------------------------------------------------------------------------------------------------------------------------------
    /* sending message on login */
    function UserLoginSend() {
        webSocket = window.parent.webSocket;
        
        logName = document.getElementById("LogName").value;
        var messageSending = "login$$$" + logName + "!" + PasswordCache;
        
        BeginSend(messageSending);
    }

    /* begin message sending */
    function BeginSend(msg) {
        //Initialize WebSocket.

        //Send data if WebSocket is opened.
        if (webSocket.OPEN && webSocket.readyState === 1)
            webSocket.send(msg);

        //If WebSocket is closed, show message.
        if (webSocket.readyState === 2 || webSocket.readyState === 3)
            console.log("WebSocket closed, the data can't be sent.");
    }

//----------------------------------------------------------------------------------------------------------------------------------

    /* get result from websocket server */
    function OpreationMap(data) {
        //is login
        switch (data.split("***")[0]) {
            case "login":
                isLogin(data.split("***")[1]);
                break
            default:
                break;
        }
    }

//----------------------------------------------------------------------------------------------------------------------------------

    /* page logic */
    function isLogin(data) {
        if (data === "lognamenotexsit") {
            alert("用户名不存在!");
        } else if (data === "authenticationfailed") {
            alert("密码验证失败!");
        } else {
            //window.location.href = "/users/index?logName=" + data;
            window.parent.document.getElementById("iframelogin").style.display = "none";
            window.parent.document.getElementById("iframecontent").src = "/users/index?logName=" + data;
            window.parent.document.getElementById("iframecontent").style.display = "block";
            window.parent.document.getElementById("layoutnav").style.display = "block";
        }
    }
}