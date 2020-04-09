"use strict"

$(document).ready(() => {
    UpdateScroll();

    // Setup event listeners
    $("#message").autogrow({ vertical: true, horizontal: false });
    $("#message").on("keyup", (e) => {
        if (e.keyCode == 13) {
            SendMessage();
        }
        e.preventDefault();
    })

    $("#message-form").on("submit", (e) => {
        SendMessage();
        e.stopImmediatePropagation()
        e.preventDefault();
    })

    //connection.on("ReceiveMessage", (message, id) => {
    //    ReceiveMessage(message);
    //    console.log(id);
    //    
    //});
})

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
        UpdateScroll();
    }).catch((err) => {
        return console.error(err.toString());
    });

}

function AddMessage(message) {
    console.log("Adding Message: " + message);

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

    UpdateScroll();
    console.log("Adding Message: " + message);
}

function UpdateScroll() {
    let messageScroll = document.getElementById("message-scroll");
    messageScroll.scrollTop = messageScroll.scrollHeight;
}