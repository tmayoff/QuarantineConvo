"use strict"



$(document).ready(() => {

    // Setup event listeners
    $(".sidebar-toggle").on("click", () => {
        ToggleSidebar();
    })

    //$("#message-input").autogrow({ vertical: true, horizontal: false });

    $("#message-input").on("keydown", (e) => {
        if (e.keyCode == 13) e.preventDefault();
    })

    $("#message-input").on("keyup", (e) => {
        if (e.keyCode == 13) {
            SendMessage();
        }
        e.preventDefault();
    })

    $("#message-form").on("submit", (e) => {
        SendMessage();
        e.stopImmediatePropagation()
        e.preventDefault();
    });

    $(".connection").on("click", (e) => {
        ToggleSidebar();

        // Update active connection
        let currentConnection = $(".active-connection")
        if (currentConnection.length != 0) {
            currentConnection.removeClass("active-connection")
        }
        $(e.currentTarget).addClass("active-connection");

        UpdatedMessages();

        $("#message-form").removeClass("d-none")
        $("#message-form").addClass("d-flex")
    })

    UpdatedMessages();

    // Hide form
    if ($(".active-connection").length == 0) {
        $("#message-form").removeClass("d-flex")
        $("#message-form").addClass("d-none")
    }
})

// ---- Functions ----- //

// Toggles the active class on the sidebar
function ToggleSidebar() {
    let sidebar = $(".sidebar")
    sidebar.toggleClass("active");
}

function UpdatedMessages() {
    let connection = $(".active-connection")
    let connectionID = connection.data("id")
    if (connectionID === undefined) return;

    let messagesContainer = $("#messages-container")
    let messageContainer = $(`#messages-container-${connectionID}`)
    if (messageContainer.length == 0) {
        let template = $("#messages-template").clone();

        $.post("/Messaging/GetMessagesContent", { connectionID: connection.data("id"), user: $("#username").text() })
            .done((data, status, jqXHR) => {
                data = JSON.parse(data)

                template.find(".messages-hidden-name").text(data.Username);
                $("#messages-name").text(data.Username);

                template.attr("id", `messages-container-${connectionID}`)
                messagesContainer.append(template);


                let username = $("#username").text();

                data.messages.forEach((msg) => {
                    let messageTemplate = $("#message-template").clone();
                    messageTemplate.attr("id", "")
                    if (username == msg.SentBy) {
                        messageTemplate.addClass("message-right")
                    } else {
                        messageTemplate.addClass("message-left")
                    }
                    messageTemplate.find("span").text(msg.Msg)

                    let scroll = $(`#messages-container-${connectionID}`).find(".messages-scroll")
                    scroll.append(messageTemplate);
                })

                UpdateScroll(connectionID)
                ReadAllMessages(connectionID)

                UpdateMessagesView(connectionID)

            }).fail((jqXHR, status, error) => {
                console.error(error)
            })
    } else {
        // Update name
        $("#messages-name").text(messageContainer.find(".messages-hidden-name").text());

        UpdateScroll(connectionID)
        ReadAllMessages(connectionID)
        UpdateMessagesView(connectionID)
    }
}

function SendMessage() {
    if (!connectionEstablished) return;

    let connection = $(".active-connection")
    let connectionID = connection.data("id");

    var message = $("#message-input").val().trim();
    if (message == "") return;

    // Send the message
    signalRConnection.invoke("SendMessage", connectionID, message).then(() => {
        // Add to view
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = msg;

        let messageTemplate = $("#message-template").clone();
        messageTemplate.attr("id", "")
        messageTemplate.attr("class", "message-right")
        messageTemplate.find("span").text(encodedMsg);

        let scroll = $(`#messages-container-${connectionID}`).find(".messages-scroll")
        scroll.append(messageTemplate);
        //connection.find(".messages-scroll").append(messageTemplate);

        $("#message-input").val("");
        UpdateScroll(connectionID);
    }).catch((err) => {
        return console.error(err.toString());
    });

}

function AddMessage(messageObj) {
    messageObj = JSON.parse(messageObj);

    let currentConnectionID = $(".active-connection").data("id")
    let connectionID = messageObj.Connection.ID;
    if (currentConnectionID != connectionID) {
        let unreadDiv = $(`#connection-${connectionID}`).find(".unread");
        unreadDiv.removeClass("d-none");
    }

    // Clean the message of dangerous strings
    var msg = messageObj.Msg.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = msg;

    // Create the message element
    let li = document.createElement("li");
    li.setAttribute("class", "message-left mb-3");
    let span = document.createElement("span");
    span.setAttribute("class", "shadow-sm");
    span.textContent = encodedMsg;
    li.appendChild(span);

    $(`#messages-container-${connectionID}`).find(".messages-scroll").append(li)

    $(`#connection-${connectionID}`).find(".last-received").text(encodedMsg)

    UpdateScroll(connectionID);
    console.log("Adding Message: " + message);
}

function ReadAllMessages(connectionID) {
    $.post("/Messaging/ReadAllMessages", { connectionID: connectionID, user: $("#username").text() })
        .done((data, status, jqXHR) => {
            let unreadDiv = $(`#connection-${connectionID}`).find(".unread");
            unreadDiv.addClass("d-none");
        }).fail((jqXHR, status, error) => {
            console.error(error)
        })

    //signalRConnection.invoke("ReadAllMessages", connectionID).then(() => {
    //    let unreadDiv = $(`#connection-${connectionID}`).find(".unread");
    //    unreadDiv.addClass("d-none");

    //}).catch((err) => {
    //    console.error(err);
    //});
}

function UpdateMessagesView(connectionID) {
    // Hide other windows
    $(".connection-container").each((idx, obj) => {

        if ($(obj).attr("id") != `messages-container-${connectionID}`) {
            $(obj).removeClass("d-flex")
            $(obj).addClass("d-none")
        } else {
            $(obj).removeClass("d-none")
            $(obj).addClass("d-flex")
        }
    })
}

function UpdateScroll(connectionID) {
    let messageScroll = $(`#messages-container-${connectionID}`).find(".messages-scroll")
    messageScroll.scrollTop(messageScroll.get(0).scrollHeight)
}
