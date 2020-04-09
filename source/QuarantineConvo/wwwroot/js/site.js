// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connectionEstablished = false;

// Setup SignalR connection
var connection = new signalR.HubConnectionBuilder().withUrl("/messaging-hub").build();
connection.start().then(() => {
    connectionEstablished = true;
}).catch((err) => {
    return console.error(err.toString());
});

$(document).ready(() => {

    // Notifications
    connection.on("ReceiveNotification", (msg) => {
        console.log("Received notification");

        $.notify(msg);
        //$.notify({ title: "Notification", message: msg }, { position: bottom - right });

        //$('#notificationModelLabel').html('New Notification!');
        //$('#notificationModelContent').html(message);
        //$('#notificationModel').modal('toggle')
    });

    connection.on("ReceiveMessage", (msg, id) => {
        if (typeof AddMessage === "function") {
            // On the messaging page
            AddMessage(msg)
            connection.invoke("ReadMessage", id).catch((err) => {
                console.error(err);
            });
        } else {
            // Any where else on the site
            //$.notify("You received a message from someone");
            $.notify(
                { title: "<strong>Message received:</strong>" },
                { placement: { from: "bottom", align: "right" } });
        }
    });
})
