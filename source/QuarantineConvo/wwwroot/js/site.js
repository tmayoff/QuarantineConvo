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

let messageNotifications = []

let notificationTemplate = `
<div class="bg-light shadow-lg d-flex flex-row align-items-start" data-notify="container">
    <div class="d-flex flex-column flex-grow-1" >
        <h3 data-notify="title">{1}</h3>
        <p class="lead" data-notify="message">{2}</p>
        <span>Click here to view it</span>
        <a class="btn" href="{3}" target="{4}" data-notify="url"></a>
        </div >
    <button class="btn d-xl-flex" type="button" data-notify="dismiss"><i class="fas fa-times"></i></button>
</div >`

$(document).ready(() => {

    // Permissions
    AskForNotificationPermissions();

    // Notifications
    signalRConnection.on("ReceiveNotification", (notification) => {

        $.notify(
            {
                title: `<strong>New Connection with</strong>`,
                message: ""
            },
            {
                url_target: "_self",
                placement: { from: "bottom", align: "right" },
                mouse_over: "pause",
                template: notificationTemplate
            });
    });

    signalRConnection.on("ReceiveMessage", (msg) => {
        if (typeof AddMessage === "function") {
            // On the messaging page
            AddMessage(msg)
        } else {
            // Any where else on the site
            msg = JSON.parse(msg)
            $.notify(
                {
                    title: `<strong>Message received from: ${msg.SentBy}</strong>`,
                    message: msg.Msg.substring(0, 50) + "...",
                    url: `/messaging?connectionID=${msg.Connection.ID}`

                },
                {
                    url_target: "_self",
                    placement: { from: "bottom", align: "right" },
                    mouse_over: "pause",
                    template: notificationTemplate
                });
            if (Notification.permission == "granted") {
                let note = new Notification(`New messages from: ${msg.SentBy}`, { body: msg.Msg, icon: "/img/bell-solid.svg" });
                messageNotifications.push(note)
            }
        }
    });
})

function AskForNotificationPermissions() {
    if (!('Notification') in window) {
        console.log("Browser doesn't support notifications")
        return
    }

    Notification.requestPermission().then((permission) => {
        // Store the permission if not already
        if (!('permission') in Notification) {
            Notification.permission = permission;
        }
    })
}

function ConnectSignalR() {
    signalRConnection = new signalR.HubConnectionBuilder().withUrl("/messaging-hub").build();
    signalRConnection.start().then(() => {
        connectionEstablished = true;
    }).catch((err) => {
        return console.error(err.toString());
    });
}