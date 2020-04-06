"use strict"

$(document).ready(() => {
    var connectionEstablished = false;

    // Setup SignalR connection
    var connection = new signalR.HubConnectionBuilder().withUrl("/messaging-hub").build();
    connection.start().then(() => {
        connectionEstablished = true;
    }).catch((err) => {
        return console.error(err.toString());
    });

    // Setup event listeners
    $("#message-form").on("submit", (e) => {
        SendMessage();
        e.preventDefault();
        return false;
    })

    connection.on("ReceiveMessage", (user, message) => {
        ReceiveMessage(user, message);
    })

    function SendMessage() {
        if (!connectionEstablished) return;

        var message = $("#message").val();
        if (message == "") return;

        // TODO change this to server side
        var user = $("#user").val();

        connection.invoke("SendMessage", user, message)
            .catch((err) => {
                return console.error(err.toString());
            });
    }

    function ReceiveMessage(user, message) {
        // Clean the message of dangerous strings
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = user + ": " + msg;

        // Create the message element
        var li = document.createElement("li");
        li.setAttribute("class", "message-right mb-3");
        var span = document.createElement("span");
        span.setAttribute("class", "shadow-sm");
        span.textContent = encodedMsg;
        li.appendChild(span);

        document.getElementById("messages-list").appendChild(li);
    }
})