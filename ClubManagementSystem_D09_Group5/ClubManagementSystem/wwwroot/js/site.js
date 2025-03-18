// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var connection = new signalR.HubConnectionBuilder().withUrl("/serverHub").build();

connection.start().then(function () {
    //connection.invoke("GetServerTime").catch(function (err) { console.error(err.toString()); });
}).catch(function (err) { console.log(err) });

connection.on("notifyNews", function (noti) {
    console.log("notifyNews");
    LoadNotifications(noti);
    LoadNotificationsIcon();
})

function LoadNotificationsIcon(){
    document.getElementById("notiDot").style.display = "block";
};

function LoadNotifications(noti) {
    var newNoti = `<div id="gridRow-${noti.notificationId}" class="row py-3 border-bottom notification-item unread-notification"">
                        <div class="col-1 d-flex align-items-center justify-content-center">
                            <input type="checkbox" class="form-check-input" value="${noti.isRead}" checked disabled>
                        </div>
                        <div class="col-3 text-center d-flex align-items-center"><span>${noti.createdAt}</span></div>
                        <div class="col-3 d-flex align-items-center"><span>${noti.location}</span></div>
                        <div class="col-5 d-flex align-items-center"><span>${noti.message}</span></div>
                    </div>`;
    document.getElementById("gridBody").insertAdjacentHTML('afterbegin', newNoti);
}