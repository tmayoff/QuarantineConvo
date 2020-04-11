// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var connectionEstablished = false;

// Setup SignalR connection
var signalRConnection = new signalR.HubConnectionBuilder().withUrl("/messaging-hub").build();
signalRConnection.start().then(() => {
    connectionEstablished = true;
}).catch((err) => {
    return console.error(err.toString());
});

$(document).ready(() => {

    // Notifications
    signalRConnection.on("ReceiveNotification", (msg) => {
        console.log("Received notification");

        $.notify(msg);
        //$.notify({ title: "Notification", message: msg }, { position: bottom - right });

        //$('#notificationModelLabel').html('New Notification!');
        //$('#notificationModelContent').html(message);
        //$('#notificationModel').modal('toggle')
    });

    signalRConnection.on("ReceiveMessage", (msg) => {
        if (typeof AddMessage === "function") {
            // On the messaging page
            AddMessage(msg)
        } else {
            // Any where else on the site
            //$.notify("You received a message from someone");
            msg = JSON.parse(msg)
            $.notify(
                {
                    title: `<strong>Message received from ${msg.SentBy}:</strong>`,
                    message: msg.Msg
                },
                { placement: { from: "bottom", align: "right" } });
        }
    });
})
