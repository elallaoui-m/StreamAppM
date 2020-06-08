"use strict";


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.hub.start().done(function () {
    console.log("id : %o", $.connection.hub.id);
});

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    console.log(li);
});

connection.start().then(function () {
    console.log('Now connected, connection ID=' + connection.id);
}).catch(function (err) {
    return console.error(err.toString());
});



