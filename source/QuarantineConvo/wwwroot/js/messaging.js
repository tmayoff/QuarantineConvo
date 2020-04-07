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
    });

    UpdateScroll();

    // Functions
    function SendMessage() {
        if (!connectionEstablished) return;

        var connectionID = $("#connection-id").val();
        if (connectionID == "") return;
        var message = $("#message").val();
        if (message == "") return;

        // Send the message
        connection.invoke("SendMessage", connectionID, message).then(() => {
            // Add to view
            var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
            var encodedMsg = msg;

            // Create the message element
            let li = document.createElement("li");
            li.setAttribute("class", "message-right mb-3");
            let span = document.createElement("span");
            span.setAttribute("class", "shadow-sm");
            span.textContent = encodedMsg;
            li.appendChild(span);

            document.getElementById("messages-list").appendChild(li);

            $("#message").val("");
        })
        .catch((err) => {
            return console.error(err.toString());
        });
    }

    function ReceiveMessage(message) {

        // Clean the message of dangerous strings
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = msg;

        // Create the message element
        let li = document.createElement("li");
        li.setAttribute("class", "message-left mb-3");
        let span = document.createElement("span");
        span.setAttribute("class", "shadow-sm");
        span.textContent = encodedMsg;
        li.appendChild(span);

        document.getElementById("messages-list").appendChild(li);
    }

    function UpdateScroll() {
        console.log("Scrolling")
        let messageScroll = document.getElementById("message-scroll");
        messageScroll.scrollTop = messageScroll.scrollHeight;
    }
})