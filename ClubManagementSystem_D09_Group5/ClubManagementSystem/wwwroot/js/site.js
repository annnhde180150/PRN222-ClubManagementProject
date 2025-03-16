// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var connection = new signalR.HubConnectionBuilder().withUrl("/serverHub").build();

connection.start().then(function () {
    console.log("Connected!");
    //connection.invoke("GetServerTime").catch(function (err) { console.error(err.toString()); });
}).catch(function (err) { console.log(err) });

connection.on("notifyNews", function (notiID) {
    console.log("notifyNews");
    LoadNotifications(notiID);
})

LoadNotifications(notiID){
    
}