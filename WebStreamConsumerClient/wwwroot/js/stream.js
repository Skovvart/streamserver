"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/stream").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    connection.stream("BeginStream", user).subscribe({
        next: (item) => {
            var li = document.createElement("li");
            li.textContent = item.message;
            document.getElementById("messagesList").appendChild(li);
        },
        error: (err) => {
            var li = document.createElement("li");
            li.textContent = err;
            document.getElementById("messagesList").appendChild(li);
        }
    });

    event.preventDefault();
}
);