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

    connection.on("ReceiveNotification", (message) => {
        console.log("RECEIVING");
        $('#notificationModelLabel').html('New Notification!');
        $('#notificationModelContent').html(message);
        $('#notificationModel').modal('toggle')
    });

})
